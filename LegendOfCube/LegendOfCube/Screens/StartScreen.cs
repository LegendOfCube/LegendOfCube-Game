using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	class StartScreen : Screen
	{
		public MenuItem startGame, exitGame, changeLevel;

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
			throw new NotImplementedException();
		}

		protected internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Engine.Graphics.RenderSystem renderSystem)
		{
			startGame.draw();
			changeLevel.draw();
			exitGame.draw();

		}

		internal override void LoadContent()
		{
			throw new NotImplementedException();
		}
	}
}
