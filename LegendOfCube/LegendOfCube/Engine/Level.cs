using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public abstract class Level
	{
		public String Name { get; private set; }
		public bool FixedCamera { get; private set; }

		protected Level(String name, bool fixedCamera = false)
		{
			Name = name;
			FixedCamera = fixedCamera;
		}

		public abstract World CreateWorld(Game game, ContentCollection contentCollection);
	}
}
