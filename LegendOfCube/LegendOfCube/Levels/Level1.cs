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
			var ductBuilder = new EntityBuilder().WithModelData(contentCollection.Duct);
			var trussBuilder = new EntityBuilder().WithModelData(contentCollection.Truss);

			world.Player = playerBuilder.AddToWorld(world);


			//Level geometry
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(0, -40, 0).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(4, -40, 0).AddToWorld(world);

			platformBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);

			platformBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(55, -37, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(55, -37, 0).AddToWorld(world);

			//Wall slide
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(65, -39, 20.5f).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(65.5f, -33, 15.5f).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(65.5f, -33, 25.5f).AddToWorld(world);

			// Ducts
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(71, -55, 11.75f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(66, -55, 11.75f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(61, -55, 11.75f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(61, -50, 11.75f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.25f, 0.5f)).WithPosition(61, -45, 13).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.25f, 0.5f)).WithPosition(61, -45, 18).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.25f, 0.5f)).WithPosition(61, -45, 23).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.25f, 0.5f)).WithPosition(61, -45, 28).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.25f, 0.5f)).WithPosition(61, -45, 33).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(61, -42.5f, 34.25f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(66, -42.5f, 34.25f).AddToWorld(world);
			ductBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.25f)).WithPosition(71, -42.5f, 34.25f).AddToWorld(world);

			trussBuilder.Copy().WithPosition(65, -30, 58).AddToWorld(world);

			platformBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.14f, 0.1f, 0.6f) * Matrix.CreateRotationY(MathHelper.ToRadians(0))
				* Matrix.CreateRotationX(MathHelper.ToRadians(-5))).WithPosition(60, -36.01f, 68.2f).AddToWorld(world);

			//Wall jump to hanging platform
			platformBuilder.Copy().WithPosition(60.5f, -34.9f, 88).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60.5f, -34.9f, 88).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 4, 2)
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(65, -34.9f, 88).AddToWorld(world);

			//Hanging platforms
			hangingPlatformBuilder.Copy().WithPosition(43, -27, 88).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(40, -27, 89).AddToWorld(world);
			dropSignBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(40, -27, 87).AddToWorld(world);
			
			hangingPlatformBuilder.Copy().WithPosition(21.5f, -21.5f, 88).AddToWorld(world);
			
			hangingPlatformBuilder.Copy().WithPosition(0, -27, 88).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(90))).WithPosition(-3, -27, 88).AddToWorld(world);
			
			hangingPlatformBuilder.Copy().WithPosition(-22.5f, -21.5f, 88).AddToWorld(world);
			
			hangingPlatformBuilder.Copy().WithPosition(-45, -27, 88).AddToWorld(world);

			//Wall jump x3
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -27, 65).AddToWorld(world);
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(5) * Matrix.CreateRotationX(MathHelper.ToRadians(180)))
				.WithPosition(-45, -14, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5) * Matrix.CreateRotationX(MathHelper.ToRadians(180)))
				.WithPosition(-44.99f, -14, 45).AddToWorld(world);
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -27, 25).AddToWorld(world);

			trussBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-71, -12, 40).AddToWorld(world);
			trussBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-71, -12, 50).AddToWorld(world);
			trussBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-71, -29.3f, 40).AddToWorld(world);
			trussBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-71, -29.3f, 50).AddToWorld(world);
				//Surrounding Walls
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -14.5f, 65).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-45, -14, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -14.5f, 25).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -39.5f, 65).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-45, -39, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -39.5f, 25).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -52, 65).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-45, -51.5f, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -52, 25).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithPosition(-35, -27, 0).AddToWorld(world);
			
			hangingPlatformBuilder.Copy().WithPosition(-15, -28, 0).AddToWorld(world);

			//2 story climb
			hangingPlatformBuilder.Copy().WithPosition(3, -22, 0).AddToWorld(world);
			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 3, 2)).WithPosition(8.5f, -22, 0).AddToWorld(world);
			platformBuilder.Copy().WithPosition(13, -14, 0).AddToWorld(world);
			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 3, 2) 
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(13, -14, 4.5f).AddToWorld(world);
			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(13, -6, 9).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithPosition(38, 0, 12).AddToWorld(world);

			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(8, 0.1f, 8))
				.WithPosition(38, 0.5f, 12)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
				.AddToWorld(world);

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
