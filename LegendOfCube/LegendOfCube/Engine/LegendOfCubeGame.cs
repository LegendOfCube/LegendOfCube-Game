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

		private readonly RenderSystem renderSystem;
		private readonly GraphicsDeviceManager graphicsManager;
		private readonly ScreenSystem screenSystem;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			Content.RootDirectory = "Content";

			graphicsManager = new GraphicsDeviceManager(this);
			renderSystem = new RenderSystem(this, graphicsManager);

			// XNA initiation moved out of RenderSystem since it's more of a "WorldRenderer"
			// that could be disposed and reused
			Window.AllowUserResizing = true;
			graphicsManager.PreferMultiSampling = true;
			graphicsManager.ApplyChanges();

			screenSystem = new ScreenSystem(this);
			screenSystem.AddScreen(new StartScreen(this), ScreenTypes.START);
			screenSystem.AddScreen(new GameScreen(this), ScreenTypes.GAME);
			//screenSystem.AddScreen(new LevelSelectScreen(this), ScreenTypes.LEVEL_SELECT);
			screenSystem.AddScreen(new PauseScreen(this, screenSystem), ScreenTypes.PAUSE);


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
			renderSystem.LoadContent();
			screenSystem.LoadAllContent();
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
			screenSystem.GetCurrentScreen().Update(gameTime, screenSystem);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			screenSystem.GetCurrentScreen().Draw(gameTime, renderSystem);
			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/*public void SwitchScreen()
		{
			if (currentScreen is GameScreen)
			{
				this.IsMouseVisible = true;
				screens[1].SetWorld(currentScreen.World);
				currentScreen = screens[1];
			}
			else if (currentScreen is PauseScreen)
			{

				this.IsMouseVisible = false;
				currentScreen = screens[0];
			}
		}*/

		public void ResetPlayer()
		{
			
		}
	}
}
