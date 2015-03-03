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
	class Wall : Asset
	{

		protected override void loadAssets()
		{
			model = game.Content.Load<Model>("Models/Brick_Wall/brick_wall");
			obb = new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY,
				Vector3.UnitZ, new Vector3(.5f,2.5f,5));
		}

		public void Add(Vector3 pos)
		{
			new EntityBuilder().WithModel(model)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(pos)
					.WithBoundingVolume(obb)
					.AddToWorld(world);
		}

		public Wall(World world, Game game) :
			base(world, game)
		{
			loadAssets();
		}
	}
}