using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	public enum ScreenTypes{ START, GAME, PAUSE, LEVEL_SELECT, NULL };

	public class ScreenSystem
	{
		private Screen[] screens;
		private ScreenTypes currentScreen = ScreenTypes.NULL;
		private readonly LegendOfCubeGame game;
		private int[] screenStack;
		private readonly ContentCollection collection;

		public ScreenSystem(LegendOfCubeGame game, ContentCollection collection)
		{
			this.game = game;
			this.collection = collection;
			screens = new Screen[5];

			AddScreen(new StartScreen(game, this), ScreenTypes.START);
			AddScreen(new GameScreen(game, collection), ScreenTypes.GAME);
			//screenSystem.AddScreen(new LevelSelectScreen(this), ScreenTypes.LEVEL_SELECT);
			AddScreen(new PauseScreen(game, this), ScreenTypes.PAUSE);
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

		public World GetCurrentWorld()
		{
				return screens[(int)ScreenTypes.GAME].World;
		}

		internal void MoveToPreviousScreen()
		{

		}
	}
}
