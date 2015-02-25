using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	class GameScreen : Screen
	{

		private readonly InputSystem inputSystem;
		private readonly GameplaySystem gameplaySystem;
		private readonly PhysicsSystem physicsSystem;
		private readonly CameraSystem cameraSystem;
		private readonly EventSystem EventSystem;

		private Entity playerEntity;
		private Entity[] otherCubes;
		private Entity ground;
		private Entity DeathZone;
		private Entity[] BouncyCubes;
		private Entity[] DeathCubes;
		private Entity[] TeleportCubes;

		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Vector2 fontPos;

		public GameScreen(Game game) : base(game)
		{
			World = new World(3002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem();
			cameraSystem = new CameraSystem();
			EventSystem = new EventSystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			inputSystem.ApplyInput(gameTime, World, switcher);
			gameplaySystem.ProcessInputData(World, delta);
			physicsSystem.ApplyPhysics(delta, World); // Note, delta should be fixed time step.
			cameraSystem.OnUpdate(World, delta);
			EventSystem.HandleEvents(World);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);

			spriteBatch.Begin();
			string output = "CamPos: " + World.CameraPosition + "\nCamDir: " + (World.Transforms[World.Player.Id].Translation - World.CameraPosition) + "\nCubePos: " + World.Transforms[World.Player.Id].Translation;
			spriteBatch.DrawString(font, output, fontPos, Color.BlueViolet);
			spriteBatch.End();
		}

		internal override void LoadContent()
		{

			var cubeModel = Game.Content.Load<Model>("Models/cube_plain");

			var playerEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Game.Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Game.Content.Load<Texture>("Models/cube_normal"),
				SpecularColor = Color.White.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var bouncyCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Game.Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Game.Content.Load<Texture>("Models/cube_normal"),
				SpecularColor = Color.Yellow.ToVector4(),
				EmissiveColor = Color.Yellow.ToVector4()
			};

			var deathCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Game.Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Game.Content.Load<Texture>("Models/cube_normal"),
				SpecularColor = Color.Red.ToVector4(),
				EmissiveColor = Color.White.ToVector4(),
				DiffuseColor = Color.Red.ToVector4(),
			};

			var teleportCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Game.Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Game.Content.Load<Texture>("Models/cube_normal"),
				SpecularColor = Color.Blue.ToVector4(),
				EmissiveColor = Color.Blue.ToVector4(),
				DiffuseColor = Color.Blue.ToVector4(),
			};

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.Gray.ToVector4(),
				SpecularColor = 0.5f * Color.White.ToVector4()
			};

			World.SpawnPoint = new Vector3(0,1,0);
			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(World.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(World);
			World.Player = playerEntity;

			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25)) * Matrix.CreateRotationY(3.14f * (float)rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(otherCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.AddToWorld(World);
			}

			BouncyCubes = new Entity[100];
			rnd = new Random(1);
			for (int i = 0; i < BouncyCubes.Length; i++)
			{
				BouncyCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25)) * Matrix.CreateRotationY(3.14f * (float)rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(bouncyCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
						.AddToWorld(World);
			}

			DeathCubes = new Entity[100];
			rnd = new Random(2);
			for (int i = 0; i < DeathCubes.Length; i++)
			{
				DeathCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25)) * Matrix.CreateRotationY(3.14f * (float)rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(deathCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
						.AddToWorld(World);
			}

			TeleportCubes = new Entity[100];
			rnd = new Random(3);
			for (int i = 0; i < TeleportCubes.Length; i++)
			{
				TeleportCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25), rnd.Next(1, 25), rnd.Next(1, 25)) * Matrix.CreateRotationY(3.14f * (float)rnd.NextDouble()))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(teleportCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.WithAdditionalProperties(new Properties(Properties.TELEPORT_FLAG))
						.AddToWorld(World);
			}

			// This is definitely the most natural way to represent the ground
			ground =
				new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(1000.0f))
					.WithPosition(new Vector3(0, -1000.0f, 0))
					.WithStandardEffectParams(groundEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.AddToWorld(World);

			DeathZone =
				new EntityBuilder().WithTransform(Matrix.CreateScale(1900))
					.WithPosition(new Vector3(0, -2000.0f, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
					.AddToWorld(World);

			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");
			fontPos = new Vector2(0, 0);
		}
	}
}
