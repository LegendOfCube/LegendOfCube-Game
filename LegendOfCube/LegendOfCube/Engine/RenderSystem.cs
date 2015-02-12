using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class RenderSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        private static readonly Properties TRANSFORM = new Properties(Properties.TRANSFORM);
		private static readonly Properties MODEL_AND_TRANSFORM = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;
		private Vector3 oldCamPos;
		private Matrix view, projection;

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
		}

		public void UpdateCamera(World world)
		{
			Matrix playerTransform = new Matrix();
			InputData input = null;
			//Find player
			foreach (Entity e in world.EnumerateEntities(new Properties(Properties.INPUT_FLAG)))
			{
				playerTransform = world.Transforms[e.Id];
				input = world.InputData[e.Id];
				break;
			}

			Vector3 backward = playerTransform.Backward;
			backward.Normalize();
			Vector3 up = playerTransform.Up;
			up.Normalize();
			Vector3 defaultCameraPos = playerTransform.Translation + backward * 3.0f + up * 2.5f;
			Vector3 newCamPos;

			if (input == null || input.GetCameraDirection().Equals(new Vector2(0, 0)))
			{ // If the user does not give any input about camera the camera position is calculated.
				Vector3 interpolatedCamPos = interpolate(oldCamPos, defaultCameraPos, 50);
				interpolatedCamPos.Normalize();
				newCamPos = playerTransform.Translation - interpolatedCamPos * 3.0f + up * 2.5f;
			}
			else
			{ // The user defines the cameras position
				Vector3 userCamera = new Vector3(input.GetCameraDirection().X, 0, input.GetCameraDirection().Y);
				userCamera.Normalize();
				userCamera.X = -userCamera.X;
				newCamPos = playerTransform.Translation - userCamera * 3.0f + up * 2.5f;
			}

			Vector3 camTarget = playerTransform.Translation;
			float fov = 90; // TODO: Should be definable

			view = Matrix.CreateLookAt(newCamPos, camTarget, up);
			projection = Matrix.CreatePerspectiveFieldOfView(
									MathHelper.ToRadians(fov),
									game.GraphicsDevice.Viewport.AspectRatio,
									0.1f,
									1000.0f);

			oldCamPos = newCamPos;
		}


		public void DrawEntities(World world)
		{
			foreach (Entity e in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				world.Models[e.Id].Draw(world.Transforms[e.Id], view, projection);
			}
		}

		// Returns a vector rotated a certain amount around the Y-axis.
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
			float rotationAngle = (float) alpha * factor;

			// Creates a matrix and rotates the input vector
			Matrix rotate = Matrix.CreateRotationY(rotationAngle);
			Vector3 interpolated = Vector3.Transform(oldVec, rotate);

			return interpolated;
		}
	}
}
