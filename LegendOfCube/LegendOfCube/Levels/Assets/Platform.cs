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
	class Platform : Asset
	{

		public void AddToWorld(Vector3 position)
		{
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");
			var obb = new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, 
				Vector3.UnitZ, new Vector3(10, .5f, 10));

			var scale = Matrix.CreateScale(1);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(scale)
				.WithPosition(position)
				.WithBoundingVolume(obb)
				.AddToWorld(world);
		}

		public Platform(World world, Game game) :
			base(world, game)
		{

		}
	}
}