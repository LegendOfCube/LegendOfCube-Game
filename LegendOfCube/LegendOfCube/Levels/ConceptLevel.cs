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
			platforms = new Entity[10];
			walls = new Entity[10];

			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");
			var wallModel = game.Content.Load<Model>("Models/Brick_Wall/brick_wall");
	
			var playerEffect = new StandardEffectParams
			{
				//DiffuseTexture = Game.Content.Load<Texture>("Models/Cube/cube_diff"),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				//DiffuseTexture = Game.Content.Load<Texture>("Models/Cube/cube_diff"),
				//SpecularTexture = Game.Content.Load<Texture>("Models/Cube/cube_specular"),
				//EmissiveTexture = Game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				//NormalTexture = Game.Content.Load<Texture>("Models/Cube/cube_normal"),
				//SpecularColor = Color.White.ToVector4(),
				//EmissiveColor = Color.White.ToVector4()
			};

			world.SpawnPoint = new Vector3(0, 5, 0);

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;

			// Starting platform
			platforms[0] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, 0))
					.WithBoundingVolume(new OBB(new Vector3(0,-0.25f,0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10,0.5f,10)))
					.AddToWorld(world);

			// Walls and platform to test length gaining wall jumps
			walls[0] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-5, 3, 20))
					.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			walls[1] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(10, 6, 40))
					.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			platforms[1] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 9, 60))
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10,0.5f,10)))
					.AddToWorld(world);

			// Platform to test normal jump
			platforms[2] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -25))
					.WithBoundingVolume(new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10,0.5f,10)))
					.AddToWorld(world);

			// Wall and platform to test height gaining wall jumps
			walls[2] =
				new EntityBuilder().WithModel(wallModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(10, 3, 0))
					.WithBoundingVolume(new OBB(new Vector3(0,1.25f,0),Vector3.UnitX,Vector3.UnitY,Vector3.UnitZ,new Vector3(0.5f, 2.5f, 5)))
					.AddToWorld(world);

			platforms[3] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 12, 0))
					.WithBoundingVolume(new OBB(new Vector3(0,-.25f,0),Vector3.UnitX,Vector3.UnitY,Vector3.UnitZ,new Vector3(10,.5f,10)))
					.AddToWorld(world);

			// TODO: Moving platforms
			platforms[4] =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(-45, 0, 0))
					.WithVelocity(Vector3.UnitX * 8, 8)
					.WithAI(new Vector3[] {new Vector3(-45,0,0),new Vector3(-20,0,0)},true)
					.AddToWorld(world);

			DeathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);
		}
	}
}
