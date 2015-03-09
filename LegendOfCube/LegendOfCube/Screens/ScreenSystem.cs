using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	public enum ScreenTypes{ START, GAME, PAUSE, NULL };

	public class ScreenSystem
	{


		public static SpriteFont font;
		public static SpriteBatch spriteBatch;

		private Screen[] screens;
		private ScreenTypes currentScreen = ScreenTypes.NULL;
		private readonly LegendOfCubeGame game;

		public ScreenSystem(LegendOfCubeGame game)
		{
			this.game = game;
			screens = new Screen[4];
		}

		public void AddScreen(Screen screen, ScreenTypes type)
		{
			if (currentScreen == ScreenTypes.NULL)
			{
				currentScreen = type;
			}
			screens[(int)type] = screen;
		}

		public void SwitchScreen(ScreenTypes indicator)
		{
			if (screens[(int)indicator] != null)
			{
				currentScreen = indicator;
			} // Else: Just deny change
		}

		public Screen GetCurrentScreen()
		{
			return screens[(int)currentScreen];
		}

		public void LoadAllContent()
		{
			foreach (var screen in screens)
			{
				if (screen != null)
				screen.LoadContent();
			}
		}

		public World GetWorld()
		{
				return screens[(int)ScreenTypes.GAME].World;
		}
	}
}
