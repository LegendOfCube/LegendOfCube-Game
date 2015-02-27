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

		private static readonly Vector4 LIGHT_COLOR = Color.White.ToVector4();

		private const float FOV = 70;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;
		private StandardEffect standardEffect;
		private OBBRenderer obbRenderer;

		// Store an array that's reused for each entity
		// (very high allocation count when profiling otherwise)
		private Matrix[] boneTransforms = new Matrix[5];

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game)
		{
			this.game = game;
			graphics = new GraphicsDeviceManager(game);
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void Initialize()
		{
			game.Window.AllowUserResizing = true;
			graphics.PreferMultiSampling = true;

			game.GraphicsDevice.BlendState = BlendState.Opaque;
			game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			graphics.ApplyChanges();

			obbRenderer = new OBBRenderer(game.GraphicsDevice);

			this.standardEffect = StandardEffect.LoadEffect(game.Content);
		}

		public void RenderWorld(World world)
		{
			Vector3 camTarget = world.Transforms[world.Player.Id].Translation;
			Vector3 up = new Vector3(0, 1, 0);

			Vector3 camPos = world.CameraPosition;
			Matrix view = Matrix.CreateLookAt(camPos, camTarget, up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(FOV),
			                        game.GraphicsDevice.Viewport.AspectRatio,
			                        0.1f,
			                        1000.0f);


			standardEffect.SetViewProjection(ref view, ref projection);
			standardEffect.SetAmbientIntensity(0.1f);

			var lightColor = LIGHT_COLOR;
			var lightStrength = 25.0f;
			standardEffect.SetPointLight0Properties(ref world.LightPosition, ref lightStrength, ref lightColor);

			var boundingFrustum = new BoundingFrustum(view * projection);

			foreach (var e in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				RenderEntity(e, world, boundingFrustum, ref view, ref projection);
			}
		}

		private void RenderEntity(Entity entity, World world, BoundingFrustum boundingFrustum, ref Matrix view, ref Matrix projection)
		{
			var model = world.Models[entity.Id];
			var worldTransform = world.Transforms[entity.Id];

			// Don't render if entity wouldn't be seen
			if (!ModelInFrustrum(model, boundingFrustum, ref worldTransform))
			{
				return;
			}

			if (world.DebugState.ShowOBBWireFrame)
			{
				OBB obb = world.ModelSpaceBVs[entity.Id];
				OBB transformed = OBB.TransformOBB(ref obb, ref worldTransform);
				obbRenderer.Render(ref transformed, ref view, ref projection);
			}


			int boneCount = model.Bones.Count;
			// Reuse the same array if larger array isn't needed
			Matrix[] transforms = boneCount <= boneTransforms.Length ? boneTransforms : new Matrix[boneCount];
			// Not exactly sure about the reason for this, but seems to be the standard way to do it
			model.CopyAbsoluteBoneTransformsTo(transforms);

			if (world.EntityProperties[entity.Id].Satisfies(STANDARD_EFFECT_COMPATIBLE))
			{
				RenderWithStandardEffect(entity, model, transforms, world, worldTransform, view, projection);
			}
			else
			{
				RenderWithBasicEffect(entity, model, transforms, world, worldTransform, view, projection);
			}
		}

		private void RenderWithBasicEffect(Entity entity, Model model, Matrix[] transforms, World world, Matrix worldTransform, Matrix view, Matrix projection)
		{
			foreach (var mesh in model.Meshes)
			{
				// Make it still possible to render with the default BasicEffect
				foreach (var effect in mesh.Effects)
				{
					var basicEffect = effect as BasicEffect;
					if (basicEffect != null)
					{
						basicEffect.World = transforms[mesh.ParentBone.Index]*worldTransform;
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

		private void RenderWithStandardEffect(Entity entity, Model model, Matrix[] transforms, World world, Matrix worldTransform, Matrix view, Matrix projection)
		{
			var sep = world.StandardEffectParams[entity.Id];

			standardEffect.SetDiffuseColor(sep.DiffuseColor);
			standardEffect.SetSpecularColor(sep.SpecularColor);
			standardEffect.SetEmissiveColor(sep.EmissiveColor);

			standardEffect.SetDiffuseTexture(sep.DiffuseTexture);
			standardEffect.SetEmissiveTexture(sep.EmissiveTexture);
			standardEffect.SetSpecularTexture(sep.SpecularTexture);
			standardEffect.SetNormalTexture(sep.NormalTexture);

			standardEffect.ApplyOnModel(model);
			standardEffect.SetShadowMapRendering(true);

			foreach (var mesh in model.Meshes)
			{
				var worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
				standardEffect.SetWorld(ref worldMatrix);
				mesh.Draw();
			}
		}

		private static bool ModelInFrustrum(Model model, BoundingFrustum boundingFrustum, ref Matrix worldTransform)
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
