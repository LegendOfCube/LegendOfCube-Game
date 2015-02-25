using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	class MenuScreen : Screen
	{
		private readonly InputSystem inputSystem;
		private readonly CameraSystem cameraSystem;

		public MenuScreen(Game game) : base(game)
		{
			World = new World(1002);
			this.inputSystem = new InputSystem(game);
			cameraSystem = new CameraSystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, World, switcher);
			cameraSystem.OnUpdate(World, delta);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.Tomato);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);
		}

		internal override void LoadContent()
		{
			throw new NotImplementedException();
		}
	}
}
