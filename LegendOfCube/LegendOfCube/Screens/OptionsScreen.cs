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
			AddOnOffSelector("Fullscreen", cfg.Fullscreen, (bool b) => { cfg.Fullscreen = b; });
			AddOnOffSelector("VSync", cfg.VSync, (bool b) => { cfg.VSync = b; });
			AddOnOffSelector("MultiSampling", cfg.MultiSampling, (bool b) => { cfg.MultiSampling = b; });
			AddSpace(35.0f);

			AddHeading("Controls");
			AddOnOffSelector("InvertX", cfg.RightStickInvertedX, (bool b) => { cfg.RightStickInvertedX = b; });
			AddOnOffSelector("InvertY", cfg.RightStickInvertedY, (bool b) => { cfg.RightStickInvertedY = b; });
			AddSpace(35.0f);

			AddClickable("Reset to defaults", () => { cfg.ResetToDefaults(); this.OnExit(); ScreenSystem.RemoveCurrentScreen(); return "Reset to defaults"; });
			AddClickable("Main Menu", () => { this.OnExit(); ScreenSystem.RemoveCurrentScreen(); return "null"; });

		}

		internal sealed override void OnExit()
		{
			cfg.SaveToFile();
		}

		private static string Parse(bool b)
		{
			return b ? "On" : "Off";
		}
	}
}
