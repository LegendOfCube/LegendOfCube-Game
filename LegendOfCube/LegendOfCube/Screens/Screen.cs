using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	public abstract class Screen
	{
		protected Game Game;
		public World World;

		protected Screen()
		{

		}
		protected Screen(Game game)
		{
			this.Game = game;
		}

		public void SetWorld(World world)
		{
			World = world;
		}

		protected internal abstract void Update(GameTime gameTime, SwitcherSystem switcher);
		protected internal abstract void Draw(GameTime gameTime, RenderSystem renderSystem);
		internal abstract void LoadContent();
	}
}
