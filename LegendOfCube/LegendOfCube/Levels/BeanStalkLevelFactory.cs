using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	class BeanStalkLevelFactory : ILevelFactory
	{
		public World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000) { SpawnPoint = new Vector3(0, 0, 0) };
			world.CameraPosition = world.SpawnPoint + new Vector3(-1.0f, 2.0f, 0.0f);
			world.LightDirection = Vector3.Normalize(new Vector3(1, -1, 1));
			world.AmbientIntensity = 0.3f;

			var player = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 0)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG))
				.AddToWorld(world);
			world.Player = player;

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.GreenYellow.ToVector4()
			};

			// Add ground
			new EntityBuilder()
				.WithModelData(contentCollection.PlainCube)
				.WithStandardEffectParams(groundEffect)
				.WithTransform(Matrix.CreateTranslation(0.0f, -0.5f, 0.0f) * Matrix.CreateScale(5000.0f, 1.0f, 5000.0f))
				.AddToWorld(world);

			// Prepare a builder for platforms
			var platformBuilder = new EntityBuilder()
				.WithModelData(contentCollection.RustPlatform);

			// Add a lot of platforms building upwards
			var rnd = new Random(0);
			for (float y = 5.0f; y < 500.0f; y += 7)
			{
				platformBuilder
					.WithPosition(new Vector3(rnd.Next(-15, 15), y, rnd.Next(-15, 15)))
					.AddToWorld(world);
			}

			// Add larger platform at the top
			platformBuilder
				.WithTransform(Matrix.CreateScale(4.0f))
				.WithPosition(new Vector3(-0.0f, 500.0f, -40.0f))
				.AddToWorld(world);

			return world;
		}
	}
}
