﻿using System;
﻿using System.Collections.Generic;
﻿using System.Diagnostics;
﻿using LegendOfCube.Engine.BoundingVolumes;
﻿using Microsoft.Xna.Framework;
﻿using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	public class RenderSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		private static readonly Properties TRANSFORM = new Properties(Properties.TRANSFORM);
		private static readonly Properties MODEL_AND_TRANSFORM = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM);

		private static readonly Properties STANDARD_EFFECT_COMPATIBLE = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM |
		                                                                Properties.STANDARD_EFFECT);

		private static readonly Properties HAS_OBB = new Properties(Properties.MODEL_SPACE_BV);
		private static readonly Properties NO_SHADOW_CAST = new Properties(Properties.NO_SHADOW_CAST_FLAG);
		private static readonly Properties NO_SHADOW_RECEIVE = new Properties(Properties.NO_SHADOW_RECEIVE_FLAG);

		private static readonly Vector3 OCCLUDED_EFFECT_COLOR = new Color(0x00,0x94,0xaa).ToVector3();
		private const int SHADOW_MAP_SIZE = 2048;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly Game game;
		private GraphicsDevice graphicsDevice;
		private GraphicsDeviceManager graphicsManager;
		private StandardEffect standardEffect;
		private OBBRenderer obbRenderer;

		private DepthStencilState renderOccludedState;
		private BasicEffect occludedEffect;

		private RenderTarget2D shadowRenderTarget0;
		private RenderTarget2D shadowRenderTarget1;

		private RasterizerState shadowMapRasterizerState;

		// Store an array that's reused for each entity
		// (very high allocation count when profiling otherwise)
		private Matrix[] boneTransforms = new Matrix[5];

		private readonly List<Entity> renderableEntities = new List<Entity>();
		private readonly List<Entity> visibleEntities = new List<Entity>();
		private bool[] standardEffectApplied;


		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game, GraphicsDeviceManager graphicsDeviceManager)
		{
			this.game = game;
			this.graphicsManager = graphicsDeviceManager;
		}

		public void LoadContent()
		{
			this.graphicsDevice = game.GraphicsDevice;
			obbRenderer = new OBBRenderer(graphicsDevice);

			shadowRenderTarget0 = CreateShadowMapTarget();
			shadowRenderTarget1 = CreateShadowMapTarget();

			// DepthStencilState with reversed depth test, used for
			// showing object only if it's occluded
			renderOccludedState = new DepthStencilState
			{
				DepthBufferFunction = CompareFunction.Greater,
				DepthBufferWriteEnable = false
			};

			// Effect used for rendering the player when it's occluded
			occludedEffect = new BasicEffect(graphicsDevice)
			{
				PreferPerPixelLighting = false,
				DiffuseColor = OCCLUDED_EFFECT_COLOR,
				VertexColorEnabled = false,
				TextureEnabled = false,
			};

			// For front-face culling when rendering shadow map, which
			// is one method for alleviating precision problems
			shadowMapRasterizerState = new RasterizerState
			{
				CullMode = CullMode.CullClockwiseFace
			};

			this.standardEffect = StandardEffect.LoadEffect(game.Content);
		}

		private RenderTarget2D CreateShadowMapTarget()
		{
			return new RenderTarget2D
			(
				graphicsDevice,
				SHADOW_MAP_SIZE,
				SHADOW_MAP_SIZE,
				false,
				SurfaceFormat.Single,
				DepthFormat.Depth24
			);
		}

		public void RenderWorld(World world)
		{
			Matrix cameraView = Matrix.CreateLookAt(world.Camera.Position, world.Camera.Target, world.Camera.Up);
			Matrix cameraProjection = Matrix.CreatePerspectiveFieldOfView(
			                              MathHelper.ToRadians(world.Camera.Fov),
			                              game.GraphicsDevice.Viewport.AspectRatio,
			                              0.1f,
			                              5000.0f);

			// View frustrum culling is disabled, due to problems with generated BoundingSpheres
			// TODO: Fix or completely remove
			//var boundingFrustum = new BoundingFrustum(cameraView * cameraProjection);

			// Filter out a list of interesting entities to be used in different steps
			// (Value types such as Enity won't be autoboxed in List<Entity>)
			renderableEntities.Clear();
			visibleEntities.Clear();
			foreach (var entity in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				renderableEntities.Add(entity);

				// View frustrum culling is disabled, due to problems with generated BoundingSpheres
				// TODO: Fix or completely remove
				/*Model model = world.Models[entity.Id];
				Matrix worldTransform = world.Transforms[entity.Id];
				if (IsModelInFrustrum(model, boundingFrustum, ref worldTransform))
				{
					visibleEntities.Add(entity);
				}*/
				visibleEntities.Add(entity);
			}

			if (standardEffectApplied == null || standardEffectApplied.Length < world.MaxNumEntities)
			{
				standardEffectApplied = new bool[world.MaxNumEntities];
			}

			standardEffect.PrepareRendering();

			// Create shadow map for the primary light
			if (GlobalConfig.Instance.ShowShadows)
			{
				Matrix shadowMatrix0;
				Matrix shadowMatrix1;
				RenderShadowMap(world, renderableEntities, 160, 160, shadowRenderTarget0, out shadowMatrix0);
				RenderShadowMap(world, renderableEntities, 400, 400, shadowRenderTarget1, out shadowMatrix1);

				standardEffect.SetDirLight0ShadowMap0(shadowRenderTarget0);
				standardEffect.SetDirLight0ShadowMatrix0(ref shadowMatrix0);

				standardEffect.SetDirLight0ShadowMap1(shadowRenderTarget1);
				standardEffect.SetDirLight0ShadowMatrix1(ref shadowMatrix1);
			}

			// For some reason, it seems that changing the render target will undo the previous clear
			game.GraphicsDevice.Clear(Color.CornflowerBlue);

			// Render all visible entities in the world
			RenderFinal(world, visibleEntities, ref cameraView, ref cameraProjection);

			// Render OBB wireframes
			if (world.DebugState.ShowOBBWireFrame)
			{
				RenderOBBs(world, ref cameraView, ref cameraProjection);
			}
		}

		private void RenderShadowMap(World world, List<Entity> entities, float width, float height, RenderTarget2D renderTarget, out Matrix shadowMatrix)
		{
			RenderTargetBinding[] origRenderTargets = new RenderTargetBinding[game.GraphicsDevice.GetRenderTargets().Length];
			RasterizerState origRasterizerState = game.GraphicsDevice.RasterizerState;
			game.GraphicsDevice.GetRenderTargets().CopyTo(origRenderTargets, 0);
			game.GraphicsDevice.SetRenderTarget(renderTarget);
			game.GraphicsDevice.Clear(Color.White);
			game.GraphicsDevice.RasterizerState = shadowMapRasterizerState;
			standardEffect.SetShadowMapRendering(true);

			// The shadow map is based on an orthographic projection, that could
			// be thought of as a plane that's a fixed distance from the player,
			// with its normal parallel to the light direction and pointed toward
			// the camera position.
			//
			// It won't alwayes be centered on the camera, but rather some rounding
			// is performed to reduse the amount of flickering

			Vector3 lightTarget = world.Camera.Position;
			Vector3 lightDir = world.DirLight.Direction;

			// Use world up if light isn't pointed up or down
			Vector3 lightUp = Math.Abs(Vector3.Dot(lightDir, Vector3.Up)) <= 0.95f ? Vector3.Up : Vector3.Forward;

			// Round of target to somewhere on grid, which is sort-of aligned with
			// the camera view space

			// A higher value gives more flickering, a lower gives more pop-in
			const float GRID_SIZE = 10.0f; 

			Vector3 gridX = Vector3.Normalize(Vector3.Cross(lightUp, lightDir));
			Vector3 gridY = Vector3.Cross(gridX, lightDir);

			float targetGridX = Vector3.Dot(gridX, lightTarget);
			float targetGridY = Vector3.Dot(gridY, lightTarget);

			float targetGridXRound = GRID_SIZE * (float)Math.Round(targetGridX / GRID_SIZE);
			float targetGridYRound = GRID_SIZE * (float)Math.Round(targetGridY / GRID_SIZE);

			lightTarget -= gridX * targetGridX;
			lightTarget -= gridY * targetGridY;

			lightTarget += gridX * targetGridXRound;
			lightTarget += gridY * targetGridYRound;

			Vector3 lightSourcePos = lightTarget - 1500.0f * world.DirLight.Direction;

			Matrix lightView = Matrix.CreateLookAt(lightSourcePos, lightTarget, lightUp);
			Matrix lightProjection = Matrix.CreateOrthographic(width, height, 100.0f, 3000.0f);

			standardEffect.SetViewProjection(ref lightView, ref lightProjection);

			// View frustrum culling is disabled, due to problems with generated BoundingSpheres
			// TODO: Fix or completely remove
			//var boundingFrustum = new BoundingFrustum(lightView * lightProjection);

			foreach (var entity in entities)
			{
				Properties properties = world.EntityProperties[entity.Id];
				if (!properties.Satisfies(STANDARD_EFFECT_COMPATIBLE) || properties.Satisfies(NO_SHADOW_CAST))
				{
					continue;
				}
				var model = world.Models[entity.Id];
				var worldTransform = world.Transforms[entity.Id];

				// View frustrum culling is disabled, due to problems with generated BoundingSpheres
				// TODO: Fix or completely remove
				/*
				// Don't render if entity wouldn't be seen
				if (!IsModelInFrustrum(model, boundingFrustum, ref worldTransform))
				{
					continue;
				}
				*/

				Matrix[] transforms = GetTransformsForModel(model);
				if (world.EntityProperties[entity.Id].Satisfies(STANDARD_EFFECT_COMPATIBLE))
				{
					RenderModelWithStandardEffect(entity, model, transforms, ref worldTransform);
				}
				else
				{
					RenderEntityWithBasicEffect(entity, model, transforms, world, ref worldTransform, ref lightView, ref lightProjection);
				}
			}
			game.GraphicsDevice.RasterizerState = origRasterizerState;
			game.GraphicsDevice.SetRenderTargets(origRenderTargets);
			standardEffect.SetShadowMapRendering(false);
			shadowMatrix = lightView * lightProjection;
		}

		private Matrix[] GetTransformsForModel(Model model)
		{
			int boneCount = model.Bones.Count;
			// Reuse the same array if larger array isn't needed
			Matrix[] transforms = boneCount <= boneTransforms.Length ? boneTransforms : new Matrix[boneCount];
			// Not exactly sure about the reason for this, but seems to be the standard way to do it
			model.CopyAbsoluteBoneTransformsTo(transforms);
			return transforms;
		}

		private void RenderFinal(World world, List<Entity> entities, ref Matrix view, ref Matrix projection)
		{
			var finalAmbientColor = new Vector4(world.AmbientIntensity * world.AmbientColor, 1.0f);
			var finalDir0Color = new Vector4(world.DirLight.Intensity * world.DirLight.Color, 1.0f);

			standardEffect.SetViewProjection(ref view, ref projection);
			standardEffect.SetAmbientColor(finalAmbientColor);

			standardEffect.SetApplyShadows(GlobalConfig.Instance.ShowShadows);
			standardEffect.SetDirLight0Properties(ref world.DirLight.Direction, ref finalDir0Color);


			standardEffect.SetPointLight0Properties(ref world.PointLight0.LightPosition, ref world.PointLight0.Reach, ref world.PointLight0.Color);

			foreach (var entity in entities)
			{
				// Filter out player, need to be rendered last for occlusion effect
				if (entity.Id == world.Player.Id)
				{
					continue;
				}
				RenderEntity(entity, world, ref view, ref projection);
			}

			// Render player
			RenderPlayer(world, ref view, ref projection);
		}

		private void RenderPlayer(World world, ref Matrix view, ref Matrix projection)
		{
			var originalDepthState = graphicsDevice.DepthStencilState;
			graphicsDevice.DepthStencilState = renderOccludedState;
			
			occludedEffect.View = view;
			occludedEffect.Projection = projection;

			Model model = world.Models[world.Player.Id];
			var worldTransform = world.Transforms[world.Player.Id];
			Matrix[] transforms = GetTransformsForModel(model);

			// Draw with solid color BasicEffect what will be seen through walls
			GraphicsUtils.ApplyEffectOnModel(model, occludedEffect);
			standardEffectApplied[world.Player.Id] = false;
			foreach (var mesh in model.Meshes)
			{
				var worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
				occludedEffect.World = worldMatrix;
				mesh.Draw();
			}

			graphicsDevice.DepthStencilState = originalDepthState;

			// Render player normally
			RenderEntity(world.Player, world, ref view, ref projection);
		}

		private void RenderEntity(Entity entity, World world, ref Matrix view, ref Matrix projection)
		{
			var model = world.Models[entity.Id];
			var worldTransform = world.Transforms[entity.Id];

			Matrix[] transforms = GetTransformsForModel(model);
			if (world.EntityProperties[entity.Id].Satisfies(STANDARD_EFFECT_COMPATIBLE))
			{
				RenderEntityWithStandardEffect(entity, model, transforms, world, ref worldTransform);
			}
			else
			{
				RenderEntityWithBasicEffect(entity, model, transforms, world, ref worldTransform, ref view, ref projection);
			}
		}


		private void RenderEntityWithStandardEffect(Entity entity, Model model, Matrix[] transforms, World world, ref Matrix worldTransform)
		{
			var sep = world.StandardEffectParams[entity.Id];

			standardEffect.SetDiffuseColor(sep.DiffuseColor);
			standardEffect.SetSpecularColor(sep.SpecularColor);
			standardEffect.SetEmissiveColor(sep.EmissiveColor);

			standardEffect.SetDiffuseTexture(sep.DiffuseTexture);
			standardEffect.SetEmissiveTexture(sep.EmissiveTexture);
			standardEffect.SetSpecularTexture(sep.SpecularTexture);
			standardEffect.SetNormalTexture(sep.NormalTexture);

			bool noShadowReceive = !GlobalConfig.Instance.ShowShadows || world.EntityProperties[entity.Id].Satisfies(NO_SHADOW_RECEIVE);
			standardEffect.SetApplyShadows(!noShadowReceive);

			RenderModelWithStandardEffect(entity, model, transforms, ref worldTransform);
		}

		private void RenderModelWithStandardEffect(Entity entity, Model model, Matrix[] transforms, ref Matrix worldTransform)
		{
			if (!standardEffectApplied[entity.Id])
			{
				standardEffect.ApplyOnModel(model);
				standardEffectApplied[entity.Id] = true;
			}
			foreach (var mesh in model.Meshes)
			{
				Matrix worldMatrix;
				Matrix.Multiply(ref transforms[mesh.ParentBone.Index], ref worldTransform, out worldMatrix);
				standardEffect.SetWorld(ref worldMatrix);
				mesh.Draw();
			}
		}

		private void RenderEntityWithBasicEffect(Entity entity, Model model, Matrix[] transforms, World world, ref Matrix worldTransform, ref Matrix view, ref Matrix projection)
		{
			foreach (var mesh in model.Meshes)
			{
				// Make it still possible to render with the default BasicEffect
				foreach (var effect in mesh.Effects)
				{
					var basicEffect = effect as BasicEffect;
					if (basicEffect != null)
					{
						basicEffect.World = transforms[mesh.ParentBone.Index] * worldTransform;
						basicEffect.View = view;
						basicEffect.Projection = projection;
					}
					else
					{
						Debug.Assert(false, "Model for entity has unknown effect");
					}
				}
				mesh.Draw();
			}
		}

		private void RenderOBBs(World world, ref Matrix view, ref Matrix projection)
		{
			foreach (var entity in world.EnumerateEntities(HAS_OBB))
			{
				OBB obb = world.ModelSpaceBVs[entity.Id];
				OBB transformed = OBB.TransformOBB(ref obb, ref world.Transforms[entity.Id]);
				obbRenderer.Render(ref transformed, ref view, ref projection);
			}
		}

		private static bool IsModelInFrustrum(Model model, BoundingFrustum boundingFrustum, ref Matrix worldTransform)
		{

			BoundingSphere boundingSphere;
			CreateMergedBoundingSphere(model, out boundingSphere);

			// Not entirely sure if model.Root.Transform should be used in
			// this context, but it seems like mesh.BoundingSphere doesn't
			// cover the whole object otherwise. The bug might lie
			// elsewhere.
			var modifiedWorldTransform = model.Root.Transform * worldTransform;
			BoundingSphere worldBoundingSphere;
			boundingSphere.Transform(ref modifiedWorldTransform, out worldBoundingSphere);
			bool intersects;
			boundingFrustum.Intersects(ref worldBoundingSphere, out intersects);
			return intersects;
		}

		private static void CreateMergedBoundingSphere(Model model, out BoundingSphere boundingSphere)
		{
			boundingSphere = model.Meshes[0].BoundingSphere;
			for (int i = 1; i < model.Meshes.Count; i++)
			{
				ModelMesh mesh = model.Meshes[i];
				// As with model.Root.Transform, it may be that this 
				// mesh.ParentBone.Transform shouldn't be used
				BoundingSphere additional = mesh.BoundingSphere.Transform(mesh.ParentBone.Transform);
				boundingSphere = BoundingSphere.CreateMerged(boundingSphere, additional);
			}
		}
	}
}
