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

			world.SpawnPoint = new Vector3(0, -40, 0);
			world.LightDirection = Vector3.Normalize(new Vector3(3.5f, -3.0f, -3.0f));
			world.CameraPosition = world.SpawnPoint + new Vector3(-3, 0, 0);
			world.AmbientIntensity = 0.45f;

			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 15)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG));

			var platformBuilder = new EntityBuilder().WithModelData(contentCollection.RustPlatform);
			var brickWallBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWall);
			var brickWallWindowBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallWindow);
			var brickWallArrowsHBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallArrowsHorizontal);
			var brickWallArrowsVBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallArrowsVertical);
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


			//Level geometry
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(0, -40, 0).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(4, -40, 0).AddToWorld(world);

			platformBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);

			platformBuilder.Copy().WithPosition(55, -37, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(55, -37, 0).AddToWorld(world);

			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(65, -39, 20).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateScale(1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(65.5f, -33, 15).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateScale(1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(65.5f, -33, 25).AddToWorld(world);

			platformBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.18f, 0.1f, 0.6f) * Matrix.CreateRotationY(MathHelper.ToRadians(0))
				* Matrix.CreateRotationX(MathHelper.ToRadians(-5))).WithPosition(60, -36.01f, 68.2f).AddToWorld(world);


			//Wall jump to hanging platform
			platformBuilder.Copy().WithPosition(60, -34.9f, 88).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -34.9f, 88).AddToWorld(world);

			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(2, 3, 2) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(64.5f, -29.9f, 92).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithPosition(43, -27, 88).AddToWorld(world);
			
			/*dropSignBuilder.Copy().WithTransform(Matrix.CreateScale(4, 4, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(58, -40, 45).AddToWorld(world);
			*/
			/*platformBuilder.Copy().WithPosition(60, -50, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -50, 0).AddToWorld(world);
			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.10f, 0.05f, 0.3f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-20))).WithPosition(72, -53, 0).AddToWorld(world);

			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(125, -80, 0).AddToWorld(world);
			*/





/*



			//Ground
			groundConcreteBuilder.Copy().WithPosition(0, -75, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, -75, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(0, -75, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, -75, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(0, -75, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, -75, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, -75, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, -75, -50).AddToWorld(world);
			//Roof
			groundConcreteBuilder.Copy().WithPosition(0, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(0, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(0, 50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(50, 50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithPosition(100, 50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(150, 50, -50).AddToWorld(world);
			//Walls
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, -50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, 0, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, -50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, 0, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, -50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, 0, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, -50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, 0, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, -50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, 0, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, -50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, 0, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, -50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, 0, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, -50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, 0, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, 50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(0, 50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, 50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, 50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, 50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(100, 50, -30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, 50, 30).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(150, 50, -30).AddToWorld(world);

			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, -50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 0, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 100, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, -50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 0, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 100, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, -50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 0, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(-25, 100, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, -50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 0, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 50, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 100, -50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, -50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 0, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 50, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 100, 0).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, -50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 0, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 50, 50).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)))
					.WithPosition(125, 100, 50).AddToWorld(world);


 */
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
