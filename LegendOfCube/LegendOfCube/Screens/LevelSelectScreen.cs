using System.Linq;
using LegendOfCube.Engine;
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
				string highscore = "N/A";
				var highscores = Highscore.Instance.GetHighScoresForLevel(level.Name);
				if (highscores != null && highscores.Count > 0)
				{
					highscore = UiUtils.UIFormat(highscores[0]);
				}
				string name = level.Name + " \nHighScore: " + highscore + "s\n";
				AddClickable(name, () => { ScreenSystem.AddGameScreen(level); return name; });
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
