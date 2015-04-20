using System;
using System.Collections.Generic;
using LegendOfCube.Engine.Input;
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
		private InputHelper iH;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public MenuInputSystem(Game game, ScreenSystem screenSystem, InputHelper inputHelper)
		{
			this.game = game;
			this.screenSystem = screenSystem;

			iH = inputHelper;
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		public void ApplyInput(GameTime gameTime, List<MenuItem> menuItems, ref int selection)
		{
			game.IsMouseVisible = true;

			if (!iH.gamePadState.IsConnected)
			{
				//Only writes message once when controller was disconnected
				if (iH.oldGamePadState.IsConnected) Console.WriteLine("Controller disconnected");
			}

			if (iH.KeyWasJustPressed(Keys.Back) || iH.ButtonWasJustPressed(Buttons.Back))
			{
				screenSystem.RemoveCurrentScreen();
			}


			if (iH.KeyWasJustPressed(Keys.W) || iH.ButtonWasJustPressed(Buttons.DPadUp))
			{
				selection = ((selection + menuItems.Count - 1) % menuItems.Count);
			}

			if (iH.KeyWasJustPressed(Keys.S) || iH.ButtonWasJustPressed(Buttons.DPadDown))
			{
				selection = ((selection + 1) % menuItems.Count);
			}

			Vector2 directionInput = iH.gamePadState.ThumbSticks.Left;
			if (directionInput.Length() > 0.05)
			{
				var xPos = iH.mouseState.X + 10 * directionInput.X;
				var yPos = iH.mouseState.Y - 10 * directionInput.Y;
				Mouse.SetPosition((int)xPos, (int)yPos);
			}
			if (iH.MouseWasMoved())
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					MenuItem menuItem = menuItems[i];
					if (iH.MouseWithinRectangle(menuItem.Rectangle))
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

			if (iH.KeyWasJustPressed(Keys.Space) || iH.ButtonWasJustPressed(Buttons.A) || iH.MouseClickWithinRectangle(selectedItem.Rectangle))
			{
				selectedItem.OnClick();
			}
		}
	}
}
