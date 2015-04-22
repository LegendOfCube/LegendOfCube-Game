using System;
using System.Linq;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Screens
{
	class LevelSelectScreen : MenuScreen
	{
		public LevelSelectScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void LoadContent()
		{
			base.LoadContent();

			// Add entry for each level
			// Not using foreach due to warning about using a foreach variable in closure
			for (int i = 0; i < LevelConstants.LEVELS.Count(); i++)
			{
				Level level = LevelConstants.LEVELS[i];
				AddItemBelow(level.Name, () =>
					ScreenSystem.AddGameScreen(level)
				);
			}

			AddItemBelow("Back", () => 
				ScreenSystem.RemoveCurrentScreen()
			);
		}
	}
}
