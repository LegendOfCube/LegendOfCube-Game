using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Level
{
	class ConceptLevel
	{
		private static Entity playerEntity;
		private static Entity platform;
		private static Entity DeathZone;

		

		public static void CreateLevel(World world, Game game)
		{
			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");

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

			world.SpawnPoint = new Vector3(0, 25, 0);

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;

			platform =
				new EntityBuilder()//.WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(5, 5, 0.25f)))
					.AddToWorld(world);

			platform =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(10, 0, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(5, 5, 0.25f)))
					.AddToWorld(world);


			platform =
				new EntityBuilder().WithModel(platformModel)
					.WithPosition(new Vector3(0, 0, -10))
					.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(5, 5, 0.25f)))
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
