using System.Collections.Generic;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine;
using LegendOfCube.Levels;

namespace LegendOfCube.Screens
{
	public class ScreenSystem
	{
		private readonly List<Screen> screens;
		private readonly Game game;
		private readonly ContentCollection contentCollection;
		private readonly GraphicsDeviceManager graphicsManager;

		public ScreenSystem(LegendOfCubeGame game, ContentCollection contentCollection, GraphicsDeviceManager graphicsManager)
		{
			this.game = game;
			this.contentCollection = contentCollection;
			this.graphicsManager = graphicsManager;
			screens = new List<Screen>(3);
		}

		public void AddScreen(Screen screen)
		{
			screen.LoadContent();
			screens.Add(screen);
		}

		public void AddGameScreen(Level level)
		{
			Screen s = new GameScreen(level, game, this, contentCollection, graphicsManager);
			AddScreen(s);
		}

		public void RemoveCurrentScreen()
		{
			screens.RemoveAt(screens.Count - 1);
		}

		public void SetScreen(Screen screen)
		{
			screens.Clear();
			AddScreen(screen);
		}

		public Screen GetCurrentScreen()
		{
			return screens[screens.Count - 1];
		}

		public void Update(GameTime gameTime)
		{
			int firstUpdate = screens.Count - 1;
			for (int i = firstUpdate; i >= 0; i--)
			{
				Screen s = screens[i];
				if (!s.UpdateBehind)
				{
					firstUpdate = i;
					break;
				}
			}

			for (int i = firstUpdate; i < screens.Count; i++)
			{
				Screen s = screens[i];
				s.Update(gameTime, i != screens.Count - 1);
			}
		}

		public void Draw(GameTime gameTime)
		{
			int firstRender = screens.Count - 1;
			for (int i = firstRender; i >= 0; i--)
			{
				Screen s = screens[i];
				if (!s.RenderBehind)
				{
					firstRender = i;
					break;
				}
			}

			for (int i = firstRender; i < screens.Count; i++)
			{
				Screen s = screens[i];
				s.Draw(gameTime, i != screens.Count - 1);
			}
		}
		public void LoadContent()
		{
			AddScreen(new GameScreen(LevelConstants.BACKGROUND_LEVEL, game, this, contentCollection, graphicsManager));
			AddScreen(new MainMenuScreen(game, this));
		}

		public void ResetGameScreen()
		{
			var sc = (GameScreen)screens[screens.Count - 1];
			RemoveCurrentScreen();
			AddGameScreen(sc.Level);
		}

	}
}
