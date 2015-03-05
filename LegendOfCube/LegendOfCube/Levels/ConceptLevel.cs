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

		public static void CreateLevel(World world, Game game)
		{

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

			var wallDeathEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				SpecularColor = Color.DarkRed.ToVector4()
			};

			var bounceEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Yellow.ToVector4()
			};

			var platformDeathEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.DarkRed.ToVector4()
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
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			// Walls and platform to test length gaining wall jumps
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(-5, 3, 20))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(10, 6, 40))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 9, 60))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			// Platform to test normal jump
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			// Wall and platform to test height gaining wall jumps
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(10, 3, 0))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 12, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, .5f, 10)))
				.AddToWorld(world);

			// TODO: Moving platforms
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-45, 0, 0))
				.WithVelocity(Vector3.UnitX * 8, 0)
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, .5f, 10)))
				.WithAI(new Vector3[] { new Vector3(-45, 0, 0), new Vector3(-20, 0, 0), new Vector3(-45, 25, 0) }, true)
				.AddToWorld(world);

			// Bounce test jump
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -75))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, -25, -50))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			//Help bounce platform
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-10, 0, -50))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Long jump
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(25, 0, -25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(63, 0, -25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Small platforms
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(new Vector3(35, 0, -35))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(new Vector3(45, 0, -35))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(new Vector3(55, 0, -35))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Trick jump
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(90, 0, -25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(75, 1, -25))
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(75, 9, -25))
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(75, 4.5f, -32))
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(75, 4.5f, -18))
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			//Crush trap#1
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 20, -85))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAI(new[] {new Vector3(0, 20, -85), new Vector3(0, 1, -85)}, true )
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -85))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -95))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Crush trap#2
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(-7, 1, -105))
				.WithVelocity(Vector3.UnitX * 10, 0)
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAI(new [] {new Vector3(-7, 0.4f, -105), new Vector3(-0.7f, 0.4f, -105)}, true)
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(7, 1, -105))
				.WithVelocity(Vector3.UnitX * -10, 0)
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAI(new[] { new Vector3(7, 0.4f, -105), new Vector3(0.7f, 0.4f, -105) }, true)
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -105))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(0, 0, -115))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Falling death
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
