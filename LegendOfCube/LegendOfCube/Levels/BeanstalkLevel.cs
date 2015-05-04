using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	class BeanstalkLevel : Level
	{
		private const float HEIGHT = 500;
		private const int RANDOM_SEED = 0;

		public BeanstalkLevel() : base("Beanstalk Level") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000)
			{
				SpawnPoint = new Vector3(0, 0, 0),
				InitialViewDirection = Vector3.Normalize(new Vector3(1.0f, 0.0f, 0.0f)),
				DirLight = new DirLight(Vector3.Normalize(new Vector3(1, -1, 1))),
			};

			var player = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube2)
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
			var rnd = new Random(RANDOM_SEED);
			for (float y = 5.0f; y < HEIGHT; y += 6.25f)
			{
				platformBuilder
					.WithPosition(new Vector3(rnd.Next(-15, 15), y, rnd.Next(-15, 15)))
					.AddToWorld(world);
			}

			// Add larger platform at the top
			platformBuilder
				.WithTransform(Matrix.CreateScale(4.0f))
				.WithPosition(new Vector3(-0.0f, 500.0f, -40.0f))
				.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
				.AddToWorld(world);

			return world;
		}
	}
}
