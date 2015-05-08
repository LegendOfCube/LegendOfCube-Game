using Microsoft.Xna.Framework;

namespace LegendOfCube.Screens
{
	public abstract class Screen
	{
		protected Game Game { get; private set; }
		protected ScreenSystem ScreenSystem { get; private set; }
		public bool RenderBehind { get; protected set; }
		public bool UpdateBehind { get; protected set; }

		internal Screen(Game game, ScreenSystem screenSystem, bool renderBehind, bool updateBehind)
		{
			Game = game;
			ScreenSystem = screenSystem;
			RenderBehind = renderBehind;
			UpdateBehind = updateBehind;
		}

		internal abstract void Update(GameTime gameTime, bool isBackground);
		internal abstract void Draw(GameTime gameTime, bool isBackground);

		internal abstract void LoadContent();
	}

}
