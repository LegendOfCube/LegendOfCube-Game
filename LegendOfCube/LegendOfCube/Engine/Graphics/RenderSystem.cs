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

		private static readonly Vector4 LIGHT_COLOR = Color.White.ToVector4();
		private const int SHADOW_MAP_SIZE = 2048;
		private const float FOV = 70;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly Game game;
		private GraphicsDevice graphicsDevice;
		private GraphicsDeviceManager graphicsManager;
		private StandardEffect standardEffect;
		private OBBRenderer obbRenderer;
		private RenderTarget2D shadowRenderTarget;

		// Store an array that's reused for each entity
		// (very high allocation count when profiling otherwise)
		private Matrix[] boneTransforms = new Matrix[5];

		private readonly List<Entity> renderableEntities = new List<Entity>();
		private readonly List<Entity> visibleEntities = new List<Entity>();

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game, GraphicsDeviceManager graphicsDeviceManager)
		{
			this.game = game;
			this.graphicsManager = graphicsDeviceManager;
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void Initialize()
		{
			this.graphicsDevice = game.GraphicsDevice;
			obbRenderer = new OBBRenderer(graphicsDevice);
			shadowRenderTarget = CreateShadowMapTarget();
		}

		public void LoadContent()
		{
			this.standardEffect = StandardEffect.LoadEffect(game.Content);
		}

		private RenderTarget2D CreateShadowMapTarget()
		{
			// Create our render target
			return new RenderTarget2D(graphicsDevice,
				SHADOW_MAP_SIZE,
				SHADOW_MAP_SIZE,
				false,
				SurfaceFormat.Single,
				DepthFormat.Depth24
			);
		}

		public void RenderWorld(World world)
		{
			Vector3 cameraTarget = world.Transforms[world.Player.Id].Translation;
			Matrix cameraView = Matrix.CreateLookAt(world.CameraPosition, cameraTarget, Vector3.Up);
			Matrix cameraProjection = Matrix.CreatePerspectiveFieldOfView(
			                              MathHelper.ToRadians(FOV),
			                              game.GraphicsDevice.Viewport.AspectRatio,
			                              0.1f,
			                              5000.0f);

			var boundingFrustum = new BoundingFrustum(cameraView * cameraProjection);

			// Filter out a list of interesting entities to be used in different steps
			// (Value types such as Enity won't be autoboxed in List<Entity>)
			renderableEntities.Clear();
			visibleEntities.Clear();
			foreach (var entity in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				renderableEntities.Add(entity);
				Model model = world.Models[entity.Id];
				Matrix worldTransform = world.Transforms[entity.Id];
				if (IsModelInFrustrum(model, boundingFrustum, ref worldTransform))
				{
					visibleEntities.Add(entity);
				}
			}

			// Create shadow map for the primary light
			Matrix shadowMatrix;
			RenderShadowMap(world, renderableEntities, shadowRenderTarget, out shadowMatrix);

			// For some reason, it seems that changing the render target will undo the previous clear
			game.GraphicsDevice.Clear(Color.CornflowerBlue);

			// Render all visible entities in the world
			RenderFinal(world, visibleEntities, ref cameraView, ref cameraProjection, ref shadowMatrix);

			// Render OBB wireframes
			if (world.DebugState.ShowOBBWireFrame)
			{
				RenderOBBs(world, ref cameraView, ref cameraProjection);
			}
		}

		private void RenderShadowMap(World world, List<Entity> entities, RenderTarget2D renderTarget, out Matrix shadowMatrix)
		{
			RenderTargetBinding[] origRenderTargets = new RenderTargetBinding[game.GraphicsDevice.GetRenderTargets().Length];
			game.GraphicsDevice.GetRenderTargets().CopyTo(origRenderTargets, 0);
			game.GraphicsDevice.SetRenderTarget(shadowRenderTarget);
			game.GraphicsDevice.Clear(Color.Black);
			standardEffect.SetShadowMapRendering(true);

			// The shadow map is based on an orthographic projection that could
			// be thought of as a plane with a center that's a fixed distance
			// from the player with its normal parallel to the light direction
			// and pointed toward the player
			Vector3 lightTarget = world.Transforms[world.Player.Id].Translation;
			Matrix lightView = Matrix.CreateLookAt(lightTarget - 300 * world.LightDirection, lightTarget, Vector3.Forward);
			Matrix lightProjection = Matrix.CreateOrthographic(80.0f, 80.0f, 100.0f, 1000.0f);

			standardEffect.SetViewProjection(ref lightView, ref lightProjection);
			var boundingFrustum = new BoundingFrustum(lightView * lightProjection);

			foreach (var entity in entities)
			{
				if (!world.EntityProperties[entity.Id].Satisfies(STANDARD_EFFECT_COMPATIBLE))
				{
					continue;
				}
				var model = world.Models[entity.Id];
				var worldTransform = world.Transforms[entity.Id];

				// Don't render if entity wouldn't be seen
				if (!IsModelInFrustrum(model, boundingFrustum, ref worldTransform))
				{
					continue;
				}

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

		private void RenderFinal(World world, List<Entity> entities, ref Matrix view, ref Matrix projection, ref Matrix shadowMatrix)
		{
			standardEffect.SetViewProjection(ref view, ref projection);
			standardEffect.SetAmbientIntensity(world.AmbientIntensity);

			var lightColor = LIGHT_COLOR;
			standardEffect.SetDirLight0Properties(ref world.LightDirection, ref lightColor);
			standardEffect.SetDirLight0ShadowMap(shadowRenderTarget);
			standardEffect.SetDirLight0ShadowMatrix(ref shadowMatrix);

			// Make the player cube a light source
			float reach = 10.0f;
			bool playerHasStandardEffect = world.EntityProperties[world.Player.Id].Satisfies(STANDARD_EFFECT_COMPATIBLE);
			// Default to white color
			Vector4 pointColor = playerHasStandardEffect ? world.StandardEffectParams[world.Player.Id].EmissiveColor : Color.White.ToVector4();
			Vector3 pointLightPos = world.Transforms[world.Player.Id].Translation + new Vector3(0.0f, 0.5f, 0.0f);
			standardEffect.SetPointLight0Properties(ref pointLightPos, ref reach, ref pointColor);

			foreach (var entity in entities)
			{
				RenderEntity(entity, world, ref view, ref projection);
			}
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
				RenderEntityWithBasicEffect(entity, model, transforms, world, ref  worldTransform, ref view, ref projection);
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

			RenderModelWithStandardEffect(entity, model, transforms, ref worldTransform);
		}

		private void RenderModelWithStandardEffect(Entity entity, Model model, Matrix[] transforms, ref Matrix worldTransform)
		{
			standardEffect.ApplyOnModel(model);
			foreach (var mesh in model.Meshes)
			{
				var worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
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
			// Go through all BoundingSpheres in Model and check if inside frustrums
			foreach (var mesh in model.Meshes)
			{
				// Not entirely sure if model.Root.Transform should be used in
				// this context, but it seems like mesh.BoundingSphere doesn't
				// cover the whole object otherwise. The bug might lie
				// elsewhere.
				var modifiedWorldTransform = model.Root.Transform * worldTransform;
				BoundingSphere worldBoundingSphere;
				mesh.BoundingSphere.Transform(ref modifiedWorldTransform, out worldBoundingSphere);
				bool intersects;
				boundingFrustum.Intersects(ref worldBoundingSphere, out intersects);
				if (intersects)
				{
					return true;
				}
			}
			return false;
		}
	}
}
