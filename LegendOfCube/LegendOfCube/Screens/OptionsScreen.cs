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

			AddTitle("Options");
			AddDescription("Options are stored in \"Documents/My Games/Legend of Cube/Config.ini\"");
			AddDescription("Legend of Cube needs to be restarted for some changes to take effect");
			AddSpace(35.0f);
			
			AddHeading("Graphics");
			AddClickable("Fullscreen: " + Parse(cfg.Fullscreen), () => { cfg.Fullscreen = !cfg.Fullscreen; cfg.SaveToFile(); return "Fullscreen: " + Parse(cfg.Fullscreen); });
			AddClickable("VSync: " + Parse(cfg.VSync), () => { cfg.VSync = !cfg.VSync; cfg.SaveToFile(); return "VSync: " + Parse(cfg.VSync); });
			AddClickable("MultiSampling: " + Parse(cfg.MultiSampling), () => { cfg.MultiSampling = !cfg.MultiSampling; return "MultiSampling: " + Parse(cfg.MultiSampling); });
			AddSpace(35.0f);

			AddHeading("Controls");
			AddClickable("InvertX: " + Parse(cfg.RightStickInvertedX), () => { cfg.RightStickInvertedX = !cfg.RightStickInvertedX; cfg.SaveToFile(); return "InvertX: " + Parse(cfg.RightStickInvertedX); });
			AddClickable("InvertY: " + Parse(cfg.RightStickInvertedY), () => { cfg.RightStickInvertedY = !cfg.RightStickInvertedY; cfg.SaveToFile(); return "InvertY: " + Parse(cfg.RightStickInvertedY); });
			AddSpace(35.0f);

			AddClickable("Main Menu", () => { ScreenSystem.RemoveCurrentScreen(); return "null"; });

		}

		private static string Parse(bool b)
		{
			return b ? "On" : "Off";
		}
	}
}
