using System;
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

		private static readonly GlobalConfig INSTANCE = new GlobalConfig();
		public static GlobalConfig Instance
		{
			get
			{
				return INSTANCE;
			}
		}

		// Private members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly IniFile iniFile;

		// Settings
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Graphics
		public bool Fullscreen;
		public int InternalResX;
		public int InternalResY;
		public bool VSync;
		public bool MultiSampling;
		public float Fov;

		// Controls
		public bool RightStickInvertedX;
		public bool RightStickInvertedY;

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void LoadFromFile()
		{
			// Graphics
			this.Fullscreen = SanitizeIniBool("Graphics", "Fullscreen", false);
			this.VSync = SanitizeIniBool("Graphics", "VSync", true);
			this.MultiSampling = SanitizeIniBool("Graphics", "MultiSampling", true);
			this.InternalResX = SanitizeIniInt("Graphics", "InternalResX", 320, 15360, 1280);
			this.InternalResY = SanitizeIniInt("Graphics", "InternalResY", 240, 8640, 800);
			this.Fov = SanitizeIniFloat("Graphics", "FOV", 40.0f, 120.0f, 70.0f);

			// Controls
			this.RightStickInvertedX = SanitizeIniBool("Controls", "RightStickInvertedX", false);
			this.RightStickInvertedY = SanitizeIniBool("Controls", "RightStickInvertedY", false);
		}

		public void SaveToFile()
		{
			Directory.CreateDirectory(Path.GetDirectoryName(iniFile.INIPath));

			// Graphics
			iniFile.WriteBool("Graphics", "Fullscreen", Fullscreen);
			iniFile.WriteBool("Graphics", "VSync", VSync);
			iniFile.WriteBool("Graphics", "MultiSampling", MultiSampling);
			iniFile.WriteInt("Graphics", "InternalResX", InternalResX);
			iniFile.WriteInt("Graphics", "InternalResY", InternalResY);
			iniFile.WriteFloat("Graphics", "FOV", Fov);

			// Controls
			iniFile.WriteBool("Controls", "RightStickInvertedX", RightStickInvertedX);
			iniFile.WriteBool("Controls", "RightStickInvertedY", RightStickInvertedY);
		}

		public void ResetToDefaults()
		{
			File.Delete(iniFile.INIPath);
			LoadFromFile();
		}

		// Constructor
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private GlobalConfig()
		{
			var iniPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
			              @"\My Games\Legend of Cube\Config.ini";
			iniFile = new IniFile(iniPath);

			LoadFromFile();
			SaveToFile(); // TODO: Maybe unnecessary, but will ensure that we generate .ini file when first run.
		}

		// Private Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private int SanitizeIniInt(string section, string key, int minVal, int maxVal, int defaultVal)
		{
			int i;
			try
			{
				i = iniFile.ReadInt(section, key);
				if (i < minVal) i = minVal;
				if (i > maxVal) i = maxVal;
			}
			catch (Exception e)
			{
				i = defaultVal;
			}
			return i;
		}

		private float SanitizeIniFloat(string section, string key, float minVal, float maxVal, float defaultVal)
		{
			float f;
			try
			{
				f = iniFile.ReadFloat(section, key);
				if (f < minVal) f = minVal;
				if (f > maxVal) f = maxVal;
			}
			catch (Exception)
			{
				f = defaultVal;
			}
			return f;
		}

		private bool SanitizeIniBool(string section, string key, bool defaultVal)
		{
			bool b;
			try
			{
				b = iniFile.ReadBool(section, key);
			}
			catch (Exception)
			{
				b = defaultVal;
			}
			return b;
		}
	}
}
