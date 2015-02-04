using System;
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


		public Entity barrelEntity;
		private Entity[] barrels;

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

			// Temporary code to create a barrel entity that should render.
			barrelEntity = CreateEntity(new Properties(Properties.TRANSFORM | Properties.MODEL | Properties.INPUT_FLAG | Properties.VELOCITY | Properties.GRAVITY_FLAG));
			barrels = new Entity[50];
			Random rnd = new Random();
			for (int i = 0; i < barrels.Length; i++)
			{
				barrels[i] = CreateEntity(new Properties(Properties.TRANSFORM | Properties.MODEL));
				Matrix test = Matrix.CreateTranslation(new Vector3(rnd.Next(-100, 100), 0, rnd.Next(-100, 100))) * Matrix.CreateScale(0.1f);
				world.Transforms[barrels[i].Id] = test;
			}

			world.Transforms[barrelEntity.Id] = Matrix.CreateScale(0.1f);
			world.Velocities[barrelEntity.Id] = new Vector3(0, 0, 0);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			world.Models[barrelEntity.Id] = Content.Load<Model>("barrel");
			for (int i = 0; i < barrels.Length; i++)
			{
				world.Models[barrels[i].Id] = Content.Load<Model>("barrel");	
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
			inputSystem.ApplyInput(gameTime, world);

			physicsSystem.ApplyPhysics(gameTime, world);

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
