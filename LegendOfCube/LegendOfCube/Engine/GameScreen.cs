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

		private Entity playerEntity;
		private Entity[] otherCubes;
		private Entity platform;

		public GameScreen(Game game) : base(game)
		{
			World = new World(1002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem();
			cameraSystem = new CameraSystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			inputSystem.ApplyInput(gameTime, World, switcher);
			gameplaySystem.ProcessInputData(World, delta);
			physicsSystem.ApplyPhysics(delta, World); // Note, delta should be fixed time step.
			cameraSystem.OnUpdate(World, delta);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);
		}

		internal override void LoadContent()
		{

			var cubeModel = Game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = Game.Content.Load<Model>("Models/Platform/platform");

			var playerEffect = new StandardEffectParams
			{
				//DiffuseTexture = Game.Content.Load<Texture>("Models/Cube/cube_diff"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/Cube/cube_emissive"),
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

			var groundEffect = new StandardEffectParams
			{
				//DiffuseColor = Color.Gray.ToVector4(),
				//SpecularColor = 0.5f * Color.White.ToVector4()
			};

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(Vector3.Zero)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(World);

			platform =
				new EntityBuilder().WithModel(platformModel)
					.WithTransform(Matrix.CreateScale(0.1f))
					.WithPosition(new Vector3(0, -3, -3))
					.AddToWorld(World);
			/*
			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25)))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(otherCubeEffect)
						.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
						.AddToWorld(World);
			}
			*/


			// This is definitely the most natural way to represent the ground
			//ground =
			//	new EntityBuilder().WithModel(cubeModel)
			//		.WithTransform(Matrix.CreateScale(1000.0f))
			//		.WithPosition(new Vector3(0, -1000.0f, 0))
			//		.WithStandardEffectParams(groundEffect)
			//		.AddToWorld(World);
		}
	}
}
