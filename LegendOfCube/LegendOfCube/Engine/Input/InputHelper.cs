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

		private bool MenuAlreadyReceivedStickInput = false;

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

		public bool MenuUpPressed()
		{
			if (KeyWasJustPressed(Keys.W)) return true;
			if (KeyWasJustPressed(Keys.Up)) return true;
			if (ButtonWasJustPressed(Buttons.DPadUp)) return true;

			Vector2 dirInp = GamePadState.ThumbSticks.Left;
			if (dirInp.Length() > 0.05)
			{
				if (!MenuAlreadyReceivedStickInput)
				{
					float angle = (float)Math.Atan2(dirInp.Y, dirInp.X);
					if (angle < 0) angle += 2.0f * (float)Math.PI;
					float MIN_ANGLE = MathHelper.ToRadians(45);
					float MAX_ANGLE = MathHelper.ToRadians(90 + 45);
					if (MIN_ANGLE < angle && angle < MAX_ANGLE)
					{
						MenuAlreadyReceivedStickInput = true;
						return true;
					}
				}
			}
			else
			{
				MenuAlreadyReceivedStickInput = false;
			}

			return false;
		}

		public bool MenuDownPressed()
		{
			if (KeyWasJustPressed(Keys.S)) return true;
			if (KeyWasJustPressed(Keys.Down)) return true;
			if (ButtonWasJustPressed(Buttons.DPadDown)) return true;

			Vector2 dirInp = GamePadState.ThumbSticks.Left;
			if (dirInp.Length() > 0.05)
			{
				if (!MenuAlreadyReceivedStickInput)
				{
					float angle = (float)Math.Atan2(dirInp.Y, dirInp.X);
					if (angle < 0) angle += 2.0f * (float)Math.PI;
					float MIN_ANGLE = MathHelper.ToRadians(180 + 45);
					float MAX_ANGLE = MathHelper.ToRadians(270 + 45);
					if (MIN_ANGLE < angle && angle < MAX_ANGLE)
					{
						MenuAlreadyReceivedStickInput = true;
						return true;
					}
				}
			}
			else
			{
				MenuAlreadyReceivedStickInput = false;
			}

			return false;
		}

		public bool MenuLeftPressed()
		{
			if (KeyWasJustPressed(Keys.A)) return true;
			if (KeyWasJustPressed(Keys.Left)) return true;
			if (ButtonWasJustPressed(Buttons.DPadLeft)) return true;

			Vector2 dirInp = GamePadState.ThumbSticks.Left;
			if (dirInp.Length() > 0.05)
			{
				if (!MenuAlreadyReceivedStickInput)
				{
					float angle = (float)Math.Atan2(dirInp.Y, dirInp.X);
					if (angle < 0) angle += 2.0f * (float)Math.PI;
					float MIN_ANGLE = MathHelper.ToRadians(90 + 45);
					float MAX_ANGLE = MathHelper.ToRadians(180 + 45);
					if (MIN_ANGLE < angle && angle < MAX_ANGLE)
					{
						MenuAlreadyReceivedStickInput = true;
						return true;
					}
				}
			}
			else
			{
				MenuAlreadyReceivedStickInput = false;
			}

			return false;
		}

		public bool MenuRightPressed()
		{
			if (KeyWasJustPressed(Keys.D)) return true;
			if (KeyWasJustPressed(Keys.Right)) return true;
			if (ButtonWasJustPressed(Buttons.DPadRight)) return true;

			Vector2 dirInp = GamePadState.ThumbSticks.Left;
			if (dirInp.Length() > 0.05)
			{
				if (!MenuAlreadyReceivedStickInput)
				{
					float angle = (float)Math.Atan2(dirInp.Y, dirInp.X);
					float MIN_ANGLE = MathHelper.ToRadians(-45);
					float MAX_ANGLE = MathHelper.ToRadians(45);
					if (MIN_ANGLE < angle && angle < MAX_ANGLE)
					{
						MenuAlreadyReceivedStickInput = true;
						return true;
					}
				}
			}
			else
			{
				MenuAlreadyReceivedStickInput = false;
			}

			return false;
		}

		public bool MenuActivatePressed(Rectangle hitBox)
		{
			if (KeyWasJustPressed(Keys.Enter)) return true;
			if (KeyWasJustPressed(Keys.Space)) return true;
			if (ButtonWasJustPressed(Buttons.A)) return true;
			if (MouseClickWithinRectangle(hitBox)) return true;
			return false;
		}

		public bool MenuCancelPressed()
		{
			if (KeyWasJustPressed(Keys.Escape)) return true;
			if (KeyWasJustPressed(Keys.Back)) return true;
			if (ButtonWasJustPressed(Buttons.B)) return true;
			return false;
		}
	}
}
