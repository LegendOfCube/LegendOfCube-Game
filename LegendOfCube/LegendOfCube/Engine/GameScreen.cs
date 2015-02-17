using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class GameScreen : Screen
	{
		private readonly InputSystem inputSystem;
		private readonly GameplaySystem gameplaySystem;
		private readonly PhysicsSystem physicsSystem;

		public GameScreen(Game game) : base(game)
		{
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem();
		}

		protected override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		protected override void Draw(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}
}
