using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	public class PauseScreen : BaseMenuScreen
	{
		public PauseScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void InitializeScreen()
		{
			AddTitle("Paused");
			AddSpace(35.0f);

			AddClickable("Resume", () => { this.OnExit(); ScreenSystem.RemoveCurrentScreen(); return "Return to game"; });
			AddClickable("Options", () => { ScreenSystem.AddScreen(new OptionsScreen(Game, ScreenSystem)); return "Options"; });
			AddSpace(20.0f);
	
			AddClickable("Main Menu", () => { ScreenSystem.SetScreen(new MainMenuScreen(Game, ScreenSystem)); return "Main Menu"; });
			AddClickable("Exit Game", () => { this.OnExit(); Game.Exit(); return "Exit Game"; });
		}

		internal override void OnExit()
		{
			// Do nothing.
		}
	}
}
