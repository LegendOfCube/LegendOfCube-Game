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
@"The goal of the game is to navigate the playable cube to the end of the level. The fastest
time for each level is stored and can be seen in the level select screen.";

		private const int IMAGE_HEIGHT = 350;

		private Texture2D controllerImage;

		public InstructionsScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) {}

		internal override void InitializeScreen()
		{
			controllerImage = Game.Content.Load<Texture2D>("LoCControlScheme");

			AddTitle("Instructions");

			AddDescription(INSTRUCTION_TEXT);
			AddSpace(20.0f);

			AddTitle("Control Scheme");

			AddSpace(40.0f);
			AddImage(controllerImage, IMAGE_HEIGHT);
			AddSpace(40.0f);

			AddClickable("Back", () => { Exit(); return "Back"; });
		}

	}
}
