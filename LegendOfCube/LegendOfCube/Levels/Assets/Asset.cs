using LegendOfCube.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Levels.Assets
{
	abstract class Asset
	{
		protected static World world;
		protected static Game game;

		public Asset(World w, Game g)
		{
			world = w;
			game = g;
		}
	}
}
