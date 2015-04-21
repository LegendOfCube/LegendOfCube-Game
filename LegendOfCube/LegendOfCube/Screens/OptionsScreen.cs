using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	class OptionsScreen : MenuScreen
	{
		private GlobalConfig cfg;

		public OptionsScreen(Game game, ScreenSystem screenSystem, InputHelper inputHelper) : base(game, screenSystem, inputHelper)
		{
			cfg = GlobalConfig.Instance;
			cfg.LoadFromFile();
		}

		internal override void LoadContent()
		{
			base.LoadContent();

			Action herp = () =>
			{
				cfg.SaveToFile();
				ScreenSystem.RemoveCurrentScreen();
				ScreenSystem.AddScreen(new OptionsScreen(Game, ScreenSystem, InputHelper));
			};

			AddTitleBelow("// Graphics", () => { } );

			if (cfg.Fullscreen)
			{
				AddItemBelow("Fullscreen: ON", () =>
				{
					cfg.Fullscreen = false;
					herp();
				});
			}
			else
			{
				AddItemBelow("Fullscreen: OFF", () =>
				{
					cfg.Fullscreen = true;
					herp();
				});
			}

			if (cfg.VSync)
			{
				AddItemBelow("VSync: ON", () =>
				{
					cfg.VSync = false;
					herp();
				});
			}
			else
			{
				AddItemBelow("VSync: OFF", () =>
				{
					cfg.VSync = true;
					herp();
				});
			}

			if (cfg.MultiSampling)
			{
				AddItemBelow("MultiSampling: ON", () =>
				{
					cfg.MultiSampling = false;
					herp();
				});
			}
			else
			{
				AddItemBelow("MultiSampling: OFF", () =>
				{
					cfg.MultiSampling = true;
					herp();
				});
			}

			AddSpaceBelow(35.0f);



			AddTitleBelow("// Controls", () => { });

			if (cfg.RightStickInvertedX)
			{
				AddItemBelow("Invert X: ON", () =>
				{
					cfg.RightStickInvertedX = false;
					herp();
				});
			}
			else
			{
				AddItemBelow("Invert X: OFF", () =>
				{
					cfg.RightStickInvertedX = true;
					herp();
				});
			}

			if (cfg.RightStickInvertedY)
			{
				AddItemBelow("Invert Y: ON", () =>
				{
					cfg.RightStickInvertedY = false;
					herp();
				});
			}
			else
			{
				AddItemBelow("Invert Y: OFF", () =>
				{
					cfg.RightStickInvertedY = true;
					herp();
				});
			}

			AddSpaceBelow(35.0f);



			AddItemBelow("Main Menu", () =>
			{
				ScreenSystem.RemoveCurrentScreen();
			});
		}
	}
}
