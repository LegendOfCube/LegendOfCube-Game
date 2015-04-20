using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// GlobalConfig for all global settings.
	/// 
	/// Settings should only be of type float, int or bool.
	/// </summary>
	public class GlobalConfig
	{
		// Singleton instance
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly GlobalConfig instance = new GlobalConfig();
		public static GlobalConfig Instance
		{
			get
			{
				return instance;
			}
		}

		// Private members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private const string INI_PATH = "LegendOfCube.ini";
		private IniFile iniFile = new IniFile(INI_PATH);

		// Settings
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Graphics
		public bool Fullscreen;
		public int InternalResX;
		public int InternalResY;
		public bool VSync;
		public bool MultiSampling;

		// Controls
		public bool RightStickInvertedX;
		public bool RightStickInvertedY;

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool LoadFromFile()
		{
			// TODO: Not yet implemented.
			return false;
		}

		public bool SaveToFile()
		{
			// TODO: Not yet implemented.
			return false;
		}

		// Constructor
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private GlobalConfig()
		{
			// TODO: Hack. Should actually check if each single item exists and add default if it doesn't.
			if (!File.Exists(INI_PATH))
			{
				// Graphics
				iniFile.WriteBool("Graphics", "Fullscreen", false);
				iniFile.WriteBool("Graphics", "VSync", true);
				iniFile.WriteBool("Graphics", "MultiSampling", true);
				iniFile.WriteInt("Graphics", "InternalResX", 1280);
				iniFile.WriteInt("Graphics", "InternalResY", 720);

				// Controls
				iniFile.WriteBool("Controls", "RightStickInvertedX", false);
				iniFile.WriteBool("Controls", "RightStickInvertedY", false);
			}

			// Graphics
			this.Fullscreen = iniFile.ReadBool("Graphics", "Fullscreen");
			this.VSync = iniFile.ReadBool("Graphics", "VSync");
			this.MultiSampling = iniFile.ReadBool("Graphics", "MultiSampling");
			this.InternalResX = iniFile.ReadInt("Graphics", "InternalResX");
			this.InternalResY = iniFile.ReadInt("Graphics", "InternalResY");

			// Controls
			this.RightStickInvertedX = iniFile.ReadBool("Controls", "RightStickInvertedX");
			this.RightStickInvertedY = iniFile.ReadBool("Controls", "RightStickInvertedY");
		}
	}
}
