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

		public static readonly Vector4 LIGHT_COLOR = Color.Orange.ToVector4();

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;
		private StandardEffect standardEffect;
		private Vector3 oldCamPos;

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

			Vector3 camPos = UpdateCamera(playerTransform, oldCamPos, playerPos, up, input);
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
			var lightStrength = 18.0f;
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

		private Vector3 UpdateCamera(Matrix playerTransform, Vector3 oldCamPos, Vector3 playerPos, Vector3 up, InputData input)
		{

			Vector3 defaultCameraPos = playerPos;
			Vector3 newCamPos;
			if (input == null || input.GetCameraDirection().Equals(new Vector2(0, 0)))
			{ // If the user does not give any input about camera the camera position is calculated.
				defaultCameraPos = playerPos + playerTransform.Backward * 3.0f + up * 2.5f;
				Vector3 interpolatedCamPos = interpolate(oldCamPos, defaultCameraPos, 50);
				interpolatedCamPos.Normalize();
				newCamPos = playerPos - interpolatedCamPos * 3.0f + up * 2.5f;
			}
			else
			{ // The user defines the cameras position
			  // TODO: Change to camera rotating when right stick used
				Vector3 userCamera = new Vector3(input.GetCameraDirection().X, 0, input.GetCameraDirection().Y);
				userCamera.Normalize();
				userCamera.X = -userCamera.X;
				newCamPos = playerTransform.Translation - userCamera * 3.0f + up * 2.5f;
			}

			return newCamPos;
		}


		// TODO: FIX interpolation
		private Vector3 interpolate(Vector3 oldVec, Vector3 newVec, float factor)
		{

			// Calculates the angle between the vectors (In 2D since we dont want to change the Y-coordinate)
			Vector2 a = new Vector2(oldVec.X, oldVec.Z);
			Vector2 b = new Vector2(newVec.X, newVec.Z);
			a.Normalize();
			b.Normalize();
			double dot = Vector2.Dot(a, b);
			if (dot >= 0.90 && dot <= 1.001f)
			{
				// Just returns the new vector if the angle between them is small enough
				return newVec;
			}

			// Creates an angle to rotate around
			double alpha = Math.Acos(dot);
			float rotationAngle = (float)alpha * factor;

			// Creates a matrix and rotates the input vector
			Matrix rotate = Matrix.CreateRotationY(rotationAngle);
			Vector3 interpolated = Vector3.Transform(oldVec, rotate);

			return interpolated;
		}


	}
}
