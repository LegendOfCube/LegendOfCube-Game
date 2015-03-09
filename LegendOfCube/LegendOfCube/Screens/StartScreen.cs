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

		private MenuItem startGame, exitGame, changeLevel;
		

		public StartScreen()
		{
			startGame = new MenuItem("Start game");
			startGame.position = new Vector2(25, 25);
			changeLevel = new MenuItem("Change Level");
			changeLevel.position = new Vector2(25, 40);
			exitGame = new MenuItem("Exit");
			exitGame.position = new Vector2(25, 55);

		}
		protected internal override void Update(Microsoft.Xna.Framework.GameTime gameTime, Screens.ScreenSystem switcher)
		{
			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
			{
				switcher.SwitchScreen(ScreenTypes.GAME);
			}
		}

		protected internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Engine.Graphics.RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);

			ScreenSystem.spriteBatch.Begin();
			startGame.draw();
			changeLevel.draw();	
			exitGame.draw();
			ScreenSystem.spriteBatch.End();	
		}	

		internal override void LoadContent()
		{
			throw new NotImplementedException();
		}
	}
}
