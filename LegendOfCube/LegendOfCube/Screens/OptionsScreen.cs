using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	public class OptionsScreen : BaseMenuScreen
	{
		private GlobalConfig cfg;

		public OptionsScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) { }

		internal sealed override void InitializeScreen()
		{
			cfg = GlobalConfig.Instance;

			Action herp = () =>
			{
				cfg.SaveToFile();
				ScreenSystem.RemoveCurrentScreen();
				ScreenSystem.AddScreen(new OptionsScreen(Game, ScreenSystem));
			};

			AddTitleBelow("Options");
			AddDescriptionBelow("Options are stored in \"Documents/My Games/Legend of Cube/Config.ini\"");
			AddDescriptionBelow("Legend of Cube needs to be restarted for some changes to take effect.");
			AddSpaceBelow(35.0f);
			
			AddHeadingBelow("Graphics");
			AddClickableBelow("Fullscreen: " + Parse(cfg.Fullscreen), () => { cfg.Fullscreen = !cfg.Fullscreen; herp(); });
			AddClickableBelow("VSync: " + Parse(cfg.VSync), () => { cfg.VSync = !cfg.VSync; herp(); });
			AddClickableBelow("MultiSampling: " + Parse(cfg.MultiSampling), () => { cfg.MultiSampling = !cfg.MultiSampling; herp(); });
			AddSpaceBelow(35.0f);

			AddHeadingBelow("Controls");
			AddClickableBelow("InvertX: " + Parse(cfg.RightStickInvertedX), () => { cfg.RightStickInvertedX = !cfg.RightStickInvertedX; herp(); });
			AddClickableBelow("InvertY: " + Parse(cfg.RightStickInvertedY), () => { cfg.RightStickInvertedY = !cfg.RightStickInvertedY; herp(); });
			AddSpaceBelow(35.0f);

			AddClickableBelow("Main Menu", () => { ScreenSystem.RemoveCurrentScreen(); });
		}

		private static string Parse(bool b)
		{
			return b ? "On" : "Off";
		}
	}
}
