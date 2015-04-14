using System;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	public abstract class Screen
	{
		protected Game Game { get; private set; }
		protected ScreenSystem ScreenSystem { get; private set; }
		public bool BackgroundRender { get; private set; }

		internal Screen(Game game, ScreenSystem screenSystem, bool backgroundRender)
		{
			Game = game;
			ScreenSystem = screenSystem;
			BackgroundRender = backgroundRender;
		}

		internal abstract void Update(GameTime gameTime);
		internal abstract void Draw(GameTime gameTime);

		internal abstract void LoadContent();
	}

}
