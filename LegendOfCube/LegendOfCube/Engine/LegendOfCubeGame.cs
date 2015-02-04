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


		private Entity playerEntity;
		private Entity[] otherCubes;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			world = new World(100);
			inputSystem = new InputSystem(this);
			renderSystem = new RenderSystem(this);
			physicsSystem = new PhysicsSystem();

			Content.RootDirectory = "Content";
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

			// Temporary code to create a cube entity that should render.
			playerEntity = CreateEntity(new Properties(Properties.TRANSFORM | Properties.MODEL | Properties.INPUT_FLAG | Properties.VELOCITY | Properties.GRAVITY_FLAG));
			otherCubes = new Entity[50];
			Random rnd = new Random();
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] = CreateEntity(new Properties(Properties.TRANSFORM | Properties.MODEL));
				Matrix test = Matrix.CreateTranslation(new Vector3(rnd.Next(-50, 50), 0, rnd.Next(-50, 50)));
				world.Transforms[otherCubes[i].Id] = test;
			}

			world.Transforms[playerEntity.Id] = Matrix.Identity;
			world.Velocities[playerEntity.Id] = new Vector3(0, 0, 0);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			BasicEffect effect = new BasicEffect(GraphicsDevice);
			effect.EnableDefaultLighting();
			effect.PreferPerPixelLighting = true;

			// Not how an effect properly should be set probably
			Model cubeModel = Content.Load<Model>("Models/cube");
			foreach (ModelMesh mesh in cubeModel.Meshes)
			{
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					part.Effect = effect;
				}
			}
			world.Models[playerEntity.Id] = cubeModel;
			for (int i = 0; i < otherCubes.Length; i++)
			{
				world.Models[otherCubes[i].Id] = cubeModel;	
			}

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


			//_renderSystem.updateTranslationTransforms(_world);
			renderSystem.DrawEntities(world);


			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


	}
}
