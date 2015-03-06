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
				DiffuseColor = Color.DarkRed.ToVector4()
			};

			var bounceEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Yellow.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformCheckpointEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.DarkTurquoise.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformDeathEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Red.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformTeleportEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.LightBlue.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			world.SpawnPoint = new Vector3(0, 5, 0);
			//world.SpawnPoint = new Vector3(-190, 0, 50);

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
			
			//Checkpoint
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.5f))
				.WithPosition(new Vector3(150, 0, 0))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Fallpuzzle
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(172.5f, -15, 0))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(157.5f, -15, 0))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(165, -15, 7.5f))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(165, -15, -7.5f))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			//Fallpuzzle #2
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(171.5f, -25, 0))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(158.5f, -25, 0))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(165, -25, 6.5f))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(165, -25, -6.5f))
				.WithStandardEffectParams(platformDeathEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(165, -35, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Wall jump puzzle
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(180, -35, 7.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(200, -35, -7.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(220, -35, 7.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(240, -35, -7.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(255, -35, 0))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Climbpuzzle
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(252.5f, -30, 2.5f))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(257.5f, -25, 2.5f))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(257.5f, -20, -2.5f))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(252.5f, -15, -2.5f))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(265, -10, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(265, -5, 15))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.5f))
				.WithPosition(new Vector3(265, 0, 30))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Teleport!!
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(265, 10, 45))
				.WithStandardEffectParams(platformTeleportEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-100, 10, 0))
				.WithStandardEffectParams(platformTeleportEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(-100, 0, 20))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Bounce puzzle
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 60))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 85))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 115))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-150, -30, 115))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-190, 0, 115))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Hidden shortcut
			new EntityBuilder()
				.WithPosition(new Vector3(-60, 0, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(50, 1, 10)))
				.AddToWorld(world);

			new EntityBuilder()
				.WithPosition(new Vector3(-150, 0, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(40, 1, 10)))
				.AddToWorld(world);

			//First labyrinth
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 7f))
				.WithPosition(new Vector3(-180, 0, 75))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-187, 0, 91.6f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-193, 0, 83.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-187, 0, 75))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-193, 0, 66.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-187, 0, 58.4f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 7f))
				.WithPosition(new Vector3(-200, 0, 75))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2.4f, 2, 3.9f))
				.WithPosition(new Vector3(-190, 11, 75))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Labyrinth #2
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-190, 0, 50))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 7f))
				.WithPosition(new Vector3(-200, 0, 25))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4f, 7f))
				.WithPosition(new Vector3(-180, 0, 25))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2.2f, 2, 3.5f))
				.WithPosition(new Vector3(-190, 0, 25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2.4f, 2, 3.9f))
				.WithPosition(new Vector3(-190, 11, 25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-187, 0, 41.6f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-195, 0, 41.59f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 1, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-195, 7.5f, 41.59f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-190, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Falling death
			new EntityBuilder()
				.WithPosition(new Vector3(0, -100.0f, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1000, 1, 1000)))
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
