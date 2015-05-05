using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	class GameScreen : Screen
	{
		public readonly Level level;
		private World world;

		private InputSystem inputSystem;
		private MovementSystem movementSystem;
		private PhysicsSystem physicsSystem;
		private CameraSystem cameraSystem;
		private AISystem aiSystem;
		private AnimationSystem animationSystem;
		private RenderSystem renderSystem;
		private AudioSystem audioSystem;

		private readonly GraphicsDeviceManager graphicsManager;
		private readonly ContentCollection contentCollection;
		private readonly InputHelper inputHelper;

		private Texture2D winScreen1;
		private Texture2D winScreen2;
		private SpriteFont font;
		private SpriteBatch spriteBatch;

		internal GameScreen(Level level, Game game, ScreenSystem screenSystem, ContentCollection contentCollection, GraphicsDeviceManager graphicsManager)
			: base(game, screenSystem, true)
		{
			this.level = level;
			this.contentCollection = contentCollection;
			this.graphicsManager = graphicsManager;
		}

		internal override void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			//GameOver update
			if (!world.WinState)
			{
				world.GameStats.GameTime += delta;
			}
			if (world.WinState)
			{
				if (world.TimeSinceGameOver == 0)
				{
					Highscore.Instance.AddHighScore(level.Name, world.GameStats.GameTime);
				}
				inputSystem.ApplyInput(gameTime, world);
				animationSystem.Update(world, delta);
				physicsSystem.ApplyPhysics(world, delta);

				//Small delay before score screen.
				if (world.TimeSinceGameOver < 1)
				{
					world.TimeSinceGameOver += delta;
				}
			}
			//Normal update
			else
			{
				inputSystem.ApplyInput(gameTime, world);
				aiSystem.Update(world, delta);
				movementSystem.ProcessInputData(world, delta);
				physicsSystem.ApplyPhysics(world, delta); // Note, delta should be fixed time step.
				EventSystem.CalculateCubeState(world, physicsSystem);
				EventSystem.HandleEvents(world);
				audioSystem.Update(world);
				animationSystem.Update(world, delta);
				cameraSystem.Update(world, gameTime, delta);
			}
		}

		internal override void Draw(GameTime gameTime)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(world);

			spriteBatch.Begin();
			if (world.DebugState.ShowDebugOverlay)
			{
				StringBuilder text = new StringBuilder();
				text.Append("FPS: ");
				text.AppendLine(UiUtils.UIFormat(1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds));
				text.Append("CamPos: ");
				text.AppendLine(UiUtils.UIFormat(world.Camera.Position));
				text.Append("CamDir: ");
				text.AppendLine(UiUtils.UIFormat(Vector3.Normalize(world.Camera.Target - world.Camera.Position)));
				text.Append("CubePos: ");
				text.AppendLine(UiUtils.UIFormat(world.Transforms[world.Player.Id].Translation));
				text.Append("CubeVel: ");
				text.AppendLine(UiUtils.UIFormat(world.Velocities[world.Player.Id]));
				text.Append("CubeAcc: ");
				text.AppendLine(UiUtils.UIFormat(world.Accelerations[world.Player.Id]));
				text.Append("OnGround: ");
				text.AppendLine(world.PlayerCubeState.OnGround.ToString());
				text.Append("OnWall: ");
				text.AppendLine(world.PlayerCubeState.OnWall.ToString());

				spriteBatch.DrawString(font, text, Vector2.One, Color.Black, 0, Vector2.Zero, 22.0f / font.LineSpacing, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, text, Vector2.Zero, Color.White, 0, Vector2.Zero, 22.0f / font.LineSpacing, SpriteEffects.None, 0);
			}

			//Gameover screen
			if (world.TimeSinceGameOver >= 1 && world.WinState)
			{
			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			world = level.CreateWorld(Game, contentCollection);

			inputSystem = new InputSystem(Game, ScreenSystem);
			movementSystem = new MovementSystem();
			audioSystem = new AudioSystem(contentCollection);
			physicsSystem = new PhysicsSystem(world.MaxNumEntities);
			cameraSystem = new CameraSystem();
			aiSystem = new AISystem();
			animationSystem = new AnimationSystem();
			renderSystem = new RenderSystem(Game, graphicsManager);
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);

			renderSystem.LoadContent();

			winScreen1 = Game.Content.Load<Texture2D>("Menu/winnerScreen1");
			winScreen2 = Game.Content.Load<Texture2D>("Menu/winnerScreen2");
			font = Game.Content.Load<SpriteFont>("Arial");

			cameraSystem.OnStart(world);
			audioSystem.OnStart(world);
		}
	}
}
