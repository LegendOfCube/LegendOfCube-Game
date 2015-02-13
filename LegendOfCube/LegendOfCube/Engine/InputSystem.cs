﻿using System;
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
		private KeyboardState keyState;
		private KeyboardState oldKeyState;
		private GamePadState oldGamePadState;
		private GamePadState gamePadState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game)
		{
			this.game = game;
			// TODO: settings for inverted y axis
			oldKeyState = Keyboard.GetState();
			oldGamePadState = GamePad.GetState(PlayerIndex.One); //Assuming single player game
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			Vector2 directionInput = new Vector2(0, 0);


			if (!gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (KeyWasPressed(Keys.Escape))
			{
				game.Exit();
			}

			foreach (var e in world.EnumerateEntities(MOVEMENT_INPUT)) {

				InputDataImpl inputData = (InputDataImpl)world.InputData[e.Id];

				if (keyState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed) directionInput.Y++;

				if (keyState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed) directionInput.Y--;

				if (keyState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed) directionInput.X--;

				if (keyState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed) directionInput.X++;

				// Normalize the vector to our needs, then set direction
				directionInput = !directionInput.Equals(Vector2.Zero) ? Vector2.Normalize(directionInput) : gamePadState.ThumbSticks.Left;

				if (keyState.IsKeyDown(Keys.LeftShift) || gamePadState.Triggers.Left > 0)
				{
					directionInput *= 0.5f;
				}

				inputData.SetDirection(directionInput);

				if ( keyState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed )
				{
					inputData.SetStateOfJumping(true);
					if (!oldKeyState.IsKeyDown(Keys.Space) && !(oldGamePadState.Buttons.A == ButtonState.Pressed) )
					{
						inputData.SetNewJump(true);
					}
					else
					{
						inputData.SetNewJump(false);
					}
				}
				else
				{
					inputData.SetStateOfJumping(false);
					inputData.SetNewJump(false);
				}
			}

			oldKeyState = keyState;
			oldGamePadState = gamePadState;
		}

		private bool KeyWasPressed(Keys key)
		{
			return keyState.IsKeyDown(key) && !oldKeyState.IsKeyDown(key);
		}

		private bool KeyWasReleased(Keys key)
		{
			return keyState.IsKeyUp(key) && !oldKeyState.IsKeyUp(key);
		}
	}
}
