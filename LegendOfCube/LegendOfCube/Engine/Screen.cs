using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	abstract class Screen
	{
		private Game game;
		private RenderSystem renderSystem;

		protected Screen(Game game)
		{
			this.game = game;
			this.renderSystem = new RenderSystem(game);
		}

		protected abstract void Update(GameTime gameTime);

		protected abstract void Draw(GameTime gameTime);
	}
}
