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

		struct Res { public int x, y; public Res(int x, int y) { this.x = x; this.y = y; } }
		private string[] resolutionStrs = { "480x240 (4:3)", "640x480 (4:3)", "1280x720 (16:9)", "1280x800 (16:10)", "1600x900 (16:9)", "1920x1080 (16:9)", "1920x1200 (16:10)", "2560x1440 (16:9)", "2560x1600 (16:10)" };
		private Res[] resolutions = { new Res(480, 240), new Res(640, 480), new Res(1280, 720), new Res(1280, 800), new Res(1600, 900), new Res(1920, 1080), new Res(1920, 1200), new Res(2560, 1440), new Res(2560, 1600)};

		public OptionsScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem) { }

		internal sealed override void InitializeScreen()
		{
			cfg = GlobalConfig.Instance;

			AddTitle("Options");
			AddDescription("Options are stored in \"Documents/My Games/Legend of Cube/Config.ini\"");
			AddDescription("Legend of Cube needs to be restarted for some changes to take effect");
			AddSpace(35.0f);
			
			AddHeading("Graphics");
			AddMultiChoiceSelector("Resolution", 0, resolutionStrs, (int i) => { cfg.InternalResX = resolutions[i].x; cfg.InternalResY = resolutions[i].y; });
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
