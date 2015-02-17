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
			World = new World(1002);
			this.inputSystem = new InputSystem(game);
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, World, switcher);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.Tomato);

			renderSystem.RenderWorld(World);
		}

		internal override void LoadContent()
		{
			throw new NotImplementedException();
		}
	}
}
