using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public abstract class Level
	{
		public String Name { get; private set; }

		protected Level(String name)
		{
			Name = name;
		}

		public abstract World CreateWorld(Game game, ContentCollection contentCollection);
	}
}
