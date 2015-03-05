using System.Collections.Generic;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{

	public class LegendOfCubeGame : Game
	{

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly RenderSystem renderSystem;
		private readonly GraphicsDeviceManager graphicsManager;
		private GameObjectTemplates gameObjectTemplates;
		private readonly List<Screen> screens;
		private Screen currentScreen;

		public SwitcherSystem SwitcherSystem;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			Content.RootDirectory = "Content";

			gameObjectTemplates = new GameObjectTemplates();

			graphicsManager = new GraphicsDeviceManager(this);
			renderSystem = new RenderSystem(this, graphicsManager);

			// XNA initiation moved out of RenderSystem since it's more of a "WorldRenderer"
			// that could be disposed and reused
			Window.AllowUserResizing = true;
			graphicsManager.PreferMultiSampling = true;
			graphicsManager.ApplyChanges();

			screens = new List<Screen> { new GameScreen(this, gameObjectTemplates), new MenuScreen(this) };
			currentScreen = screens[0];
			SwitcherSystem = new SwitcherSystem(this);

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
			gameObjectTemplates.LoadContent(Content);
			renderSystem.LoadContent();

			foreach (var screen in screens)
			{
				screen.LoadContent();
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
			currentScreen.Update(gameTime, SwitcherSystem);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			currentScreen.Draw(gameTime, renderSystem);
			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void SwitchScreen()
		{
			if (currentScreen is GameScreen)
			{
				this.IsMouseVisible = true;
				screens[1].SetWorld(currentScreen.World);
				currentScreen = screens[1];
			}
			else if (currentScreen is MenuScreen)
			{

				this.IsMouseVisible = false;
				currentScreen = screens[0];
			}
		}

		public void ResetPlayer()
		{
			
		}
	}
}
