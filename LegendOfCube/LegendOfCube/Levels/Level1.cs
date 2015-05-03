using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace LegendOfCube.Levels
{
	class Level1 : Level
	{

		public Level1() : base("Level 1") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000)
			{
				SpawnPoint = new Vector3(0, -40, 0),
				LightDirection = Vector3.Normalize(new Vector3(3.5f, -3.0f, -3.0f)),
				InitialViewDirection = Vector3.Normalize(new Vector3(1, 0, 0)),
				AmbientIntensity = 0.25f
			};

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
			var railingBuilder = new EntityBuilder().WithModelData(contentCollection.Railing);
			var grassSmallBuilder = new EntityBuilder().WithModelData(contentCollection.GrassSmall);
			var grassRoundBuilder = new EntityBuilder().WithModelData(contentCollection.GrassRound);
			var grassLongBuilder = new EntityBuilder().WithModelData(contentCollection.GrassLong);
			var containerRedBuilder = new EntityBuilder().WithModelData(contentCollection.ContainerRed);
			var containerBlueBuilder = new EntityBuilder().WithModelData(contentCollection.ContainerBlue);
			var containerGreenBuilder = new EntityBuilder().WithModelData(contentCollection.ContainerGreen);
			var cart1Builder = new EntityBuilder().WithModelData(contentCollection.Cart1);
			var cart2Builder = new EntityBuilder().WithModelData(contentCollection.Cart2);
			var trainDoorBuilder = new EntityBuilder().WithModelData(contentCollection.TrainDoor);
			var trainDoorClosedBuilder = new EntityBuilder().WithModelData(contentCollection.TrainDoorClosed);
			var railsBuilder = new EntityBuilder().WithModelData(contentCollection.Rails);
			var locomotiveBuilder = new EntityBuilder().WithModelData(contentCollection.Locomotive);
			var woodenPlatformBuilder = new EntityBuilder().WithModelData(contentCollection.WoodenPlatform);
			var woodPileBuilder = new EntityBuilder().WithModelData(contentCollection.WoodPile);
			//var roofBuilder = new EntityBuilder().WithModelData(contentCollection.Roof);


			var placeholderWallBuilder = new EntityBuilder().WithModelData(contentCollection.placeholderWall);

			world.Player = playerBuilder.AddToWorld(world);

			//MediaPlayer.Play(contentCollection.level1full);
			MediaPlayer.Play(contentCollection.level1amb);
			MediaPlayer.IsRepeating = true;

			//Level geometry
			groundStoneBuilder.Copy().WithTransform(Matrix.CreateScale(0.25f, 0.25f, 0.25f)).WithPosition(0, -40, 0).AddToWorld(world);
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90))
				* Matrix.CreateRotationZ(MathHelper.ToRadians(-90))).WithPosition(4, -40, 0).AddToWorld(world);

			railingBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(25, -43, -4.9f).AddToWorld(world);
			railingBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(25, -43, 4.9f).AddToWorld(world);
			platformBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(30, -43, 0).AddToWorld(world);

			railingBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(50, -37, -4.9f).AddToWorld(world);
			railingBuilder.Copy().WithPosition(59.9f, -37, -5).AddToWorld(world);
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

			railingBuilder.Copy().WithPosition(55.1f, -37, 45).AddToWorld(world);
			railingBuilder.Copy().WithPosition(64.9f, -37, 45).AddToWorld(world);
			platformBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60, -37, 50).AddToWorld(world);

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.14f, 0.1f, 0.6f)* Matrix.CreateRotationX(MathHelper.ToRadians(-5)))
				.WithPosition(60, -36.01f, 68.2f).AddToWorld(world);

			grassSmallBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithPosition(64.8f, -37, 54.9f).AddToWorld(world);
			grassSmallBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithPosition(64.8f, -37, 49.9f).AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			//Wall jump to hanging platform
			railingBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 0.9f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(55.5f, -34.9f, 92.9f).AddToWorld(world);

			grassLongBuilder.Copy().WithTransform(Matrix.CreateScale(2.5f, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithPosition(64.2f, -34.9f, 88).AddToWorld(world);

			platformBuilder.Copy().WithPosition(60.5f, -34.9f, 88).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(60.5f, -34.9f, 88).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 4, 2)
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(65, -34.9f, 88).AddToWorld(world);

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.14f, 0.1f, 0.5f)* Matrix.CreateRotationX(MathHelper.ToRadians(-13))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(43.5f, -38.2f, 89.5f).AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			//Lower level Easy road
			pipeWalkBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-10, -44.5f, 90).AddToWorld(world);
			pipeWalkBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(20, -44.5f, 90).AddToWorld(world);
			pipeTurnBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(180))).WithPosition(20, -44.5f, 90).AddToWorld(world);
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(6, 6, 12))
				.WithPosition(49, -47.84f, 99)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.AddToWorld(world);
			pipeTurnBuilder.Copy().WithTransform(Matrix.CreateRotationZ(MathHelper.ToRadians(0))).WithPosition(-10, -44.5f, 90).AddToWorld(world);
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(6, 6, 12))
				.WithPosition(-38, -47.15f, 99)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.AddToWorld(world);

			woodPileBuilder.Copy().WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(5, -41.2f, 90).AddToWorld(world);

			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.14f, 0.1f, 0.5f) * Matrix.CreateRotationX(MathHelper.ToRadians(-13))
				 * Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-37.2f, -38.7f, 90).AddToWorld(world);
			railingBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-59.2f, -36.18f, 94.9f).AddToWorld(world);
			railingBuilder.Copy().WithPosition(-59.1f, -36.18f, 85).AddToWorld(world);
			platformBuilder.Copy().WithPosition(-54.2f, -36.18f, 90).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(-54.2f, -36.18f, 90).AddToWorld(world);

			grassSmallBuilder.Copy().WithTransform(Matrix.CreateScale(1.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(125)))
				.WithPosition(-59.1f, -36.18f, 94.9f).AddToWorld(world);

			railingBuilder.Copy().WithPosition(-49.3f, -31.18f, 60).AddToWorld(world);
			platformBuilder.Copy().WithPosition(-54.2f, -31.18f, 65).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(-54.2f, -31.18f, 65).AddToWorld(world);
			grassLongBuilder.Copy().WithTransform(Matrix.CreateScale(2.5f, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(-58.5f, -31.18f, 65).AddToWorld(world);

			railingBuilder.Copy().WithPosition(-59.1f, -25.99f, 40).AddToWorld(world);
			railingBuilder.Copy().WithPosition(-49.3f, -25.99f, 40).AddToWorld(world);
			platformBuilder.Copy().WithPosition(-54.2f, -25.99f, 45).AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

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
			arrowDownBuilder.Copy().WithTransform(Matrix.CreateScale(3, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(210))).WithPosition(-48, -27, 86).AddToWorld(world);

			//Background building
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -18, 30.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -18, 60.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-97.5f, -18, 76.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-97.5f, -18, 14.5f).AddToWorld(world);
				//LV -1
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -43, 30.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -43, 60.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-97.5f, -43, 76.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-97.5f, -43, 14.5f).AddToWorld(world);
				//LV -2
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -68, 30.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(180))).WithPosition(-82.5f, -68, 60.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-97.5f, -68, 76.5f).AddToWorld(world);
			groundConcreteBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f, 0.5f, 0.6f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-97.5f, -68, 14.5f).AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

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

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

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
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
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
				//Back fences
			fenceBuilder.Copy().WithPosition(93, -20, 10.5f).AddToWorld(world);
			fenceBuilder.Copy().WithPosition(93, -20, 20.5f).AddToWorld(world);
			fenceBuilder.Copy().WithPosition(93, -20, 30.5f).AddToWorld(world);
			
			/*Falling death
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(1900))
				.WithPosition(0, -1970.0f, 0)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);*/
			
			// TEST GEOMETRY
				//WALLS
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))* Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.WithPosition(-100, -40, 50)
				.AddToWorld(world);
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))* Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.WithPosition(99, -40, 50)
				.AddToWorld(world);
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(20, -40, -20)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(20, -40, 110.5f)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);
				//FLOOR and ROOF
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5, 3, 3))
				.WithPosition(20, -70, 50)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);
			placeholderWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5, 0.5f, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(180)))
				.WithPosition(0, 30, 40)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG | Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);
			
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
			
			//Floor decor
			containerGreenBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(30, -70, 40).AddToWorld(world);
			containerRedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(30, -59.9f, 40).AddToWorld(world);
			containerBlueBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(30, -70, 50).AddToWorld(world);
			containerGreenBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(30, -59.9f, 50).AddToWorld(world);
			containerRedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(30, -70, 60).AddToWorld(world);
			containerBlueBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(30)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(15, -70, 25).AddToWorld(world);

			// Active trains
			railsBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-10, -70, 10).AddToWorld(world);
			cart2Builder.Copy()
				.WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -70, -95), new Vector3(-10, -70, 170), new Vector3(-10, -170, 0) }, false)
				.AddToWorld(world);
			containerGreenBuilder.Copy()
				.WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -66, -95), new Vector3(-10, -66, 170), new Vector3(-10, -166, 0) }, false)
				.AddToWorld(world);
			cart1Builder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -70, -70), new Vector3(-10, -70, 195), new Vector3(-10, -170, 25) }, false)
				.AddToWorld(world);
			cart2Builder.Copy()
				.WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -70, -120), new Vector3(-10, -70, 145), new Vector3(-10, -170, -25) }, false)
				.AddToWorld(world);
			containerRedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -66, -120), new Vector3(-10, -66, 145), new Vector3(-10, -166, -25) }, false)
				.AddToWorld(world);
			locomotiveBuilder.Copy()
				.WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)))
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithAI(new[] { new Vector3(-10, -70, -42), new Vector3(-10, -70, 223), new Vector3(-10, -170, 53) }, false)
				.AddToWorld(world);
			trainDoorBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-10, -70, -20).AddToWorld(world);
			trainDoorBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-10, -70, 104).AddToWorld(world);

			// Passive trains
			railsBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-30, -70, 10).AddToWorld(world);
			cart2Builder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-30, -70, 70).AddToWorld(world);
			containerBlueBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(-30, -66, 70).AddToWorld(world);
			cart1Builder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-30, -70, 40).AddToWorld(world);
			cart2Builder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-30, -70, 10).AddToWorld(world);
			containerRedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithBoundingVolume(OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)).WithPosition(-30, -66, 10).AddToWorld(world);
			trainDoorClosedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(-30, -70, -20).AddToWorld(world);
			trainDoorClosedBuilder.Copy().WithTransform(Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90))).WithPosition(-30, -70, 104).AddToWorld(world);

			woodPileBuilder.Copy().WithTransform(Matrix.CreateScale(3)).WithPosition(10, -70, -3).AddToWorld(world);

			return world;
		}
	}
}
