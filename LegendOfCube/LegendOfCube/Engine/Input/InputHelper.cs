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
		private static readonly InputHelper instance = new InputHelper();
		public static InputHelper Instance
		{
			get
			{
				return instance;
			}
		}

		public KeyboardState KeyState { get; private set; }
		public KeyboardState OldKeyState { get; private set; }
		public GamePadState GamePadState { get; private set; }
		public GamePadState OldGamePadState { get; private set; }
		public MouseState MouseState { get; private set; }
		public MouseState OldMouseState { get; private set; }

		private InputHelper()
		{
			OldKeyState = Keyboard.GetState();
			OldGamePadState = GamePad.GetState(PlayerIndex.One);
			OldMouseState = Mouse.GetState();

			KeyState = Keyboard.GetState();
			GamePadState = GamePad.GetState(PlayerIndex.One);
			MouseState = Mouse.GetState();
		}

		public void UpdateInputStates()
		{
			OldKeyState = KeyState;
			OldMouseState = MouseState;
			OldGamePadState = GamePadState;

			KeyState = Keyboard.GetState();
			GamePadState = GamePad.GetState(PlayerIndex.One);
			MouseState = Mouse.GetState();
		}

		public bool ButtonWasJustPressed(Buttons button)
		{
			return GamePadState.IsButtonDown(button) && OldGamePadState.IsButtonUp(button);
		}

		public bool KeyWasJustPressed(Keys key)
		{
			return KeyState.IsKeyDown(key) && OldKeyState.IsKeyUp(key);
		}

		public bool MouseLeftWasJustPressed()
		{
			return (MouseState.LeftButton == ButtonState.Pressed) && (OldMouseState.LeftButton != ButtonState.Pressed);
		}

		public bool MouseWasMoved()
		{
			return (MouseState.X != OldMouseState.X) || (MouseState.Y != OldMouseState.Y);
		}

		public bool MouseClickWithinRectangle(Rectangle rect)
		{
			return MouseLeftWasJustPressed() && rect.Contains(MouseState.X, MouseState.Y);
		}

		public bool MouseWithinRectangle(Rectangle rect)
		{
			return rect.Contains(MouseState.X, MouseState.Y);
		}
	}
}
