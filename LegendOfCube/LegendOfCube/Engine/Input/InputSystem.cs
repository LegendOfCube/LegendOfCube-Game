using System;
using System.Collections.Generic;
using System.Security.Policy;
using LegendOfCube.Engine.Input;
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
		private InputHelper iH;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game, ScreenSystem screenSystem)
		{
			this.game = game;
			this.screenSystem = screenSystem;

			iH = InputHelper.Instance;
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			Vector2 directionInput = new Vector2(0, 0);

			game.IsMouseVisible = false;

			if (!iH.GamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (iH.OldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (iH.KeyWasJustPressed(Keys.Escape) || iH.KeyWasJustPressed(Keys.Tab) || iH.ButtonWasJustPressed(Buttons.Start))
			{
				screenSystem.AddScreen(new PauseScreen(game, screenSystem));
			}

			if (iH.KeyWasJustPressed(Keys.F1))
			{
				world.DebugState.ShowOBBWireFrame = !world.DebugState.ShowOBBWireFrame;
			}
			if (iH.KeyWasJustPressed(Keys.F2))
			{
				world.DebugState.ShowDebugOverlay = !world.DebugState.ShowDebugOverlay;
			}

			if (iH.KeyWasJustPressed(Keys.R) || iH.ButtonWasJustPressed(Buttons.Back))
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

			if (iH.KeyWasJustPressed(Keys.Back))
			{
				screenSystem.ResetGameScreen();
			}

			foreach (var e in world.EnumerateEntities(MOVEMENT_INPUT)) {

				InputData inputData = world.InputData[e.Id];

				if (iH.KeyState.IsKeyDown(Keys.W) || iH.GamePadState.DPad.Up == ButtonState.Pressed) directionInput.Y++;

				if (iH.KeyState.IsKeyDown(Keys.S) || iH.GamePadState.DPad.Down == ButtonState.Pressed) directionInput.Y--;

				if (iH.KeyState.IsKeyDown(Keys.A) || iH.GamePadState.DPad.Left == ButtonState.Pressed) directionInput.X--;

				if (iH.KeyState.IsKeyDown(Keys.D) || iH.GamePadState.DPad.Right == ButtonState.Pressed) directionInput.X++;

				// Normalize the vector to our needs, then set direction
				directionInput = !directionInput.Equals(Vector2.Zero) ? Vector2.Normalize(directionInput) : iH.GamePadState.ThumbSticks.Left;

				if (iH.KeyState.IsKeyDown(Keys.LeftShift) || iH.GamePadState.Triggers.Left > 0)
				{
					directionInput *= 0.5f;
				}

				inputData.SetDirection(directionInput);

				if (iH.KeyState.IsKeyDown(Keys.Space) || iH.GamePadState.Buttons.A == ButtonState.Pressed)
				{
					inputData.SetStateOfJumping(true);
					if (!iH.OldKeyState.IsKeyDown(Keys.Space) && !(iH.OldGamePadState.Buttons.A == ButtonState.Pressed))
					{
						inputData.SetNewJump(true);
						inputData.BufferedJump = true;
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

				if (inputData.BufferedJump)
				{
					inputData.BufferTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
					if (inputData.BufferTimeElapsed >= inputData.BufferTime)
					{
						inputData.BufferedJump = false;
						inputData.BufferTimeElapsed = 0;
					}
					else if (world.PlayerCubeState.OnGround && inputData.BufferedJump)
					{
						inputData.SetNewJump(true);
						inputData.BufferedJump = false;
						inputData.BufferTimeElapsed = 0;
					}
				}

				Vector2 cameraDirection = new Vector2(0,0);
				// Camera movement
				if (iH.KeyState.IsKeyDown(Keys.Up)) cameraDirection.Y = 1.0f;
				if (iH.KeyState.IsKeyDown(Keys.Down)) cameraDirection.Y = -1.0f;
				if (iH.KeyState.IsKeyDown(Keys.Left)) cameraDirection.X = -1.0f;
				if (iH.KeyState.IsKeyDown(Keys.Right)) cameraDirection.X = 1.0f;

				// Normalize the vector to our needs, then set direction
				cameraDirection = !cameraDirection.Equals(Vector2.Zero) ? Vector2.Normalize(cameraDirection) : iH.GamePadState.ThumbSticks.Right;

				if (cfg.RightStickInvertedY) cameraDirection.Y = -cameraDirection.Y;
				if (cfg.RightStickInvertedX) cameraDirection.X = -cameraDirection.X;

				inputData.SetCameraDirection(cameraDirection);

			}
		}
	}
}
