using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	public class ScreenSystem
	{
		public static SpriteFont font;
		public static SpriteBatch spriteBatch;

		private readonly LegendOfCubeGame game;

		public ScreenSystem(LegendOfCubeGame game)
		{
			this.game = game;
		}


		public void Switch()
		{
			game.SwitchScreen();
		}
	}
}
