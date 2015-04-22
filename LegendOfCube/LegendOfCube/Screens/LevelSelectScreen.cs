using System;
using System.Linq;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;
using LegendOfCube.Levels;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Screens
{
	public class LevelSelectScreen : BaseMenuScreen
	{
		public LevelSelectScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void InitializeScreen()
		{
			AddTitle("Select Level");
			AddSpace(35.0f);

			// Add entry for each level
			// Not using foreach due to warning about using a foreach variable in closure
			for (int i = 0; i < LevelConstants.LEVELS.Count(); i++)
			{
				Level level = LevelConstants.LEVELS[i];
				AddClickable(level.Name, () => { ScreenSystem.AddGameScreen(level); return level.Name; });
			}
			AddSpace(35.0f);

			AddClickable("Main Menu", () => { this.OnExit(); ScreenSystem.RemoveCurrentScreen(); return "null"; });
		}

		internal override void OnExit()
		{
			// Do nothing.
		}
	}
}
