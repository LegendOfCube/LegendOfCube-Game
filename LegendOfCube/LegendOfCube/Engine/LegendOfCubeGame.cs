﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{

	public class LegendOfCubeGame : Microsoft.Xna.Framework.Game
	{

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private World world;
		private InputSystem inputSystem;
		private PhysicsSystem physicsSystem;
		private RenderSystem renderSystem;
		private GameplaySystem gameplaySystem;

		private Entity playerEntity;
		private Entity[] otherCubes;
		private Entity ground;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			world = new World(1002);
			inputSystem = new InputSystem(this);
			renderSystem = new RenderSystem(this);
			physicsSystem = new PhysicsSystem();
			gameplaySystem = new GameplaySystem();
			Content.RootDirectory = "Content";

			// TODO: Remove this. It's for unlocking frame rate temporarily.
			IsFixedTimeStep = false;
		}

		//Temp entityFactory with an empty prop.
		public Entity CreateEntity(Properties props)
		{
			return world.CreateEntity(props);
		}

		// Overriden XNA methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			renderSystem.Initialize();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			var cubeModel = Content.Load<Model>("Models/cube_plain");

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(Vector3.Zero)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG | Properties.FRICTION_FLAG | Properties.FULL_LIGHT_EFFECT))
					.AddToWorld(world);

			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25)))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithAdditionalProperties(new Properties(Properties.FULL_LIGHT_EFFECT))
						.AddToWorld(world);
			}

			// This is definitely the most natural way to represent the ground
			ground =
				new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(1000.0f))
					.WithPosition(new Vector3(0, -1000.0f, 0))
					.WithAdditionalProperties(new Properties(Properties.FULL_LIGHT_EFFECT))
					.AddToWorld(world);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, world);
			gameplaySystem.processInputData(world, delta);
			physicsSystem.ApplyPhysics(delta, world); // Note, delta should be fixed time step.

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(world);
			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


	}
}
