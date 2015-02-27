using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
		
		public void ApplyInput(GameTime gameTime, World world, SwitcherSystem switcher, ref int selection)
		{
			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			mouseState = Mouse.GetState();
			Vector2 directionInput = Vector2.Zero;

			if (!gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (KeyWasJustPressed(Keys.Escape) || ButtonWasJustPressed(Buttons.Back))
			{
				game.Exit();
			}	

			if (MouseClickWithinRectangle(new Rectangle(100, 20, 398, 59)))
			{	
				switcher.Switch();
			}

			if (MouseClickWithinRectangle(new Rectangle(100, 200, 151, 59)))
			{
				game.Exit();
			}

			if (KeyWasJustPressed(Keys.W))
			{
				if (selection == 1)
				{
					selection = 0;
				}
			}

			if (KeyWasJustPressed(Keys.S))
			{
				if (selection == 0)
				{
					selection = 1;
				}
			}

			if (KeyWasJustPressed(Keys.Space))
			{
				switch (selection)
				{
					case 0:
						switcher.Switch();
						break;
					case 1:
						game.Exit();
						break;
				}
			}

			// Normalize the vector to our needs, then set direction
			directionInput = !directionInput.Equals(Vector2.Zero) ? Vector2.Normalize(directionInput) : gamePadState.ThumbSticks.Left;

			var xPos = mouseState.X + directionInput.X;
			var yPos = mouseState.Y + directionInput.Y;
			Mouse.SetPosition((int)xPos, (int)yPos);

			oldMouseState = mouseState;	
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
