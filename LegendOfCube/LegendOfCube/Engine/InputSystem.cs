using System;
using System.Collections.Generic;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LegendOfCube.Screens;

namespace LegendOfCube.Engine
{
	public class InputSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
		                                                                  Properties.INPUT);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private GlobalConfig cfg = GlobalConfig.Instance;

		private Game game;
		private ScreenSystem screenSystem;
		private KeyboardState keyState;
		private KeyboardState oldKeyState;

		private GamePadState gamePadState;
		private GamePadState oldGamePadState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game, ScreenSystem screenSystem)
		{
			this.game = game;
			this.screenSystem = screenSystem;

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

			game.IsMouseVisible = false;

			if (!gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (KeyWasJustPressed(Keys.Escape) || KeyWasJustPressed(Keys.Tab) || ButtonWasJustPressed(Buttons.Start))
			{
				screenSystem.AddScreen(new PauseScreen(game, screenSystem));
			}

			if (KeyWasJustPressed(Keys.F1))
			{
				world.DebugState.ShowOBBWireFrame = !world.DebugState.ShowOBBWireFrame;
			}
			if (KeyWasJustPressed(Keys.F2))
			{
				world.DebugState.ShowDebugOverlay = !world.DebugState.ShowDebugOverlay;
			}

			if (KeyWasJustPressed(Keys.R) || ButtonWasJustPressed(Buttons.Back))
			{
				if (world.WinState)
				{
					screenSystem.ResetGameScreen();
				}
				else
				{
					EventSystem.RespawnPlayer(world);
				}
			}

			if (KeyWasJustPressed(Keys.Back))
			{
				screenSystem.ResetGameScreen();
			}

			foreach (var e in world.EnumerateEntities(MOVEMENT_INPUT)) {

				InputData inputData = world.InputData[e.Id];

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

				if ( keyState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
				{
					inputData.BufferedJump = true;
					inputData.SetStateOfJumping(true);
					if (!oldKeyState.IsKeyDown(Keys.Space) && !(oldGamePadState.Buttons.A == ButtonState.Pressed) )
					{
						inputData.SetNewJump(true);
					}
					else if ( inputData.BufferedJump && world.PlayerCubeState.OnGround )
					{
						inputData.SetNewJump(true);
						inputData.BufferedJump = false;
					}
					else
					{
						inputData.SetNewJump(false);
					}
				}
				else
				{
					if (world.PlayerCubeState.OnGround && inputData.BufferedJump)
					{
						inputData.SetStateOfJumping(true);
						inputData.SetNewJump(true);
						inputData.BufferedJump = false;
					}
					else
					{
						inputData.SetStateOfJumping(false);
						inputData.SetNewJump(false);
					}
				}

				Vector2 cameraDirection = new Vector2(0,0);
				// Camera movement
				if (keyState.IsKeyDown(Keys.Up)) cameraDirection.Y = 1.0f;
				if (keyState.IsKeyDown(Keys.Down)) cameraDirection.Y = -1.0f;
				if (keyState.IsKeyDown(Keys.Left)) cameraDirection.X = -1.0f;
				if (keyState.IsKeyDown(Keys.Right)) cameraDirection.X = 1.0f;

				// Normalize the vector to our needs, then set direction
				cameraDirection = !cameraDirection.Equals(Vector2.Zero) ? Vector2.Normalize(cameraDirection) : gamePadState.ThumbSticks.Right;

				if (cfg.RightStickInvertedY) cameraDirection.Y = -cameraDirection.Y;
				if (cfg.RightStickInvertedX) cameraDirection.X = -cameraDirection.X;

				inputData.SetCameraDirection(cameraDirection);

			}

			oldKeyState = keyState;
			oldGamePadState = gamePadState;
		}

		private bool KeyWasJustPressed(Keys key)
		{
			return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
		}

		private bool KeyWasJustReleased(Keys key)
		{
			return keyState.IsKeyUp(key) && oldKeyState.IsKeyDown(key);
		}

		private bool ButtonWasJustPressed(Buttons button)
		{
			return gamePadState.IsButtonDown(button) && oldGamePadState.IsButtonUp(button);
		}

		private bool ButtonWasJustReleased(Buttons button)
		{
			return gamePadState.IsButtonUp(button) && oldGamePadState.IsButtonDown(button);
		}
	}
}
