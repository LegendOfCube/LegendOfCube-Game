using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	class GameScreen : Screen
	{
		public readonly Level Level;
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

		private SpriteFont font;
		private SpriteBatch spriteBatch;

		internal GameScreen(Level level, Game game, ScreenSystem screenSystem, ContentCollection contentCollection, GraphicsDeviceManager graphicsManager)
			: base(game, screenSystem, true)
		{
			this.Level = level;
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
					Highscore.Instance.AddHighScore(Level.Name, world.GameStats.GameTime);
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
				spriteBatch.DrawString(font,
					world.GameStats.GameTime <= Highscore.Instance.GetHighScoresForLevel(Level.Name)[0]
						? "NEEEEEEEEEEEEW HIGHSCORE!"
						: "You win, absolutely cubical!", new Vector2(300, 150), Color.White);
				spriteBatch.DrawString(font, "Time: " + UiUtils.UIFormat(world.GameStats.GameTime) + "s", new Vector2(300, 250), Color.White);
				spriteBatch.DrawString(font, world.GameStats.GameTime <= Highscore.Instance.GetHighScoresForLevel(Level.Name)[0]
						? "Old Highscore: " + UiUtils.UIFormat(Highscore.Instance.GetHighScoresForLevel(Level.Name)[1])
						: "Highscore: " + UiUtils.UIFormat(Highscore.Instance.GetHighScoresForLevel(Level.Name)[0]), new Vector2(300, 300), Color.White);
				spriteBatch.DrawString(font, "Press 'r' to restart or 'esc' to go to menu.", new Vector2(300, 400), Color.White);

			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			world = Level.CreateWorld(Game, contentCollection);

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

			font = Game.Content.Load<SpriteFont>("Arial");

			cameraSystem.OnStart(world);
			audioSystem.OnStart(world);
		}
	}
}
