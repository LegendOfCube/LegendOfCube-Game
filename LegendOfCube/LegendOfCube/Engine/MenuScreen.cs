using System;
using System.Collections.Generic;
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

		public MenuScreen(Game game) : base(game)
		{
			World = new World(1002);
			inputSystem = new InputSystem(game);
			cameraSystem = new CameraSystem();
			menuInputSystem = new MenuInputSystem(game);
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, World, switcher);
			menuInputSystem.ApplyInput(gameTime, World, switcher);
			cameraSystem.OnUpdate(World, delta);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.DarkTurquoise);

			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			renderSystem.RenderWorld(World);

			spriteBatch.Begin();

			//Ugly box
			//spriteBatch.Draw(tex2D, new Rectangle(100, 20, 400, 200), Color.Black);

			spriteBatch.DrawString(font, "Play", new Vector2(100, 20), Color.Maroon, 0f, Vector2.Zero, new Vector2(10f), new SpriteEffects(), 1f);
			//spriteBatch.DrawString(font, "Options", new Vector2(100, 200), Color.Crimson, 0f, Vector2.Zero, new Vector2(10f), new SpriteEffects(), 1f);
			spriteBatch.DrawString(font, "Exit", new Vector2(100, 380), Color.Goldenrod, 0f, Vector2.Zero, new Vector2(10f), new SpriteEffects(), 1f);
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");

			//Custom texture forugly box
			tex2D = new Texture2D(Game.GraphicsDevice, 1, 1);
			tex2D.SetData(new Color[] { Color.Black});
		}
	}
}
