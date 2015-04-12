using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	class GameScreen : Screen
	{
		private InputSystem inputSystem;
		private MovementSystem movementSystem;
		private PhysicsSystem physicsSystem;
		private CameraSystem cameraSystem;
		private AISystem aiSystem;
		private AnimationSystem animationSystem;
		private ContentCollection contentCollection;

		private Texture2D winScreen1;
		private Texture2D winScreen2;
		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Vector2 fontPos;

		public GameScreen(Game game, ContentCollection contentCollection) : base(game)
		{
			this.contentCollection = contentCollection;
		}

		protected internal override void Update(GameTime gameTime, ScreenSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			//GameOver update
			if (!World.WinState)
			{
				World.GameStats.GameTime += delta;
			}
			if (World.WinState)
			{
				inputSystem.ApplyInput(gameTime, World, switcher);
				physicsSystem.ApplyPhysics(World, delta);

				//Small delay before score screen.
				if (World.TimeSinceGameOver < 1)
				{
					World.TimeSinceGameOver += delta;
				}
			}
			//Normal update
			else
			{
				inputSystem.ApplyInput(gameTime, World, switcher);
				aiSystem.Update(World, delta);
				movementSystem.ProcessInputData(World, delta);
				physicsSystem.ApplyPhysics(World, delta); // Note, delta should be fixed time step.
				EventSystem.CalculateCubeState(World);
				EventSystem.HandleEvents(World);
				animationSystem.OnUpdate(World, delta);
				cameraSystem.OnUpdate(World, delta);
			}
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);

			spriteBatch.Begin();
			if (World.DebugState.ShowDebugOverlay)
			{
				StringBuilder text = new StringBuilder();
				text.Append("FPS: ");
				text.AppendLine(UIFormat(1.0f/(float) gameTime.ElapsedGameTime.TotalSeconds));
				text.Append("CamPos: ");
				text.AppendLine(UIFormat(World.CameraPosition));
				text.Append("CamDir: ");
				text.AppendLine(UIFormat(Vector3.Normalize(World.Transforms[World.Player.Id].Translation - World.CameraPosition)));
				text.Append("CubePos: ");
				text.AppendLine(UIFormat(World.Transforms[World.Player.Id].Translation));
				text.Append("CubeVel: ");
				text.AppendLine(UIFormat(World.Velocities[World.Player.Id]));
				text.Append("CubeAcc: ");
				text.AppendLine(UIFormat(World.Accelerations[World.Player.Id]));
				text.Append("OnGround: ");
				text.AppendLine(World.PlayerCubeState.OnGround.ToString());
				text.Append("OnWall: ");
				text.AppendLine(World.PlayerCubeState.OnWall.ToString());

				spriteBatch.DrawString(font, text, fontPos, Color.DarkGreen);
			}

			//Gameover screen
			if (World.TimeSinceGameOver >= 1 && World.WinState)
			{
				spriteBatch.Draw(winScreen1, new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(font, World.GameStats.PlayerDeaths.ToString(), new Vector2(400, 260), Color.Red);
				spriteBatch.DrawString(font, UIFormat(World.GameStats.GameTime) + "s", new Vector2(300, 160), Color.Red);
			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{

			World = new ConceptLevel().CreateWorld(Game, contentCollection);
			//World = new TestLevel1().CreateWorld(Game, contentCollection);
			//World = new DemoLevel().CreateWorld(Game, contentCollection);
			//World = new BeanStalkLevelFactory().CreateWorld(Game, contentCollection);
			//World = new WallClimbLevelFactory().CreateWorld(Game, contentCollection);

			inputSystem = new InputSystem(Game);
			movementSystem = new MovementSystem();
			physicsSystem = new PhysicsSystem(World.MaxNumEntities);
			cameraSystem = new CameraSystem();
			aiSystem = new AISystem();
			animationSystem = new AnimationSystem();

			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			winScreen1 = Game.Content.Load<Texture2D>("Menu/winnerScreen1");
			winScreen2 = Game.Content.Load<Texture2D>("Menu/winnerScreen2");
			font = Game.Content.Load<SpriteFont>("Arial");
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
