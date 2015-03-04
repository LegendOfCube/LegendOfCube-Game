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
		private static Entity DeathZone;

		

		public static void CreateLevel(World world, Game game)
		{
			// Initialize assets.
			Player player = new Assets.Player(world, game);
			Platform platform = new Assets.Platform(world, game);
			Wall wall = new Assets.Wall(world, game);

			player.Add(new Vector3(0, 5, 0));

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

			// Moving platform
			platform.AddMoving(new Vector3[] { new Vector3(-45, 0, 0), 
				new Vector3(-20, 0, 0), new Vector3(-45, 25, 0) }, 8);

			// Bounce test jump
			platform.Add(new Vector3(0, 0, -75));
			platform.AddWithProp(new Vector3(0, -50, -50), new Properties(Properties.BOUNCE_FLAG));

			//Falling death
			DeathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);
		}
	}
}
