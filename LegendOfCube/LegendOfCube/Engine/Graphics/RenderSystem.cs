﻿using System;
﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		private static readonly Properties FULL_LIGHT_EFFECT = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM |
		                                                                Properties.FULL_LIGHT_EFFECT);

		public static readonly Vector4 LIGHT_COLOR = Color.White.ToVector4();

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;
		private StandardEffect standardEffect;

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

			this.standardEffect = new StandardEffect(game.Content.Load<Effect>("Effects/standardEffect"));
		}

		public void RenderWorld(World world)
		{
			Matrix playerTransform = new Matrix();
			Vector3 playerPos = new Vector3();
			InputData input = null;
			//Find player
			foreach (Entity e in world.EnumerateEntities(new Properties(Properties.INPUT_FLAG)))
			{
				playerTransform = world.Transforms[e.Id];
				playerPos = playerTransform.Translation;
				input = world.InputData[e.Id];
				break;
			}


			Vector3 camTarget = playerPos;
			Vector3 up = new Vector3(0, 1, 0);
			float fov = 90;

			Vector3 camPos = world.CameraPosition;
			Matrix view = Matrix.CreateLookAt(camPos, camTarget, up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(fov),
			                        game.GraphicsDevice.Viewport.AspectRatio,
			                        0.1f,
			                        1000.0f);
			oldCamPos = camPos;


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

			// Not exactly sure about the reason for this, but seems to be the standard way to do it
			var transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(transforms);

			if (world.EntityProperties[entity.Id].Satisfies(FULL_LIGHT_EFFECT))
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

			foreach (var mesh in model.Meshes)
			{
				foreach (var meshPart in mesh.MeshParts)
				{
					// TODO: Move from here. Ugly and unnecessary to do all the time.
					// Replace with standard effect
					meshPart.Effect = standardEffect.Effect;
				}
				var worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
				standardEffect.SetWorld(ref worldMatrix);
				mesh.Draw();
			}
		}

		private void RenderWithStandardEffect(Entity entity, World world, Matrix view, Matrix projection)
		{
			var sep = world.StandardEffectParams[entity.Id];

			standardEffect.SetDiffuseColor(sep.DiffuseColor);
			standardEffect.SetSpecularColor(sep.SpecularColor);
			standardEffect.SetEmissiveColor(sep.EmissiveColor);

			standardEffect.SetDiffuseTexture(sep.DiffuseTexture);
			standardEffect.SetEmissiveTexture(sep.EmissiveTexture);
			standardEffect.SetSpecularTexture(sep.SpecularTexture);
			standardEffect.SetNormalTexture(sep.NormalTexture);
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
