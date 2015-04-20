using System.Collections.Generic;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using LegendOfCube.Screens;

namespace LegendOfCube.Engine
{

	public class LegendOfCubeGame : Game
	{

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly GraphicsDeviceManager graphicsManager;
		private readonly ScreenSystem screenSystem;
		private readonly ContentCollection contentCollection;

		private readonly GlobalConfig cfg = GlobalConfig.Instance;
	
		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			Content.RootDirectory = "Content";

			contentCollection = new ContentCollection();

			graphicsManager = new GraphicsDeviceManager(this);
			graphicsManager.PreferredBackBufferWidth = cfg.InternalResX;
			graphicsManager.PreferredBackBufferHeight = cfg.InternalResY;
			graphicsManager.IsFullScreen = cfg.Fullscreen;
			graphicsManager.SynchronizeWithVerticalRetrace = cfg.VSync;
			graphicsManager.PreferMultiSampling = cfg.MultiSampling;

			// XNA initiation moved out of RenderSystem since it's more of a "WorldRenderer"
			// that could be disposed and reused
			Window.AllowUserResizing = true;

			graphicsManager.ApplyChanges();

			screenSystem = new ScreenSystem(this, contentCollection, graphicsManager);

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
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			contentCollection.LoadContent(Content);
			screenSystem.LoadContent();
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
			screenSystem.Update(gameTime);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			screenSystem.Draw(gameTime);
			base.Draw(gameTime);
		}

	}
}
