using System;
using System.Linq;
using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class DemoLevel
	{
		private static Entity playerEntity;

		public static void CreateLevel(World world, Game game)
		{
			world.SpawnPoint = new Vector3(0, 0, 0);
			//world.SpawnPoint = new Vector3(-190, 0, 50);
			//world.SpawnPoint = new Vector3(255, -35, 0);
			//world.SpawnPoint = (new Vector3(425, 0, 65));
			world.CameraPosition = world.SpawnPoint + new Vector3(-1.0f, 2.0f, 0.0f);
			world.LightDirection = Vector3.Normalize(new Vector3
			{
				X = -1.0f,
				Y = -1.0f,
				Z = -1.0f
			});
			world.AmbientIntensity = 0.25f;

			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");
			var wallModel = game.Content.Load<Model>("Models/Brick_Wall/brick_wall");
			var dropSignModel = game.Content.Load<Model>("Models/Sign_Drop/danger_drop");
			var catwalkStartModel = game.Content.Load<Model>("Models/Catwalk/catwalk_start_fix_2");
			var catwalkMiddleModel = game.Content.Load<Model>("Models/Catwalk/catwalk_middle_fix_2");
			var catwalkEndModel = game.Content.Load<Model>("Models/Catwalk/catwalk_end_fix_2");
			var doorModel = game.Content.Load<Model>("Models/Door/door");
			var exitSignModel = game.Content.Load<Model>("Models/Sign_Exit/exit_sign");
			var pillarModel = game.Content.Load<Model>("Models/Platform/pillar");
			var deathDuctModel = game.Content.Load<Model>("Models/Duct/deathcube");
			var deathDuctFanModel = game.Content.Load<Model>("Models/Duct/deathcube_fan");
			var cube10Model = game.Content.Load<Model>("Models/Duct/cube10");
			var movingPartsSignModel = game.Content.Load<Model>("Models/Sign_Moving/moving_parts");

			var playerEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

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

			var blackEffect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0), 0),
				SpecularColor = Color.Gray.ToVector4()
			};

			var pillarEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/metal_rust_tex_01"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var stonePlatformEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Paved_Stone/paved_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Paved_Stone/paved_n"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var dropSignEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Sign_Drop/danger_drop_d"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var doorEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Door/door_d"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var catwalkEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Catwalk/catwalk-d"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var exitSignEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Sign_Exit/exit_d_e"),
				EmissiveTexture = game.Content.Load<Texture>("Models/Sign_Exit/exit_d_e"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var movingSignEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Sign_Moving/moving_parts_d"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var ductEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Duct/duct_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Duct/duct_n"),
				SpecularColor = Color.Gray.ToVector4()
			};
			var ductFanEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Duct/duct_d"),
				SpecularColor = Color.Gray.ToVector4()
			};

			var wallEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
			};

			var wallHorizontalEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_arrows_h_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
			};

			var wallVerticalEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_arrows_v_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
			};

			var wallDeathEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_d"),
				NormalTexture = game.Content.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
				DiffuseColor = Color.DarkRed.ToVector4()
			};

			var bounceEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Yellow.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformCheckpointEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.DarkTurquoise.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformDeathEffect = new StandardEffectParams
			{
				DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Red.ToVector4(),
				SpecularColor = Color.Gray.ToVector4()
			};

			var platformTeleportEffect = new StandardEffectParams
			{
				//DiffuseTexture = game.Content.Load<Texture>("Models/Platform/rusted metal-d"),
				//NormalTexture = game.Content.Load<Texture>("Models/Platform/rust_normal_sharp"),
				DiffuseColor = Color.Purple.ToVector4(),
				//SpecularColor = Color.Purple.ToVector4()
			};

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 60)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
						new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			// Starting platform
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(0, 0, 0))
				.WithStandardEffectParams(stonePlatformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);
			new EntityBuilder().WithModel(pillarModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(0, 0, 0))
				.WithStandardEffectParams(pillarEffect)
				.AddToWorld(world);
			//DROP SIGN
			new EntityBuilder().WithModel(dropSignModel)
				.WithTransform(Matrix.CreateScale(5, 5, 5) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90)))
				.WithPosition(new Vector3(2, 0, 0))
				.WithStandardEffectParams(dropSignEffect)
				.AddToWorld(world);

			// Normal jump platform
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(30, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);
			new EntityBuilder().WithModel(pillarModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(30, 0, 0))
				.WithStandardEffectParams(pillarEffect)
				.AddToWorld(world);

			// Wall slide platform
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(82, 0, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);
			new EntityBuilder().WithModel(pillarModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(82, 0, 0))
				.WithStandardEffectParams(pillarEffect)
				.AddToWorld(world);

			//First wallslide
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 7.5f, 10) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(60, 0, -5.5f))
				.WithStandardEffectParams(wallHorizontalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);
			new EntityBuilder().WithModel(pillarModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(50, 8, -5.5f))
				.WithStandardEffectParams(pillarEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(pillarModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(70, 8, -5.5f))
				.WithStandardEffectParams(pillarEffect)
				.AddToWorld(world);

			//Signs on wall
			new EntityBuilder().WithModel(movingPartsSignModel)
				.WithTransform(Matrix.CreateScale(5, 5, 5) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90)))
				.WithPosition(new Vector3(87, 0, 0))
				.WithStandardEffectParams(movingSignEffect)
				.AddToWorld(world);
			
			//Wall high jump (DEATH)
			new EntityBuilder().WithModel(deathDuctModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(105, 0, 0))
				.WithStandardEffectParams(ductEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 5, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 10, 10)))
				.AddToWorld(world);
			//BLACK HACK
			new EntityBuilder().WithModel(cube10Model)
				.WithTransform(Matrix.CreateScale(0.95f, 0.95f, 0.1f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(105, 0.3f, 0))
				.WithStandardEffectParams(blackEffect)
				.AddToWorld(world);
			//THE FAN
			new EntityBuilder().WithModel(deathDuctFanModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(105, 0, 0))
				.WithStandardEffectParams(ductFanEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 5, -5), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(7, 7, 0.5f)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			//DUCT PILLAR
			new EntityBuilder().WithModel(cube10Model)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(105, -10, 0))
				.WithStandardEffectParams(ductEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 5, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 10, 10)))
				.AddToWorld(world);
			new EntityBuilder().WithModel(cube10Model)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(105, -20, 0))
				.WithStandardEffectParams(ductEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 5, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 10, 10)))
				.AddToWorld(world);

			//Checkpoint
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2, 1, 1))
				.WithPosition(new Vector3(125, 0, 0))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			/***************BACKGROUND GEOMETRY************************/

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(1, 100, 100) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(0, -50, -15))
				.WithStandardEffectParams(platformTeleportEffect)
				.AddToWorld(world);

			//CATWALK UPPER
			new EntityBuilder().WithModel(catwalkStartModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(0, 10, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(catwalkMiddleModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(10, 10, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(catwalkEndModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(20, 10, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			
			//Door 1
			new EntityBuilder().WithModel(doorModel)
				.WithTransform(Matrix.CreateScale(2, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(0, 10.5f,-14.7f))
				.WithStandardEffectParams(doorEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(exitSignModel)
				.WithTransform(Matrix.CreateScale(3, 3, 3))
				.WithPosition(new Vector3(0, 15, -14.7f))
				.WithStandardEffectParams(exitSignEffect)
				.AddToWorld(world);
			//Door 2
			new EntityBuilder().WithModel(doorModel)
				.WithTransform(Matrix.CreateScale(2, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(20, 10.5f, -14.7f))
				.WithStandardEffectParams(doorEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(exitSignModel)
				.WithTransform(Matrix.CreateScale(3, 3, 3))
				.WithPosition(new Vector3(20, 15, -14.7f))
				.WithStandardEffectParams(exitSignEffect)
				.AddToWorld(world);

			//CATWALK LOWER
			new EntityBuilder().WithModel(catwalkStartModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(0, 0, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(catwalkMiddleModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(10, 0, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(catwalkEndModel)
				.WithTransform(Matrix.CreateScale(1, 1, 1))
				.WithPosition(new Vector3(20, 0, -10))
				.WithStandardEffectParams(catwalkEffect)
				.AddToWorld(world);
			//Door 1
			new EntityBuilder().WithModel(doorModel)
				.WithTransform(Matrix.CreateScale(2, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(0, 0.5f, -14.7f))
				.WithStandardEffectParams(doorEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(exitSignModel)
				.WithTransform(Matrix.CreateScale(3, 3, 3))
				.WithPosition(new Vector3(0, 5, -14.7f))
				.WithStandardEffectParams(exitSignEffect)
				.AddToWorld(world);
			//Door 2
			new EntityBuilder().WithModel(doorModel)
				.WithTransform(Matrix.CreateScale(2, 2, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(20, 0.5f, -14.7f))
				.WithStandardEffectParams(doorEffect)
				.AddToWorld(world);
			new EntityBuilder().WithModel(exitSignModel)
				.WithTransform(Matrix.CreateScale(3, 3, 3))
				.WithPosition(new Vector3(20, 5, -14.7f))
				.WithStandardEffectParams(exitSignEffect)
				.AddToWorld(world);
			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			//Slidy slide
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(7, 1, 1) * Matrix.CreateRotationZ(MathHelper.ToRadians(-35)))
				.WithPosition(new Vector3(164, -19, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Wall jump puzzle
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(200, -35, -7.5f))
				.WithStandardEffectParams(wallHorizontalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationZ(MathHelper.ToRadians(180)) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(220, -20, 7.5f))
				.WithStandardEffectParams(wallHorizontalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(3.5f, 6, 4) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(240, -35, -7.5f))
				.WithStandardEffectParams(wallHorizontalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(255, -35, 0))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Climbpuzzle
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(275f, -30, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(300f, -25, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(325f, -20, 0))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(350f, -15, 10f))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(375, -10, 25))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(0.75f))
				.WithPosition(new Vector3(400, -5, 45))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(1, 1, 2))
				.WithPosition(new Vector3(425, 0, 65))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			//Wall climb
			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(430, -2, 70))
				.WithStandardEffectParams(wallVerticalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 5, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 3, 75))
				.WithStandardEffectParams(wallVerticalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(420, 8, 70))
				.WithStandardEffectParams(wallVerticalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 5, 2) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 12, 65))
				.WithStandardEffectParams(wallVerticalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(wallModel)
				.WithTransform(Matrix.CreateScale(2, 5, 2))
				.WithPosition(new Vector3(430, 17, 70))
				.WithStandardEffectParams(wallVerticalEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, 1.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 2.5f, 5)))
				.AddToWorld(world);

			//Teleport!!
			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(425, 25, 80))
				.WithStandardEffectParams(platformTeleportEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateRotationX(MathHelper.ToRadians(90)))
				.WithPosition(new Vector3(-100, 10, 0))
				.WithStandardEffectParams(platformTeleportEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(new Vector3(-100, 0, 20))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Bounce puzzle
			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 60))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 85))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 115))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-90, -10, 150))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithVelocity(Vector3.UnitX * -5, 0)
				.WithAI(new [] { new Vector3(-90, -10, 150), new Vector3(-110, -10, 150) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-100, 0, 185))
				.WithStandardEffectParams(platformEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-150, -30, 185))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-190, 0, 185))
				.WithStandardEffectParams(platformCheckpointEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-180, -10, 155))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithVelocity(Vector3.UnitX * -5, 0)
				.WithAI(new[] { new Vector3(-180, -10, 150), new Vector3(-200, -10, 150) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			new EntityBuilder().WithModel(platformModel)
				.WithPosition(new Vector3(-180, 0, 180))
				.WithStandardEffectParams(bounceEffect)
				.WithBoundingVolume(new OBB(new Vector3(0, -0.25f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(10, 0.5f, 10)))
				.WithVelocity(Vector3.UnitX * 5, 0)
				.WithAI(new[] { new Vector3(-180, 0, 115), new Vector3(-200, 0, 115) }, true)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

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
				.WithAdditionalProperties(new Properties(Properties.CHECKPOINT_FLAG))
				.AddToWorld(world);

			//Falling death
			new EntityBuilder()
				.WithPosition(new Vector3(0, -100.0f, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1000, 1, 1000)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);
		}
	}
}
