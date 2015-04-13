using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	class PauseScreen : MenuScreen
	{
		public PauseScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) {}

		internal override void LoadContent()
		{
			base.LoadContent();
			AddItemBelow("Return to Game", () =>
				ScreenSystem.RemoveCurrentScreen()
			);
			AddItemBelow("Main Menu", () =>
			{
				ScreenSystem.RemoveCurrentScreen();
				ScreenSystem.RemoveCurrentScreen();
			});
			AddItemBelow("Exit Game", () =>
				Game.Exit()
			);
		}
	}
}
