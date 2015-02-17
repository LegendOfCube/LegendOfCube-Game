using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class MenuScreen : Screen
	{
		private readonly InputSystem inputSystem;

		public MenuScreen(Game game) : base(game)
		{
			this.inputSystem = new InputSystem(game);
		}

		protected internal override void Update(GameTime gameTime, World world, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, world, switcher);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem, World world)
		{
			Game.GraphicsDevice.Clear(Color.Tomato);

			renderSystem.RenderWorld(world);
		}
	}
}
