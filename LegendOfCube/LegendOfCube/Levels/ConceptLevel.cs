using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Levels.Assets;
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

			world.SpawnPoint = new Vector3(0, 5, 0);
			world.CameraPosition = new Vector3(0, 7, -1);

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

			Platform platform = new Assets.Platform(world, game);
			Wall wall = new Assets.Wall(world, game);
			// Starting platform
			platform.Add(Vector3.Zero);

			// Walls and platform to test length gaining wall jumps
			wall.Add(new Vector3(-5, 3, 20));

			wall.Add(new Vector3(10, 6, 40));

			platform.Add(new Vector3(0, 9, 60));

			// Platform to test normal jump
			platform.Add(new Vector3(0, 0, -25));

			// Wall and platform to test height gaining wall jumps
			wall.Add(new Vector3(10, 3, 0));

			platform.Add(new Vector3(0, 12, 0));

			// TODO: Moving platforms
			platform.AddMoving(new Vector3[] { new Vector3(-45, 0, 0), 
				new Vector3(-20, 0, 0), new Vector3(-45, 25, 0) }, 8);

			DeathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);
		}
	}
}
