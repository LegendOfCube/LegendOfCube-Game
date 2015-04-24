using System;
using System.Linq;
using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	public class Level2 : Level
	{
		public Level2() : base("Level 2") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000)
			{
				SpawnPoint = new Vector3(300, 0, 0),
				InitialViewDirection = Vector3.Normalize(new Vector3(1, 0, 0)),
				LightDirection = Vector3.Normalize(new Vector3
				{
					X = -2.0f,
					Y = -1.0f,
					Z = -2.0f
				}),
				AmbientIntensity = 0.25f
			};

			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");
			var wallModel = game.Content.Load<Model>("Models/Brick_Wall/brick_wall");
			var catwalkStartModel = game.Content.Load<Model>("Models/Catwalk/catwalk_start_fix_2");
			var catwalkMiddleModel = game.Content.Load<Model>("Models/Catwalk/catwalk_middle_fix_2");
			var catwalkEndModel = game.Content.Load<Model>("Models/Catwalk/catwalk_end_fix_2");
			var doorModel = game.Content.Load<Model>("Models/Door/door");
			var exitSignModel = game.Content.Load<Model>("Models/Sign_Exit/exit_sign");
			var deathDuctFanModel = game.Content.Load<Model>("Models/Duct/deathcube_fan");
			var cube10Model = game.Content.Load<Model>("Models/Duct/cube10");

			//Builders form Level1
			var movingPartsBuilder = new EntityBuilder().WithModelData(contentCollection.MovingPartsSign);
			var platformBuilder = new EntityBuilder().WithModelData(contentCollection.RustPlatform);
			var brickWallBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWall);
			var brickWallWindowBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallWindow);
			var brickWallArrowsHBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallArrowsHorizontal);
			var brickWallArrowsVBuilder = new EntityBuilder().WithModelData(contentCollection.BrickWallArrowsVertical);
			var windowBarsBuilder = new EntityBuilder().WithModelData(contentCollection.WindowBars);
			var groundConcreteBuilder = new EntityBuilder().WithModelData(contentCollection.GroundConcrete);
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
			var containerBuilder = new EntityBuilder().WithModelData(contentCollection.Container);
			var placeholderWallBuilder = new EntityBuilder().WithModelData(contentCollection.placeholderWall);

			var deathCubeEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.Red.ToVector4()
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

			var bounceEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/platform-normal"),
				DiffuseColor = Color.Yellow.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformCheckpointEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/platform-normal"),
				DiffuseColor = Color.DarkTurquoise.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformTeleportEffect = new StandardEffectParams
			{
				//DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				//NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Purple.ToVector4(),
				//SpecularColor = Color.Purple.ToVector4()
			};

			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube2)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 20)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG));

			world.Player = playerBuilder.AddToWorld(world);

			/***************BACKGROUND GEOMETRY************************/

			// TEST GEOMETRY
			//WALLS
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(1.5f, 2.5f, 1.5f) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG))
				.WithPosition(-30, -15, 0).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationX(MathHelper.ToRadians(90))
				* Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG))
				.WithPosition(320, -40, 50).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(12, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(20, -40, -20).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(10, 3, 3) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(30, -40, 40f).WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
			//FLOOR and ROOF
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(12, 3, 3)).WithPosition(200, -30, 50)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);
			placeholderWallBuilder.Copy().WithTransform(Matrix.CreateScale(12, 3, 3)).WithPosition(200, 30, 40)
				.WithAdditionalProperties(new Properties(Properties.NO_SHADOW_CAST_FLAG)).AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			// Starting platform
			platformBuilder.Copy().WithPosition(new Vector3(0, 0, 0)).AddToWorld(world);
			pillarBuilder.WithPosition(new Vector3(0, 0, 0)).AddToWorld(world);

			//DROP SIGN
			dropSignBuilder.Copy().WithTransform(Matrix.CreateScale(5) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90)))
				.WithPosition(new Vector3(2, 0, 0)).AddToWorld(world);

			// Normal jump platform
			platformBuilder.Copy().WithTransform(Matrix.CreateScale(2, 1, 1)).WithPosition(new Vector3(35, 0, 0)).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(new Vector3(35, 0, 0)).AddToWorld(world);

			// Wall slide platform
			platformBuilder.Copy().WithTransform(Matrix.CreateScale(2, 1, 1)).WithPosition(new Vector3(82, 0, 0)).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(new Vector3(82, 0, 0)).AddToWorld(world);

			//First wallslide
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(1.8f, 7.5f, 10) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(60, 0, -5.5f)).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(50, 8, -5.5f)).AddToWorld(world);
			pillarBuilder.Copy().WithTransform(Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(65, 8, -5.5f)).AddToWorld(world);

			//Signs on platform
			movingPartsBuilder.Copy().WithTransform(Matrix.CreateScale(5) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90)))
				.WithPosition(new Vector3(87, 0, 0)).AddToWorld(world);

			//DUCT PILLAR
			ductBuilder.Copy().WithPosition(new Vector3(113, -5f, 0)).AddToWorld(world);
			ductBuilder.Copy().WithPosition(new Vector3(113, -15f, 0)).AddToWorld(world);
			ductBuilder.Copy().WithPosition(new Vector3(113, -25f, 0)).AddToWorld(world);

			//Slidy slide
			groundWoodBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f, 0.5f, 0.2f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-20)))
				.WithPosition(new Vector3(135.5f, -1.5f, 0)).AddToWorld(world);

			//Wall jump puzzle
			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(200, -17.5f, -7.5f)).AddToWorld(world);

			brickWallArrowsHBuilder.Copy().WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationZ(MathHelper.ToRadians(180)) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(230, -2.5f, 7.5f)).AddToWorld(world);

			brickWallArrowsHBuilder.WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(260, -17.5f, -7.5f)).AddToWorld(world);

			platformBuilder.Copy().WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(300, -15, 0)).WithStandardEffectParams(platformCheckpointEffect)
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG)).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(300, -15, 0).AddToWorld(world);
			railingBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 2)).WithPosition(310, -15, -10).AddToWorld(world);
			railingBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90))).WithPosition(290, -15, -10).AddToWorld(world);
			

			//Climbpuzzle
			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(335f, -15, 5)).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(350f, -10, 0)).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(360f, -5, 10f)).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(375, -0, 25)).AddToWorld(world);

			hangingPlatformBuilder.Copy().WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(400, -5, 45)).AddToWorld(world);

			platformBuilder.Copy().WithTransform(Matrix.CreateScale(1, 1, 2))
				.WithPosition(new Vector3(425, 0, 65)).AddToWorld(world);
			pillarBuilder.Copy().WithPosition(new Vector3(425, 0, 65)).AddToWorld(world);

			//Wall climb
			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(430, -2, 70)).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 5, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 3, 75)).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(420, 8, 70)).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 5, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 12, 65)).AddToWorld(world);

			brickWallArrowsVBuilder.Copy().WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(430, 17, 70)).AddToWorld(world);

			//Teleport!!
			platformBuilder.Copy().WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 25, 80)).WithStandardEffectParams(platformTeleportEffect)
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-100, 10, 0)).WithStandardEffectParams(platformTeleportEffect)
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(-100, 0, 20)).WithStandardEffectParams(platformCheckpointEffect)
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG)).AddToWorld(world);

			//Bounce puzzle
			platformBuilder.Copy().WithPosition(new Vector3(-100, 0, 60)).WithStandardEffectParams(bounceEffect)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-100, 0, 85)).WithStandardEffectParams(bounceEffect)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-100, 0, 115)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-90, -10, 150)).WithStandardEffectParams(bounceEffect)
				.WithVelocity(Vector3.UnitX * -5, 0).WithAI(new [] { new Vector3(-90, -10, 150), new Vector3(-110, -10, 150) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-100, 0, 185)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-150, -30, 185)).WithStandardEffectParams(bounceEffect)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-190, 0, 185)).WithStandardEffectParams(platformCheckpointEffect)
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-180, -10, 155)).WithStandardEffectParams(bounceEffect)
				.WithVelocity(Vector3.UnitX * -5, 0).WithAI(new[] { new Vector3(-180, -10, 150), new Vector3(-200, -10, 150) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

			platformBuilder.Copy().WithPosition(new Vector3(-180, 0, 180))
				.WithStandardEffectParams(bounceEffect).WithVelocity(Vector3.UnitX * 5, 0)
				.WithAI(new[] { new Vector3(-180, 0, 115), new Vector3(-200, 0, 115) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG)).AddToWorld(world);

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

			//First wall
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

			//Second wall
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 4, 3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-193, 0, 33.6f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-185, 0, 33.59f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 1, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-185, 7.5f, 33.59f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			//Deathcubes
			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-190, 3.5f, 38))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-189, 0, 38))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-193, 0, 40))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-191, 0, 36))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-185, 4f, 36))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(2))
					.WithPosition(new Vector3(-185, 0, 38))
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			//Third wall
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 3.5f, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-190, 0, 8.5f))
				.WithStandardEffectParams(wallEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			//Boss 
			Vector3[] list =
			{
				new Vector3(-185, 6, 13), new Vector3(-195, 6, 13), new Vector3(-195, 6, 29),
				new Vector3(-185, 6, 29), new Vector3(-190, 6, 21), new Vector3(-185, 0.5f, 13),
				new Vector3(-195, 0.5f, 13), new Vector3(-195, 0.5f, 29), new Vector3(-185, 0.5f, 29),
				new Vector3(-190, 0.5f, 21)
			};
			Random rnd = new Random();
			list = list.OrderBy(x => rnd.Next()).ToArray();


			new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(3.5f))
					.WithPosition(new Vector3(-184, 0, 12))
					.WithVelocity(Vector3.UnitX * 45, 15)
					.WithStandardEffectParams(deathCubeEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAI(list, false)
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(world);

			//Finish
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-190, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
				.AddToWorld(world);

			//Falling death
			new EntityBuilder()
				.WithPosition(new Vector3(0, -100.0f, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1000, 1, 1000)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			return world;
		}
	}
}
