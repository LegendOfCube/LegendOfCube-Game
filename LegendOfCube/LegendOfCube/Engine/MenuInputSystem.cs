using System;
using System.Collections.Generic;
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
		                                                                  Properties.INPUT);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private ScreenSystem screenSystem;

		private KeyboardState keyState;
		private KeyboardState oldKeyState;

		private GamePadState gamePadState;
		private GamePadState oldGamePadState;

		private MouseState mouseState;
		private MouseState oldMouseState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public MenuInputSystem(Game game, ScreenSystem screenSystem)
		{
			this.game = game;
			this.screenSystem = screenSystem;
			
			// TODO: settings for inverted y axis
			oldKeyState = Keyboard.GetState();
			oldGamePadState = GamePad.GetState(PlayerIndex.One); //Assuming single player game
			oldMouseState = Mouse.GetState();
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		public void ApplyInput(GameTime gameTime, List<MenuItem> menuItems, ref int selection)
		{
			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			mouseState = Mouse.GetState();

			game.IsMouseVisible = true;

			if (!gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (KeyWasJustPressed(Keys.Back) || ButtonWasJustPressed(Buttons.Back))
			{
				screenSystem.RemoveCurrentScreen();
			}


			if (KeyWasJustPressed(Keys.W) || ButtonWasJustPressed(Buttons.DPadUp))
			{
				selection = ((selection + menuItems.Count - 1) % menuItems.Count);
			}

			if (KeyWasJustPressed(Keys.S) || ButtonWasJustPressed(Buttons.DPadDown))
			{
				selection = ((selection + 1) % menuItems.Count);
			}

			Vector2 directionInput =  gamePadState.ThumbSticks.Left;
			if (directionInput.Length() > 0.05)
			{
				var xPos = mouseState.X + 10 * directionInput.X;
				var yPos = mouseState.Y - 10 * directionInput.Y;
				Mouse.SetPosition((int)xPos, (int)yPos);
			}
			if (MouseWasMoved())
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					MenuItem menuItem = menuItems[i];
					if (MouseWithinRectangle(menuItem.Rectangle))
					{
						selection = i;
						break;
					}
				}
			}

			for (int i = 0; i < menuItems.Count; i++)
			{
				MenuItem menuItem = menuItems[i];
				menuItem.Selected = i == selection;
			}

			MenuItem selectedItem = menuItems[selection];

			if (KeyWasJustPressed(Keys.Space) || ButtonWasJustPressed(Buttons.A) || MouseClickWithinRectangle(selectedItem.Rectangle))
			{
				selectedItem.OnClick();
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

		private bool MouseLeftWasJustPressed()
		{
			return (mouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton != ButtonState.Pressed);
		}

		private bool MouseWasMoved()
		{
			return (mouseState.X != oldMouseState.X) || (mouseState.Y != oldMouseState.Y);
		}

		private bool MouseClickWithinRectangle(Rectangle rect)
		{
			return MouseLeftWasJustPressed() && rect.Contains(mouseState.X, mouseState.Y);
		}

		private bool MouseWithinRectangle(Rectangle rect)
		{
			return rect.Contains(mouseState.X, mouseState.Y);
		}
	}
}
