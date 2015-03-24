using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;

namespace LegendOfCube.Screens
{
	class PauseScreen : MenuScreen
	{
		private readonly InputSystem inputSystem;
		private readonly MenuInputSystem menuInputSystem;
		private readonly CameraSystem cameraSystem;
		private readonly ScreenSystem screenSystem;
		private readonly Game game;
		private SpriteBatch spriteBatch;
		private SpriteFont font;

		private Texture2D play;
		private Texture2D exit;
		private Texture2D levelSelect;
		private Texture2D select;
		private int selection;

		public PauseScreen(Game game, ScreenSystem screenSystem) : base(game)
		{
			World = screenSystem.GetWorld();
			this.game = game;
			this.screenSystem = screenSystem;
			inputSystem = new InputSystem(game);
			cameraSystem = new CameraSystem();
			menuInputSystem = new MenuInputSystem(game);
			selection = 0;
		}

		protected internal override void Update(GameTime gameTime, ScreenSystem switcher)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, World, switcher);
			menuInputSystem.ApplyInput(gameTime, World, switcher, this, ref selection);
			cameraSystem.OnUpdate(World, delta);
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.DarkTurquoise);

			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			renderSystem.RenderWorld(World);

			spriteBatch.Begin();
			spriteBatch.Draw(play, new Vector2(100, 20), Color.Red);
			spriteBatch.Draw(levelSelect, new Vector2(100, 110), Color.Red);
			spriteBatch.Draw(exit, new Vector2(100, 200), Color.Red);

			switch (selection)
			{
				case 0:
					spriteBatch.Draw(select, new Vector2(35, 10), Color.Red);
					break;
				case 1:
					spriteBatch.Draw(select, new Vector2(35, 100), Color.Red);
					break;
				case 2:
					spriteBatch.Draw(select, new Vector2(35, 190), Color.Red);
					break;

			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");

			play = Game.Content.Load<Texture2D>("Menu/play");
			exit = Game.Content.Load<Texture2D>("Menu/exit");
			levelSelect = Game.Content.Load<Texture2D>("Menu/level select");
			select = Game.Content.Load<Texture2D>("Menu/selector");
		}

		internal override void PerformSelection()
		{
			switch (selection)
			{
				case 0:
					screenSystem.SwitchScreen(Screens.ScreenTypes.GAME);
					break;
				case 1:
					screenSystem.SwitchScreen(Screens.ScreenTypes.LEVEL_SELECT);
					break;
				case 2:
					screenSystem.SwitchScreen(Screens.ScreenTypes.START);
					break;
			}
		}
	}
}
