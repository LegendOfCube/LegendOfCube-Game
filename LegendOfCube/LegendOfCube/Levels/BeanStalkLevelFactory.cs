using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class BeanStalkLevelFactory : ILevelFactory
	{
		public World CreateWorld(Game game, GameObjectTemplates gameObjectTemplates)
		{
			World world = new World(1000);
			var player = new EntityBuilder().WithTemplate(gameObjectTemplates.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 15)
				.WithAcceleration(Vector3.Zero, 30)
				.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
				.AddToWorld(world);
			world.Player = player;

			var platformBuilder = new EntityBuilder()
				.WithTemplate(gameObjectTemplates.RustPlatform);

			platformBuilder.WithTransform(Matrix.CreateScale(5000));
			platformBuilder.AddToWorld(world);

			world.LightDirection = Vector3.Normalize(new Vector3(1, -1, 1));
			world.SpawnPoint = new Vector3(0, 0, 0);
			world.AmbientIntensity = 0.3f;
			var rnd = new Random(0);

			for (float y = 1; y < 1000; y += 7)
			{
				platformBuilder.WithTransform(Matrix.Identity);
				platformBuilder.WithPosition(new Vector3(rnd.Next(-15, 15), y, rnd.Next(-15, 15))).AddToWorld(world);
			}

			return world;
		}
	}
}
