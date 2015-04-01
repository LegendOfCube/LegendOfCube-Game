using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class Level1 : ILevelFactory
	{
		public World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000);

			world.SpawnPoint = new Vector3(0, 0, 0);
			world.LightDirection = Vector3.Normalize(new Vector3(3.5f, -3.0f, -3.0f));
			world.CameraPosition = world.SpawnPoint + new Vector3(-3, 0, 0);
			world.AmbientIntensity = 0.45f;

			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 15)
				.WithAcceleration(Vector3.Zero, 60)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG));

			var platformBuilder = new EntityBuilder().WithModelData(contentCollection.RustPlatform);
			var brickWallBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWall);
			var brickWallWindowBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallWindow);
			var windowBarsBuilder = new EntityBuilder().WithModelData(contentCollection.WindowBars);
			var groundConcreteBuilder = new EntityBuilder()	.WithModelData(contentCollection.GroundConcrete);
			var hangingPlatformBuilder = new EntityBuilder().WithModelData(contentCollection.HangingPlatform);
			var dropSignBuilder = new EntityBuilder().WithModelData(contentCollection.DropSign);
			var arrowDownBuilder = new EntityBuilder().WithModelData(contentCollection.SignArrowUp);
			var pillarBuilder = new EntityBuilder().WithModelData(contentCollection.Pillar);
			var groundStoneBuilder = new EntityBuilder().WithModelData(contentCollection.GroundStone);
			var groundWoodBuilder = new EntityBuilder().WithModelData(contentCollection.GroundWood);
			var groundAsphaltBuilder = new EntityBuilder().WithModelData(contentCollection.GroundAsphalt);

			world.Player = playerBuilder.AddToWorld(world);

			// Starting Room
			groundStoneBuilder.Copy().WithPosition(0, -75, 0).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, 0).AddToWorld(world);
			groundStoneBuilder.Copy().WithPosition(100, -75, 0).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, 0).AddToWorld(world);
			groundStoneBuilder.Copy().WithPosition(0, -75, 50).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, 50).AddToWorld(world);
			groundStoneBuilder.Copy().WithPosition(100, -75, 50).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, 50).AddToWorld(world);
			groundStoneBuilder.Copy().WithPosition(0, -75, -50).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, -50).AddToWorld(world);
			groundStoneBuilder.Copy().WithPosition(100, -75, -50).AddToWorld(world);
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, -50).AddToWorld(world);


			//Level geometry
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(0, 0, 0).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) 
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(4, 0, 0).AddToWorld(world);

			platformBuilder.Copy().WithPosition(20, -3, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(20, -3, 0).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithPosition(40, 1, 0).AddToWorld(world);
			dropSignBuilder.Copy().WithTransform(Matrix.CreateScale(4, 4, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(43, 1, 0).AddToWorld(world);

			platformBuilder.Copy().WithPosition(60, -10, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -10, 0).AddToWorld(world);
			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.10f, 0.05f, 0.3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-20))).WithPosition(72, -13, 0).AddToWorld(world);
			
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(125, -40, 0).AddToWorld(world);



			//Falling death
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(1900))
				.WithPosition(0, -1970.0f, 0)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			return world;
		}
	}
}
