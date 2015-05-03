using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Screens
{
	public class StartScreen : BaseMenuScreen
	{
		internal StartScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void InitializeScreen()
		{
			AddTitle("Legend of Cube");
			AddSpace(35.0f);

			AddClickable("Start Game", () => { ScreenSystem.AddGameScreen(LevelConstants.LEVEL_1); return "Start Game"; });
			AddClickable("Select Level", () => { ScreenSystem.AddScreen(new LevelSelectScreen(Game, ScreenSystem)); return "Select Level"; });
			AddClickable("Options", () => { ScreenSystem.AddScreen(new OptionsScreen(Game, ScreenSystem)); return "Options"; });
			AddSpace(20.0f);

			AddClickable("Exit Game", () => { this.OnExit(); return "null"; });
		}

		internal override void OnExit()
		{
			Game.Exit();
		}
	}
}
