using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class Level1 : Level
	{

		public Level1() : base("Level 1") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000);

			world.SpawnPoint = new Vector3(0, -40, 0);
			world.LightDirection = Vector3.Normalize(new Vector3(3.5f, -3.0f, -3.0f));
			world.CameraPosition = world.SpawnPoint + new Vector3(-3, 0, 0);
			world.AmbientIntensity = 0.35f;

			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube2)
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
			var catWalkStartBuilder = new EntityBuilder().WithModelData(contentCollection.CatwalkStart);
			var catWalkMiddleBuilder = new EntityBuilder().WithModelData(contentCollection.CatwalkMiddle);
			var catWalkEndBuilder = new EntityBuilder().WithModelData(contentCollection.CatwalkEnd);
			var doorBuilder = new EntityBuilder().WithModelData(contentCollection.Door);
			var exitSignBuilder = new EntityBuilder().WithModelData(contentCollection.ExitSign);
			var fenceBuilder = new EntityBuilder().WithModelData(contentCollection.Fence);
			var barbsBuilder = new EntityBuilder().WithModelData(contentCollection.Barbs);
			var pipeWalkBuilder = new EntityBuilder().WithModelData(contentCollection.PipeWalk);
			var pipeBuilder = new EntityBuilder().WithModelData(contentCollection.Pipe);
			var pipeTurnBuilder = new EntityBuilder().WithModelData(contentCollection.PipeTurn);


			var placeholderWallBuilder = new EntityBuilder().WithModelData(contentCollection.placeholderWall);

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

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.14f, 0.1f, 0.5f)* Matrix.CreateRotationX(MathHelper.ToRadians(-13))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(43.5f, -38.2f, 89.5f).AddToWorld(world);

			pipeWalkBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-10, -45, 90).AddToWorld(world);
			pipeWalkBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(20, -45, 90).AddToWorld(world);
			pipeTurnBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(180))).WithPosition(20, -45, 90).AddToWorld(world);
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(6, 6, 12))
				.WithPosition(49, -48.34f, 99)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.AddToWorld(world);

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
			
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -64.5f, 65).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-45, -64, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -64.5f, 25).AddToWorld(world);
			
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -77, 65).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-45, -76.5f, 45).AddToWorld(world);
			brickWallBuilder.Copy().WithTransform(Matrix.CreateScale(5)).WithPosition(-60, -77, 25).AddToWorld(world);
				//Support Pipes
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(-60.5f, -13, 65).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(-60.5f, -33, 65).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(-60.5f, -13, 25).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(-60.5f, -33, 25).AddToWorld(world);

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

			/*
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(9.8f, 0.1f, 9.8f))
				.WithPosition(38, 0.001f, 12)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
				.AddToWorld(world);
			*/

			//Win zone building
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f)).WithPosition(86, -15.5f, 20.5f).WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG)).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73.5f, -28, 20.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(87.5f, -28, 36.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(87.5f, -28, 4.5f).AddToWorld(world);
				//LV -1
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73.5f, -53, 20.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(87.5f, -53, 36.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(87.5f, -53, 4.5f).AddToWorld(world);
				//LV -2
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73.5f, -78, 20.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(87.5f, -78, 36.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(87.5f, -78, 4.5f).AddToWorld(world);
				//Centre fences
			fenceBuilder.Copy().WithPosition(73, -20, 10.5f).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73, -10, 10.5f).AddToWorld(world);
			fenceBuilder.Copy().WithPosition(73, -20, 20.5f).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73, -10, 20.5f).AddToWorld(world);
			fenceBuilder.Copy().WithPosition(73, -20, 30.5f).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(73, -10, 30.5f).AddToWorld(world);
				//Left fences
			fenceBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(78, -20, 5).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(78, -10, 5).AddToWorld(world);
			fenceBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(88, -20, 5).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(88, -10, 5).AddToWorld(world);
				//Right fences
			fenceBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(78, -20, 36).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithPosition(78, -10, 36).AddToWorld(world);
			fenceBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(88, -20, 36).AddToWorld(world);
			barbsBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(-45)) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithPosition(88, -10, 36).AddToWorld(world);
			
			//platformBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 3) * Matrix.CreateRotationZ(MathHelper.ToRadians(-45))).WithPosition(72, -14.5f, 20.5f).AddToWorld(world);
			//Falling death
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(1900))
				.WithPosition(0, -1970.0f, 0)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			
			// TEST GEOMETRY
				//WALLS
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG))
				.WithPosition(-100, -40, 50).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG))
				.WithPosition(99, -40, 50).AddToWorld(world);							
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(5, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(20, -40, -20).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(5, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(20, -40, 110.5f).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
				//FLOOR and ROOF
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(5, 3, 3)).WithPosition(20, -70, 50)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(5, 3, 3)).WithPosition(0, 30, 40)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);

			// Catwalk lower
			catWalkStartBuilder.Copy().WithPosition(new Vector3(0, -40, -15.3f)).AddToWorld(world);
			catWalkMiddleBuilder.Copy().WithPosition(new Vector3(10, -40, -15.3f)).AddToWorld(world);
			catWalkEndBuilder.Copy().WithPosition(new Vector3(20, -40, -15.3f)).AddToWorld(world);
			doorBuilder.Copy().WithTransform(Matrix.CreateScale(2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(0, -39.5f,-20)).AddToWorld(world);
			doorBuilder.Copy().WithTransform(Matrix.CreateScale(2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(20, -39.5f, -20)).AddToWorld(world);
			exitSignBuilder.Copy().WithTransform(Matrix.CreateScale(3)).WithPosition(new Vector3(0, -35, -20)).AddToWorld(world);
			exitSignBuilder.Copy().WithTransform(Matrix.CreateScale(3)).WithPosition(new Vector3(20, -35, -20)).AddToWorld(world);
			// Catwalk upper
			catWalkStartBuilder.Copy().WithPosition(new Vector3(0, -20, -15.3f)).AddToWorld(world);
			catWalkMiddleBuilder.Copy().WithPosition(new Vector3(10, -20, -15.3f)).AddToWorld(world);
			catWalkEndBuilder.Copy().WithPosition(new Vector3(20, -20, -15.3f)).AddToWorld(world);
			doorBuilder.Copy().WithTransform(Matrix.CreateScale(2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(0, -19.5f, -20)).AddToWorld(world);
			doorBuilder.Copy().WithTransform(Matrix.CreateScale(2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(20, -19.5f, -20)).AddToWorld(world);
			exitSignBuilder.Copy().WithTransform(Matrix.CreateScale(3)).WithPosition(new Vector3(0, -15, -20)).AddToWorld(world);
			exitSignBuilder.Copy().WithTransform(Matrix.CreateScale(3)).WithPosition(new Vector3(20, -15, -20)).AddToWorld(world);
			
			return world;
		}
	}
}
