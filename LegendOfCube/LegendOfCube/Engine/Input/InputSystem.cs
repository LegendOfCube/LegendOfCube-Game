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

		public InputSystem(Game game, ScreenSystem screenSystem, InputHelper inputHelper)
		{
			this.game = game;
			this.screenSystem = screenSystem;

			iH = inputHelper;
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			Vector2 directionInput = new Vector2(0, 0);

			game.IsMouseVisible = false;

			if (!iH.gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (iH.oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (iH.KeyWasJustPressed(Keys.Escape) || iH.KeyWasJustPressed(Keys.Tab) || iH.ButtonWasJustPressed(Buttons.Start))
			{
				screenSystem.AddScreen(new PauseScreen(game, screenSystem, iH));
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

				if (iH.keyState.IsKeyDown(Keys.W) || iH.gamePadState.DPad.Up == ButtonState.Pressed) directionInput.Y++;

				if (iH.keyState.IsKeyDown(Keys.S) || iH.gamePadState.DPad.Down == ButtonState.Pressed) directionInput.Y--;

				if (iH.keyState.IsKeyDown(Keys.A) || iH.gamePadState.DPad.Left == ButtonState.Pressed) directionInput.X--;

				if (iH.keyState.IsKeyDown(Keys.D) || iH.gamePadState.DPad.Right == ButtonState.Pressed) directionInput.X++;

				// Normalize the vector to our needs, then set direction
				directionInput = !directionInput.Equals(Vector2.Zero) ? Vector2.Normalize(directionInput) : iH.gamePadState.ThumbSticks.Left;

				if (iH.keyState.IsKeyDown(Keys.LeftShift) || iH.gamePadState.Triggers.Left > 0)
				{
					directionInput *= 0.5f;
				}

				inputData.SetDirection(directionInput);

				if (iH.keyState.IsKeyDown(Keys.Space) || iH.gamePadState.Buttons.A == ButtonState.Pressed)
				{
					inputData.SetStateOfJumping(true);
					if (!iH.oldKeyState.IsKeyDown(Keys.Space) && !(iH.oldGamePadState.Buttons.A == ButtonState.Pressed))
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
				if (iH.keyState.IsKeyDown(Keys.Up)) cameraDirection.Y = 1.0f;
				if (iH.keyState.IsKeyDown(Keys.Down)) cameraDirection.Y = -1.0f;
				if (iH.keyState.IsKeyDown(Keys.Left)) cameraDirection.X = -1.0f;
				if (iH.keyState.IsKeyDown(Keys.Right)) cameraDirection.X = 1.0f;

				// Normalize the vector to our needs, then set direction
				cameraDirection = !cameraDirection.Equals(Vector2.Zero) ? Vector2.Normalize(cameraDirection) : iH.gamePadState.ThumbSticks.Right;

				if (cfg.RightStickInvertedY) cameraDirection.Y = -cameraDirection.Y;
				if (cfg.RightStickInvertedX) cameraDirection.X = -cameraDirection.X;

				inputData.SetCameraDirection(cameraDirection);

			}
		}
	}
}
