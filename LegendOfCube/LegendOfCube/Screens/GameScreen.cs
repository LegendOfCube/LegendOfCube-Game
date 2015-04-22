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
		private Vector2 fontPos;

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
				inputSystem.ApplyInput(gameTime, world);
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
				animationSystem.OnUpdate(world, delta);
				cameraSystem.OnUpdate(world, delta);
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
				text.AppendLine(UIFormat(1.0f/(float) gameTime.ElapsedGameTime.TotalSeconds));
				text.Append("CamPos: ");
				text.AppendLine(UIFormat(world.CameraPosition));
				text.Append("CamDir: ");
				text.AppendLine(UIFormat(Vector3.Normalize(world.Transforms[world.Player.Id].Translation - world.CameraPosition)));
				text.Append("CubePos: ");
				text.AppendLine(UIFormat(world.Transforms[world.Player.Id].Translation));
				text.Append("CubeVel: ");
				text.AppendLine(UIFormat(world.Velocities[world.Player.Id]));
				text.Append("CubeAcc: ");
				text.AppendLine(UIFormat(world.Accelerations[world.Player.Id]));
				text.Append("OnGround: ");
				text.AppendLine(world.PlayerCubeState.OnGround.ToString());
				text.Append("OnWall: ");
				text.AppendLine(world.PlayerCubeState.OnWall.ToString());

				spriteBatch.DrawString(font, text, fontPos, Color.DarkGreen);
			}

			//Gameover screen
			if (world.TimeSinceGameOver >= 1 && world.WinState)
			{
				spriteBatch.Draw(winScreen1, new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(font, world.GameStats.PlayerDeaths.ToString(), new Vector2(400, 260), Color.Red);
				spriteBatch.DrawString(font, UIFormat(world.GameStats.GameTime) + "s", new Vector2(300, 160), Color.Red);
			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			audioSystem = new AudioSystem(contentCollection);
			world = level.CreateWorld(Game, contentCollection);

			inputSystem = new InputSystem(Game, ScreenSystem);
			movementSystem = new MovementSystem();
			physicsSystem = new PhysicsSystem(world.MaxNumEntities);
			cameraSystem = new CameraSystem();
			aiSystem = new AISystem();
			animationSystem = new AnimationSystem();
			renderSystem = new RenderSystem(Game, graphicsManager);
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			winScreen1 = Game.Content.Load<Texture2D>("Menu/winnerScreen1");
			winScreen2 = Game.Content.Load<Texture2D>("Menu/winnerScreen2");
			font = Game.Content.Load<SpriteFont>("Arial");

			renderSystem.LoadContent();

			fontPos = new Vector2(0, 0);
		}

		private static string UIFormat(Vector3 value)
		{
			return String.Format("(X: {0}, Y: {1}, Z: {2})", UIFormat(value.X), UIFormat(value.Y), UIFormat(value.Z));
		}

		private static string UIFormat(float value)
		{
			return String.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}", value);
		}
	}
}
