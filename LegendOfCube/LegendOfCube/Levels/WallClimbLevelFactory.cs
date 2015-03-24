using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	class WallClimbLevelFactory : ILevelFactory
	{
		public World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000);
			world.SpawnPoint = new Vector3(-30.0f, 5.0f, 0.0f);
			world.CameraPosition = world.SpawnPoint + new Vector3(-1.0f, 2.0f, 0.0f);
			world.LightDirection = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
			world.AmbientIntensity = 0.3f;

			world.Player = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 15)
				.WithAcceleration(Vector3.Zero, 30)
				.WithAdditionalProperties(new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG))
				.AddToWorld(world);

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.GreenYellow.ToVector4()
			};
			var massiveWallEffect = new StandardEffectParams
			{
				DiffuseColor = Color.BlueViolet.ToVector4()
			};
			var platformEffect = new StandardEffectParams
			{
				DiffuseColor = Color.LightBlue.ToVector4()
			};
			
			// Add ground
			new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(groundEffect)
				.WithTransform(Matrix.CreateScale(5000.0f, 1.0f, 5000.0f))
				.AddToWorld(world);

			// Add a massive wall
			new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(massiveWallEffect)
				.WithTransform(Matrix.CreateScale(10.0f, 600.0f, 200.0f))
				.WithPosition(new Vector3(5.0f, 0.0f, 0.0f))
				.AddToWorld(world);

			// Prepare builder for platforms
			var platformBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithTransform(Matrix.CreateTranslation(0.0f, -0.5f, 0.0f) * Matrix.CreateScale(10.0f, 0.5f, 10.0f))
				.WithStandardEffectParams(platformEffect);

			// Add platforms randomly located on the wall
			var rnd = new Random(0);
			for (int i = 0; i < 600; i++)
			{
				platformBuilder
					.WithPosition(new Vector3(-5.0f, rnd.Next(5, 600), rnd.Next(-100, 100)))
					.AddToWorld(world);
			}

			return world;
		}
	}
}
