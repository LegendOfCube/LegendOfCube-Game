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

		public void DrawEntities(World world)
		{
			Matrix playerTransform = new Matrix();
			//Find player
			foreach (Entity e in world.EnumerateEntities(new Properties(Properties.INPUT_FLAG)))
			{
				playerTransform = world.Transforms[e.Id];
				break;
			}

			Vector3 backward = playerTransform.Backward;
			backward.Normalize();
			Vector3 up = playerTransform.Up;
			up.Normalize();
			Vector3 camPos = playerTransform.Translation + backward*3.0f + up*1.5f;
			Vector3 camTarget = playerTransform.Translation;
			Vector3 upVec = new Vector3(0, 1, 0);
			float fov = 75;

			Matrix view = Matrix.CreateLookAt(camPos, camTarget, upVec);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(fov),
			                        game.GraphicsDevice.Viewport.AspectRatio,
			                        0.1f,
			                        1000.0f);

			foreach (Entity e in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				world.Models[e.Id].Draw(world.Transforms[e.Id], view, projection);
			}
		}
	}
}
