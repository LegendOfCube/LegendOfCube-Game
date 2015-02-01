using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	class NewLegendOfCubeGame : Microsoft.Xna.Framework.Game
	{
		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private World _world;
		private InputSystem _inputSystem;
		private PhysicsSystem _physicsSystem;
		private RenderSystem _renderSystem;


		private Entity barrelEntity;
		private Model barrelModel;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public NewLegendOfCubeGame()
		{
			_world = new World(100);
			_renderSystem = new RenderSystem(this);

			Content.RootDirectory = "Content";

			// Temporary code to create a barrel entity that should render.
			ComponentMask barrelMask = new ComponentMask(ComponentMask.POSITION |
			                                             ComponentMask.TRANSFORM |
														 ComponentMask.MODEL);
			barrelEntity = _world.createEntity(barrelMask);
			_world.Positions[barrelEntity.ID] = new Vector3(0, 0, 0);
			_world.Transforms[barrelEntity.ID] = Matrix.CreateScale(0.1f);
		}

		// Overriden XNA methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/**
		 * Allows the game to perform any initialization it needs to before starting to run.
		 * This is where it can query for any required services and load any non-graphic
		 * related content.  Calling base.Initialize will enumerate through any components
		 * and initialize them as well.
		 */
		protected override void Initialize()
		{
			_renderSystem.Initialize();


			base.Initialize();
		}

		/**
		 * LoadContent will be called once per game and is the place to load
		 * all of your content.
		 */
		protected override void LoadContent()
		{
			barrelModel = Content.Load<Model>("barrel");
			_world.Models[barrelEntity.ID] = barrelModel;
		}

		/**
		 * UnloadContent will be called once per game and is the place to unload
		 * all content.
		 */
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/**
		 * Allows the game to run logic such as updating the world,
		 * checking for collisions, gathering input, and playing audio.
		 * @param gameTime Provides a snapshot of timing values.
		 */
		protected override void Update(GameTime gameTime)
		{


			base.Update(gameTime);
		}

		/**
		 * This is called when the game should draw itself.
		 * @param gameTime Provides a snapshot of timing values.
		 */
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			_renderSystem.updateTranslationTransforms(_world);
			_renderSystem.DrawEntities(_world);


			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


	}
}
