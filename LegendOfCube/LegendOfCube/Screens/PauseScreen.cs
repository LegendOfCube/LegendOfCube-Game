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
			AddTitle("Pause");
			AddSpace(35.0f);

			AddClickable("Return to game", () => { this.OnExit(); ScreenSystem.RemoveCurrentScreen(); return "Return to game"; });
			AddClickable("Main Menu", () => { this.OnExit(); ScreenSystem.RemoveCurrentScreen(); ScreenSystem.RemoveCurrentScreen(); return "Main Menu"; });
			AddClickable("Exit Game", () => { this.OnExit(); Game.Exit(); return "Exit Game"; });
		}

		internal override void OnExit()
		{
			// Do nothing.
		}
	}
}
