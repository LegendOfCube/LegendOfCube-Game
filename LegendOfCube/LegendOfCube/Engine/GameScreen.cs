using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Engine.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Levels;

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

		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Vector2 fontPos;

		public GameScreen(Game game) : base(game)
		{
			World = new World(3002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem();
			cameraSystem = new CameraSystem();
			EventSystem = new EventSystem();
			AI_system = new AISystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			inputSystem.ApplyInput(gameTime, World, switcher);
			AI_system.update(World, delta);
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

			spriteBatch.Begin();
			string output = "CamPos: " + World.CameraPosition + "\nCamDir: " + (World.Transforms[World.Player.Id].Translation - World.CameraPosition) + "\nCubePos: " + World.Transforms[World.Player.Id].Translation;
			spriteBatch.DrawString(font, output, fontPos, Color.BlueViolet);
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			//ConceptLevel.CreateLevel(World, Game);
			TestLevel1.CreateLevel(World, Game);


			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");
			fontPos = new Vector2(0, 0);
		}
	}
}
