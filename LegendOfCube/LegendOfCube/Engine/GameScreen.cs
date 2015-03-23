using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	class GameScreen : Screen
	{

		private readonly InputSystem inputSystem;
		private readonly GameplaySystem gameplaySystem;
		private readonly PhysicsSystem physicsSystem;
		private readonly CameraSystem cameraSystem;
		private readonly AISystem AI_system;
		private readonly ContentCollection ContentCollection;

		private Texture2D winScreen1;
		private Texture2D winScreen2;
		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Vector2 fontPos;

		public GameScreen(Game game, ContentCollection contentCollection) : base(game)
		{
			this.ContentCollection = contentCollection;

			World = new World(3002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem(World.MaxNumEntities);
			cameraSystem = new CameraSystem();
			AI_system = new AISystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!World.WinState)
			{
				World.GameTime += delta;
			}
			if (World.WinState)
			{
				inputSystem.ApplyInput(gameTime, World, switcher);
				cameraSystem.OnUpdate(World, delta);

				if (World.TimeSinceGameOver < 1)
				{
					World.TimeSinceGameOver += delta;
				}
			}
			else
			{
				inputSystem.ApplyInput(gameTime, World, switcher);
				AI_system.Update(World, delta);
				gameplaySystem.ProcessInputData(World, delta);
				physicsSystem.ApplyPhysics(delta, World); // Note, delta should be fixed time step.
				EventSystem.CalculateCubeState(World);
				EventSystem.HandleEvents(World);
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
			if (World.TimeSinceGameOver > 1 && World.WinState)
			{
				spriteBatch.Draw(winScreen1, new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(font, World.PlayerDeaths.ToString(), new Vector2(400, 260), Color.Red);
				spriteBatch.DrawString(font, UIFormat(World.GameTime) + "s", new Vector2(300, 160), Color.Red);
			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			//World = new ConceptLevel().CreateWorld(Game, contentCollection);
			//World = new TestLevel1().CreateWorld(Game, contentCollection);
			World = new DemoLevel().CreateWorld(Game, ContentCollection);
			//World = new BeanStalkLevelFactory().CreateWorld(Game, contentCollection);
			//World = new WallClimbLevelFactory().CreateWorld(Game, contentCollection);

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
