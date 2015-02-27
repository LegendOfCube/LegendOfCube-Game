using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	class MenuScreen : Screen
	{
		private readonly InputSystem inputSystem;
		private readonly MenuInputSystem menuInputSystem;
		private readonly CameraSystem cameraSystem;
		private SpriteBatch spriteBatch;
		private SpriteFont font;
		private Texture2D tex2D;

		private Texture2D play;
		private Texture2D exit;
		private Texture2D select;
		private int selection;

		public MenuScreen(Game game) : base(game)
		{
			World = new World(1002);
			inputSystem = new InputSystem(game);
			cameraSystem = new CameraSystem();
			menuInputSystem = new MenuInputSystem(game);
			selection = 0;
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, World, switcher);
			menuInputSystem.ApplyInput(gameTime, World, switcher, ref selection);
			cameraSystem.OnUpdate(World, delta);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.DarkTurquoise);

			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			renderSystem.RenderWorld(World);

			spriteBatch.Begin();
			spriteBatch.Draw(play, new Vector2(100, 20), Color.Red);
			spriteBatch.Draw(exit, new Vector2(100, 200), Color.Red);

			spriteBatch.Draw(@select, selection == 0 ? new Vector2(35, 10) : new Vector2(35, 190), Color.Red);
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");

			play = Game.Content.Load<Texture2D>("Menu/play");
			exit = Game.Content.Load<Texture2D>("Menu/exit");
			select = Game.Content.Load<Texture2D>("Menu/selector");
		}
	}
}
