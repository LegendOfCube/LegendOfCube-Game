namespace LegendOfCube.Engine
{
	public class SwitcherSystem
	{
		private readonly LegendOfCubeGame game;

		public SwitcherSystem(LegendOfCubeGame game)
		{
			this.game = game;
		}

		public void Switch()
		{
			game.SwitchScreen();
		}
	}
}
