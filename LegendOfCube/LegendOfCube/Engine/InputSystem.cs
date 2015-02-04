using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Engine
{
	public class InputSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
		                                                                        Properties.INPUT_FLAG);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private KeyboardState oldKeyState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game)
		{
			this.game = game;
			oldKeyState = Keyboard.GetState();
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			KeyboardState keyState = Keyboard.GetState();

			for (UInt32 i = 0; i < world.MaxNumEntities; i++) {
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;

				if (keyState.IsKeyDown(Keys.W))
				{
					world.Transforms[i] = Matrix.CreateTranslation(7f * world.Transforms[i].Forward) * world.Transforms[i];
				}

				if (keyState.IsKeyDown(Keys.S))
				{
					world.Transforms[i] = Matrix.CreateTranslation(7f * world.Transforms[i].Backward) * world.Transforms[i];
				}
				
				if (keyState.IsKeyDown(Keys.A))
				{
					world.Transforms[i] = Matrix.CreateTranslation(7f * world.Transforms[i].Left) * world.Transforms[i];
				}
				
				if (keyState.IsKeyDown(Keys.D))
				{
					world.Transforms[i] = Matrix.CreateTranslation(7f * world.Transforms[i].Right) * world.Transforms[i];
				}

				if (keyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space) && world.Transforms[i].Translation.Y <= 0)
				{
					world.Velocities[i].Y = 8f;
					/*if (cube.ModelToWorld.Translation.Y == 0)
					{
						cube.Vel.Y += 0.21f;
					}
					else if (cube.ModelToWorld.Translation.Y > 0 && doubleJump)
					{
						cube.Vel.Y += 0.21f;
						doubleJump = false;
					}*/
				}
			}

			oldKeyState = keyState;
		}
	}
}
