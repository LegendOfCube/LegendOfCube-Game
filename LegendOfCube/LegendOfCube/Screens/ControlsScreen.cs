using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	class ControlsScreen : BaseMenuScreen
	{

		private const string KEYBOARD_CONTROL_TEXT =
@"WASD: Move Cube
Space: Jump
Arrow Keys: Camera
R: Respawn
Backspace: Restart Level
Escape/Tab: Pause";

		private const float IMAGE_HEIGHT = 250;

		private Texture2D controllerImage;

		public ControlsScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) {}

		internal override void InitializeScreen()
		{
			controllerImage = Game.Content.Load<Texture2D>("LoCControlScheme");

			AddHeading("Keyboard");

			AddDescription(KEYBOARD_CONTROL_TEXT);
			AddSpace(20.0f);

			AddHeading("Control Scheme");

			AddSpace(10.0f);
			AddImage(controllerImage, IMAGE_HEIGHT);
			AddSpace(15.0f);

			AddClickable("Back", () => { Exit(); return "Back"; });
		}

	}
}
