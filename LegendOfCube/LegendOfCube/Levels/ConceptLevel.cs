using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class ConceptLevel
	{
		private static Entity playerEntity;
		private static Entity[] platforms;
		private static Entity[] walls;
		private static Entity DeathZone;

		public static void CreateLevel(World world, Game game)
		{
			platforms = new Entity[100];
			walls = new Entity[10];

			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");
			var wallModel = game.Content.Load<Model>("Models/Brick_Wall/brick_wall");

			var playerEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var platformEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var wallEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
			};

			var bounceEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Yellow.ToVector4()
			};

			world.SpawnPoint = new Vector3(0, 5, 0);

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 60)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;

			// Starting platform
			platforms[0] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, 0))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			// Walls and platform to test length gaining wall jumps
			walls[0] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-5, 3, 20))
					.WithStandardEffectParams(wallEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			walls[1] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(10, 6, 40))
					.WithStandardEffectParams(wallEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			platforms[1] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 9, 60))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10,0.5f,10)))
					.AddToWorld(world);

			// Platform to test normal jump
			platforms[2] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10,0.5f,10)))
					.AddToWorld(world);

			// Wall and platform to test height gaining wall jumps
			walls[2] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(10, 3, 0))
					.WithStandardEffectParams(wallEffect)
					.WithBoundingVolume(new OBB(new Vector3(0,1.25f,0),Vector3.UnitX,Vector3.UnitY,Vector3.UnitZ,new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			platforms[3] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 12, 0))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0,-.25f,0),Vector3.UnitX,Vector3.UnitY,Vector3.UnitZ,new Vector3(10,.5f,10)))
					.AddToWorld(world);

			// TODO: Moving platforms
			platforms[4] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(-45, 0, 0))
					.WithVelocity(Vector3.UnitX * 8, 0)
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0,-.25f,0),Vector3.UnitX,Vector3.UnitY,Vector3.UnitZ,new Vector3(10,.5f,10)))
					.WithAI(new Vector3[] { new Vector3(-45, 0, 0), new Vector3(-20, 0, 0), new Vector3(-45, 25, 0)}, true)
					.AddToWorld(world);

			// Bounce test jump
			platforms[5] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -75))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			platforms[6] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, -50, -50))
					.WithStandardEffectParams(bounceEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
					.AddToWorld(world);

			//MOAR Platforms
			platforms[7] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(25, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			platforms[8] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(63, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);
			platforms[9] =
				new EntityBuilder().WithModel(platformModel)
					.WithTransform(Matrix.CreateScale(0.2f))
					.WithPosition(new Vector3(40, 0, -35))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			platforms[10] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			platforms[11] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			platforms[12] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -25))
					.WithStandardEffectParams(platformEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
					.AddToWorld(world);

			//Falling death
			DeathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);
			world.LightDirection = Vector3.Normalize(new Vector3
			{
				X = 3,
				Y = -1,
				Z = 0
			});
			world.AmbientIntensity = 0.45f;
		}
	}
}
