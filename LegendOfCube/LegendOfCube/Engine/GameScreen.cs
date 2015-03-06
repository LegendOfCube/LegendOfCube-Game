using System;
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
		private readonly EventSystem EventSystem;
		private readonly AISystem AI_system;
		private readonly GameObjectTemplateCollection gameObjectTemplates;

		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Vector2 fontPos;

		public GameScreen(Game game, GameObjectTemplateCollection gameObjectTemplates) : base(game)
		{
			this.gameObjectTemplates = gameObjectTemplates;

			World = new World(3002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem(World.MaxNumEntities);
			cameraSystem = new CameraSystem();
			EventSystem = new EventSystem();
			AI_system = new AISystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			inputSystem.ApplyInput(gameTime, World, switcher);
			AI_system.Update(World, delta);
			gameplaySystem.ProcessInputData(World, delta);
			physicsSystem.ApplyPhysics(delta, World); // Note, delta should be fixed time step.
			cameraSystem.OnUpdate(World, delta);
			EventSystem.HandleEvents(World);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);

			StringBuilder text = new StringBuilder();
			text.Append("FPS: ");
			text.AppendLine(UIFormat(1.0f/(float)gameTime.ElapsedGameTime.TotalSeconds));
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

			spriteBatch.Begin();
			spriteBatch.DrawString(font, text, fontPos, Color.DarkGreen);
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			//ConceptLevel.CreateLevel(World, Game);
			//TestLevel1.CreateLevel(World, Game);
			World = new BeanStalkLevelFactory().CreateWorld(Game, gameObjectTemplates);

			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
