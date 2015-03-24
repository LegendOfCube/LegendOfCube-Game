using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LegendOfCube.Screens;

namespace LegendOfCube.Engine
{
	class MenuInputSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
		                                                                  Properties.INPUT_FLAG);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private KeyboardState keyState;
		private KeyboardState oldKeyState;

		private GamePadState gamePadState;
		private GamePadState oldGamePadState;

		private MouseState mouseState;
		private MouseState oldMouseState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public MenuInputSystem(Game game)
		{
			this.game = game;
			// TODO: settings for inverted y axis
			oldKeyState = Keyboard.GetState();
			oldGamePadState = GamePad.GetState(PlayerIndex.One); //Assuming single player game
			oldMouseState = Mouse.GetState();
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		public void ApplyInput(GameTime gameTime, World world, ScreenSystem switcher, MenuScreen currentScreen, ref int selection)
		{
			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			mouseState = Mouse.GetState();
			Vector2 directionInput = Vector2.Zero;

			Rectangle playRect = new Rectangle(100, 20, 398, 59);
			Rectangle levelSelectRect = new Rectangle(100, 110, 462, 59);
			Rectangle exitRect = new Rectangle(100, 200, 151, 59);

			if (!gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (KeyWasJustPressed(Keys.Escape) || ButtonWasJustPressed(Buttons.Back))
			{
				switcher.MoveToPreviousScreen();
				//game.Exit();
			}	

			if (MouseClickWithinRectangle(playRect))
			{	
				switcher.SwitchScreen(Screens.ScreenTypes.GAME);
			}

			if (MouseClickWithinRectangle(exitRect))
			{
				game.Exit();
			}

			if (KeyWasJustPressed(Keys.W) || ButtonWasJustPressed(Buttons.DPadUp))
			{
				switch (selection)
				{
					case 2:
						selection = 1;
						break;
					case 1:
						selection = 0;
						break;
				}
			}

			if (KeyWasJustPressed(Keys.S) || ButtonWasJustPressed(Buttons.DPadDown))
			{
				switch (selection)
				{
					case 0:
						selection = 1;
						break;
					case 1:
						selection = 2;
						break;
				}
			}

			if (KeyWasJustPressed(Keys.Space) || ButtonWasJustPressed(Buttons.A))
			{
				currentScreen.PerformSelection();
			}

			// Normalize the vector to our needs, then set direction
			directionInput = !directionInput.Equals(Vector2.Zero) ? Vector2.Normalize(directionInput) : gamePadState.ThumbSticks.Left;

			var xPos = mouseState.X + 10*directionInput.X;
			var yPos = mouseState.Y - 10*directionInput.Y;
			Mouse.SetPosition((int)xPos, (int)yPos);
			if (oldMouseState.X != mouseState.X || oldMouseState.Y != mouseState.Y)
			{
				if (playRect.Contains(mouseState.X, mouseState.Y))
				{
					selection = 0;
				} 
				else if (levelSelectRect.Contains(mouseState.X, mouseState.Y))
				{
					selection = 1;
				} 
				else if (exitRect.Contains(mouseState.X, mouseState.Y))
				{
					selection = 2;
				}
			}

			oldMouseState = mouseState;
			oldKeyState = keyState;
			oldGamePadState = gamePadState;
		}

		private bool ButtonWasJustPressed(Buttons button)
		{
			return gamePadState.IsButtonDown(button) && oldGamePadState.IsButtonUp(button);
		}

		private bool KeyWasJustPressed(Keys key)
		{
			return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
		}

		private bool MouseWasJustPressed()
		{
			return (mouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton != ButtonState.Pressed);
		}

		private bool MouseClickWithinRectangle(Rectangle rect)
		{
			return (MouseWasJustPressed() || ButtonWasJustPressed(Buttons.A)) && rect.Contains(mouseState.X, mouseState.Y);
		}
	}
}
