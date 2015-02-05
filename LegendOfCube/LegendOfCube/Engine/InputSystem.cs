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
        private GamePadState oldGamePadState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game)
		{
            // TODO: settings for inverted y axis
			this.game = game;
			oldKeyState = Keyboard.GetState();
            oldGamePadState = GamePad.GetState(PlayerIndex.One); //Assuming single player game
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            Vector2 directionInput = new Vector2(0, 0);
            

            if (!gamePadState.IsConnected)
            {
                //Only writes message once when controller was disconnected
                if(oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
            }

			for (UInt32 i = 0; i < world.MaxNumEntities; i++) {
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;

                InputDataImpl inputData = (InputDataImpl) world.InputData[i];


				if (keyState.IsKeyDown(Keys.W))
				{
                    directionInput.Y++;
                    world.Transforms[i] = Matrix.CreateTranslation(2.5f * world.Transforms[i].Forward) * world.Transforms[i];
                }

				if (keyState.IsKeyDown(Keys.S))
				{
                    directionInput.Y--;
				    world.Transforms[i] = Matrix.CreateTranslation(2.5f * world.Transforms[i].Backward) * world.Transforms[i];
				}
				
				if (keyState.IsKeyDown(Keys.A))
				{
                    directionInput.X--;
					world.Transforms[i] = Matrix.CreateTranslation(2.5f * world.Transforms[i].Left) * world.Transforms[i];
				}
				
				if (keyState.IsKeyDown(Keys.D))
				{
                    directionInput.X++;
				    world.Transforms[i] = Matrix.CreateTranslation(2.5f * world.Transforms[i].Right) * world.Transforms[i];
				}

                // Normalize the vector to our needs, then set direction
                if (!directionInput.Equals(new Vector2(0, 0)))
                {
                    directionInput = Vector2.Normalize(directionInput);
                    // TODO: Apply speed modifier
                }
                else
                {
                    directionInput = gamePadState.ThumbSticks.Left;                    
                }

                inputData.SetDirection(directionInput);

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
