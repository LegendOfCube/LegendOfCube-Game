using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	public class ScreenSystem
	{
		private readonly List<Screen> screens;
		private readonly Game game;
		private readonly ContentCollection contentCollection;
		private readonly GraphicsDeviceManager graphicsManager;
		private readonly InputHelper inputHelper;

		public ScreenSystem(LegendOfCubeGame game, ContentCollection contentCollection, GraphicsDeviceManager graphicsManager, InputHelper inputHelper)
		{
			this.game = game;
			this.contentCollection = contentCollection;
			this.graphicsManager = graphicsManager;
			this.inputHelper = inputHelper;
			screens = new List<Screen>(3);
		}

		public void AddScreen(Screen screen)
		{
			screen.LoadContent();
			screens.Add(screen);
		}

		public void AddGameScreen(Level level)
		{
			Screen s = new GameScreen(level, game, this, contentCollection, graphicsManager, inputHelper);
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
			screens[screens.Count - 1].Update(gameTime);
		}

		public void Draw(GameTime gameTime)
		{
			int firstRender= screens.Count - 1;
			for (int i = firstRender - 1; i >= 0; i--)
			{
				Screen s = screens[i];
				if (!s.BackgroundRender)
				{
					firstRender = i + 1;
				}
			}

			for (int i = firstRender; i < screens.Count; i++)
			{
				Screen s = screens[i];
				s.Draw(gameTime);
			}
		}
		public void LoadContent()
		{
			AddScreen(new StartScreen(game, this, inputHelper));
		}

		public void ResetGameScreen()
		{
			var sc = (GameScreen)screens[screens.Count - 1];
			RemoveCurrentScreen();
			AddGameScreen(sc.level);
		}

	}
}
