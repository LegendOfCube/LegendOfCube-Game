using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	public class TestLevel1 : Level
	{
		private const int RANDOM_SEED = 2;
		private const int NUM_OTHER_CUBE = 1200;
		private const int NUM_DEATH_CUBE = 0;
		private const int NUM_BOUNCE_CUBE = 300;
		private const int NUM_TELEPORT_CUBE = 0;

		public TestLevel1() : base("Test Level 1") {}


		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(3000)
			{
				SpawnPoint = new Vector3(0, 2, 0),
				InitialViewDirection = Vector3.Normalize(new Vector3(-1, 0, 0)),
				LightDirection = Vector3.Normalize(new Vector3 {X = 0, Y = -2, Z = 1}),
				AmbientIntensity = 0.3f
			};

			Random rnd = new Random(RANDOM_SEED);

			// Add player
			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube2)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 0)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT |
					               Properties.GRAVITY_FLAG |
					               Properties.DYNAMIC_VELOCITY_FLAG));
			var playerEntity = playerBuilder.AddToWorld(world);
			world.Player = playerEntity;

			// Add a large number of cubes to the world, with different size and rotation
			var cyanCubeEffect = contentCollection.PlayerCubePlain.EffectParams.ShallowCopy();
			cyanCubeEffect.EmissiveColor = Color.Cyan.ToVector4();
			
			var otherCubeBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(cyanCubeEffect);

			for (int i = 0; i < NUM_OTHER_CUBE; i++)
			{
				otherCubeBuilder
					.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
					               Matrix.CreateRotationY(MathHelper.TwoPi*(float) rnd.NextDouble())*
					               Matrix.CreateRotationX(MathHelper.TwoPi*(float) rnd.NextDouble()))
					.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
					.AddToWorld(world);
			}

			// Add bounce cubes
			var bounceCubeEffect = contentCollection.PlayerCubePlain.EffectParams.ShallowCopy();
			bounceCubeEffect.EmissiveColor = Color.Yellow.ToVector4();

			var bouncyCubeBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(bounceCubeEffect)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG));

			for (int i = 0; i < NUM_BOUNCE_CUBE; i++)
			{
				bouncyCubeBuilder
					.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
					               Matrix.CreateRotationY(MathHelper.TwoPi*(float) rnd.NextDouble()))
					.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
					.AddToWorld(world);
			}

			// Add death cubes
			var deathCubeEffect = contentCollection.PlayerCubePlain.EffectParams.ShallowCopy();
			deathCubeEffect.EmissiveColor = Color.Red.ToVector4();

			var deathCubeBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(deathCubeEffect)
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG));

			for (int i = 0; i < NUM_DEATH_CUBE; i++)
			{
				deathCubeBuilder
					.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
					               Matrix.CreateRotationY(MathHelper.TwoPi*(float) rnd.NextDouble()))
					.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
					.AddToWorld(world);
			}

			// Add teleport cubes
			var teleportCubeEffect = contentCollection.PlayerCubePlain.EffectParams.ShallowCopy();
			teleportCubeEffect.EmissiveColor = Color.DodgerBlue.ToVector4();

			var teleportCubeBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(teleportCubeEffect)
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG));

			for (int i = 0; i < NUM_TELEPORT_CUBE; i++)
			{
				teleportCubeBuilder
					.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
					               Matrix.CreateRotationY(MathHelper.TwoPi*(float) rnd.NextDouble()))
					.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
					.AddToWorld(world);
			}

			// Add enormous ground cube
			var groundBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithTransform(Matrix.CreateScale(1000.0f))
				.WithPosition(new Vector3(0, -1000.0f, 0))
				.WithStandardEffectParams(new StandardEffectParams
				{
					DiffuseColor = new Vector4(new Vector3(0.55f), 1.0f),
					SpecularColor = new Vector4(new Vector3(0.5f), 1.0f)
				});
			groundBuilder.AddToWorld(world);

			// Add death floor, for respawning after falling off
			var deathFloorBuilder = new EntityBuilder()
				.WithTransform(Matrix.CreateScale(1900))
				.WithPosition(new Vector3(0, -2000.0f, 0))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 0.5f, 0), 1, 1, 1))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG));
			deathFloorBuilder.AddToWorld(world);

			return world;
		}
	}
}
