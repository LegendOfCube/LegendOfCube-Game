using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Levels.Assets
{
	abstract class Asset
	{
		protected World world;
		protected Game game;
		protected Model model;
		protected OBB obb;

		public Asset(World w, Game g)
		{
			world = w;
			game = g;
		}

		protected abstract void loadAssets();
	}
}
