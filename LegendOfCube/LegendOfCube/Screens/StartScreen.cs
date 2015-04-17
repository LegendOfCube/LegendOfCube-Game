﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Screens
{
	class StartScreen : MenuScreen
	{
		internal StartScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void LoadContent()
		{
			base.LoadContent();
			AddItemBelow("Start Game", () =>
				ScreenSystem.AddGameScreen(LevelConstants.LEVEL_1)
			);

			AddItemBelow("Select Level", () => 
				ScreenSystem.AddScreen(new LevelSelectScreen(Game, ScreenSystem))
			);

			AddItemBelow("Options", () =>
				{ }
			);

			AddItemBelow("Exit",
				() => Game.Exit()
			);
		}
	}
}
