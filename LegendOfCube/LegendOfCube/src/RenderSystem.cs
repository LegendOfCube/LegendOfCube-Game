using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	class RenderSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		private static readonly ComponentMask POS_AND_TRANSFORM = new ComponentMask(
																      ComponentMask.POSITION |
																      ComponentMask.TRANSFORM);
		private static readonly ComponentMask MODEL_AND_TRANSFORM = new ComponentMask(
		                                                                ComponentMask.MODEL |
																		ComponentMask.TRANSFORM);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game _game;
		private GraphicsDeviceManager _graphics;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game)
		{
			_game = game;
			_graphics = new GraphicsDeviceManager(game);
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void Initialize()
		{
			_game.Window.AllowUserResizing = true;
			_graphics.PreferMultiSampling = true;
			_game.GraphicsDevice.BlendState = BlendState.Opaque;
			_game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			_graphics.ApplyChanges();
		}

		public void updateTranslationTransforms(World world)
		{
			for (UInt32 i = 0; i < world.MAX_NUM_ENTITIES; i++) {
				if (world.ComponentMasks[i].satisfies(POS_AND_TRANSFORM)) {
					world.Transforms[i].Translation = world.Positions[i];
				}
			}
		}

		public void DrawEntities(World world)
		{
			Vector3 camPos = new Vector3(0, 1, -5);
			Vector3 camTarget = new Vector3(0, 0, 0);
			Vector3 up = new Vector3(0, 1, 0);
			float fov = 75;

			Matrix view = Matrix.CreateLookAt(camPos, camTarget, up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(fov),
								    _game.GraphicsDevice.Viewport.AspectRatio,
									0.1f,
									1000.0f);

			for (UInt32 i = 0; i < world.MAX_NUM_ENTITIES; i++) {
				if (world.ComponentMasks[i].satisfies(MODEL_AND_TRANSFORM)) {
					world.Models[i].Draw(world.Transforms[i], view, projection);
				}
			}
		}
	}
}
