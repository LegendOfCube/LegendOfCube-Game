using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Engine.Input
{
	public class InputHelper
	{
		public KeyboardState keyState { get; private set; }
		public KeyboardState oldKeyState { get; private set; }
		public GamePadState gamePadState { get; private set; }
		public GamePadState oldGamePadState { get; private set; }
		public MouseState mouseState { get; private set; }
		public MouseState oldMouseState { get; private set; }

		public InputHelper()
		{
			oldKeyState = Keyboard.GetState();
			oldGamePadState = GamePad.GetState(PlayerIndex.One);
			oldMouseState = Mouse.GetState();

			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			mouseState = Mouse.GetState();
		}

		public void UpdateInputStates()
		{
			oldKeyState = keyState;
			oldMouseState = mouseState;
			oldGamePadState = gamePadState;

			keyState = Keyboard.GetState();
			gamePadState = GamePad.GetState(PlayerIndex.One);
			mouseState = Mouse.GetState();
		}

		public bool ButtonWasJustPressed(Buttons button)
		{
			return gamePadState.IsButtonDown(button) && oldGamePadState.IsButtonUp(button);
		}

		public bool KeyWasJustPressed(Keys key)
		{
			return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
		}

		public bool MouseLeftWasJustPressed()
		{
			return (mouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton != ButtonState.Pressed);
		}

		public bool MouseWasMoved()
		{
			return (mouseState.X != oldMouseState.X) || (mouseState.Y != oldMouseState.Y);
		}

		public bool MouseClickWithinRectangle(Rectangle rect)
		{
			return MouseLeftWasJustPressed() && rect.Contains(mouseState.X, mouseState.Y);
		}

		public bool MouseWithinRectangle(Rectangle rect)
		{
			return rect.Contains(mouseState.X, mouseState.Y);
		}
	}
}
