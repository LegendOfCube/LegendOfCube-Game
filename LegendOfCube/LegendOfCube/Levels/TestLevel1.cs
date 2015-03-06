using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	public class TestLevel1
	{
		
		private static Entity playerEntity;
		private static Entity[] otherCubes;
		private static Entity ground;
		private static Entity deathZone;
		private static Entity[] bouncyCubes;
		private static Entity[] deathCubes;
		private static Entity[] teleportCubes;

		public static void CreateLevel(World world, Game game)
		{
			var cubeModel = game.Content.Load<Model>("Models/cube/cube_plain");

			var playerEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.45f), 1.0f),
				SpecularTexture = game.Content.Load<Texture>("Models/cube/cube_specular"),
				EmissiveTexture = game.Content.Load<Texture>("Models/cube/cube_emissive_plain"),
				NormalTexture = game.Content.Load<Texture>("Models/cube/cube_normal"),
				SpecularColor = Color.White.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.45f), 1.0f),
				SpecularTexture = game.Content.Load<Texture>("Models/cube/cube_specular"),
				EmissiveTexture = game.Content.Load<Texture>("Models/cube/cube_emissive_plain"),
				NormalTexture = game.Content.Load<Texture>("Models/cube/cube_normal"),
				SpecularColor = Color.White.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var bouncyCubeEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.45f), 1.0f),
				SpecularTexture = game.Content.Load<Texture>("Models/cube/cube_specular"),
				EmissiveTexture = game.Content.Load<Texture>("Models/cube/cube_emissive_plain"),
				NormalTexture = game.Content.Load<Texture>("Models/cube/cube_normal"),
				SpecularColor = Color.Yellow.ToVector4(),
				EmissiveColor = Color.Yellow.ToVector4()
			};

			var deathCubeEffect = new StandardEffectParams
			{
				SpecularTexture = game.Content.Load<Texture>("Models/cube/cube_specular"),
				EmissiveTexture = game.Content.Load<Texture>("Models/cube/cube_emissive_plain"),
				NormalTexture = game.Content.Load<Texture>("Models/cube/cube_normal"),
				SpecularColor = Color.Red.ToVector4(),
				EmissiveColor = Color.White.ToVector4(),
				DiffuseColor = Color.Red.ToVector4(),
			};

			var teleportCubeEffect = new StandardEffectParams
			{
				SpecularTexture = game.Content.Load<Texture>("Models/cube/cube_specular"),
				EmissiveTexture = game.Content.Load<Texture>("Models/cube/cube_emissive_plain"),
				NormalTexture = game.Content.Load<Texture>("Models/cube/cube_normal"),
				SpecularColor = Color.Blue.ToVector4(),
				EmissiveColor = Color.Blue.ToVector4(),
				DiffuseColor = Color.Blue.ToVector4(),
			};

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.55f), 1.0f),
				SpecularColor = new Vector4(new Vector3( 0.5f), 1.0f)
			};

			world.SpawnPoint = new Vector3(0, 1, 0);
			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
						new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);
			world.Player = playerEntity;

			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
						               Matrix.CreateRotationY(MathHelper.TwoPi * (float)rnd.NextDouble()) *
						               Matrix.CreateRotationX(MathHelper.TwoPi * (float)rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(otherCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
							new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.AddToWorld(world);
			}

			bouncyCubes = new Entity[100];
			rnd = new Random(1);
			for (int i = 0; i < bouncyCubes.Length; i++)
			{
				bouncyCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
						               Matrix.CreateRotationY(MathHelper.TwoPi * (float) rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(bouncyCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
							new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
						.AddToWorld(world);
			}

			deathCubes = new Entity[100];
			rnd = new Random(2);
			for (int i = 0; i < deathCubes.Length; i++)
			{
				deathCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
						               Matrix.CreateRotationY(MathHelper.TwoPi * (float) rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(deathCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
							new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
						.AddToWorld(world);
			}

			teleportCubes = new Entity[100];
			rnd = new Random(3);
			for (int i = 0; i < teleportCubes.Length; i++)
			{
				teleportCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25))*
						               Matrix.CreateRotationY(MathHelper.TwoPi * (float) rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(teleportCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
							new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
						.AddToWorld(world);
			}

			// This is definitely the most natural way to represent the ground
			ground =
				new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(1000.0f))
					.WithPosition(new Vector3(0, -1000.0f, 0))
					.WithStandardEffectParams(groundEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
						new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.AddToWorld(world);

			deathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
						new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			world.LightDirection = Vector3.Normalize(-new Vector3 {X = 0, Y = 1.5f, Z = 1});
			world.AmbientIntensity = 0.3f;
		}
	}
}
