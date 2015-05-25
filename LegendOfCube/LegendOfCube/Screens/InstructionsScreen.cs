using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	class InstructionsScreen : BaseMenuScreen
	{
		private const string INSTRUCTION_TEXT = 
@"Some text here...";

		private const int IMAGE_HEIGHT = 350;

		private Texture2D controllerImage;

		public InstructionsScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) {}

		internal override void InitializeScreen()
		{
			controllerImage = Game.Content.Load<Texture2D>("LoCControlScheme");

			AddTitle("Instructions");

			AddDescription(INSTRUCTION_TEXT);

			AddSpace(40.0f);
			AddImage(controllerImage, IMAGE_HEIGHT);
			AddSpace(40.0f);

			AddClickable("Back", () => { Exit(); return "Back"; });
		}

	}
}
