using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public abstract class Screen
	{
		protected Game Game;

		protected Screen(Game game)
		{
			this.Game = game;
		}

		protected internal abstract void Update(GameTime gameTime, World world, SwitcherSystem switcher);
		protected internal abstract void Draw(GameTime gameTime, RenderSystem renderSystem, World world);
	}
}
