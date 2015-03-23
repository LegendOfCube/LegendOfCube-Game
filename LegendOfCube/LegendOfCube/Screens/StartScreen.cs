using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Screens
{
	class StartScreen : Screen
	{
		private readonly MenuInputSystem menuInputSystem;

		private MenuItem[] items = new MenuItem[]{new MenuItem("Start Game"), new MenuItem("Change Level"), new MenuItem("Exit")};

		private SpriteBatch spriteBatch;
		private SpriteFont font;

		private int selection;
		

		public StartScreen(Game game)
		{
			menuInputSystem = new MenuInputSystem(game);
			this.Game = game;

			selection = 0;
			items[0].Selected = true;
			items[0].Position = new Vector2(25, 25);

			items[1] = new MenuItem("Change Level");
			items[1].Position = new Vector2(25, 40);

			items[2] = new MenuItem("Exit");
			items[2].Position = new Vector2(25, 55);

		}
		protected internal override void Update(Microsoft.Xna.Framework.GameTime gameTime, Screens.ScreenSystem switcher)
		{
			menuInputSystem.ApplyInput(gameTime, World, switcher, ref selection);
			foreach(var item in items) 
			{
				item.Selected = false;
			}
			items[selection].Selected = true;
		}

		protected internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Engine.Graphics.RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			spriteBatch.Begin();
			items[0].Draw(spriteBatch, font);
			items[1].Draw(spriteBatch, font);
			items[2].Draw(spriteBatch, font);
			spriteBatch.End();	
		}	

		internal override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");
		}
	}
}
