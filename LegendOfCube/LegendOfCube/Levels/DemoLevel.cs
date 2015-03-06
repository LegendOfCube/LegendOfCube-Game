using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class DemoLevel
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
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;

			// Starting platform
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2,1,1))
				.WithPosition(new Vector3(0, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(30, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(85, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//First wallslide
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2,7,10) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(60, 0, -5))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(115, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Second wallslide
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 10, 5) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(125, 0, -5))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 4, 2))
				.WithPosition(new Vector3(125, 0, 0))
				.WithStandardEffectParams(wallDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(135, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
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
