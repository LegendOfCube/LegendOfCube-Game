using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Wrapper class to easily access ini reading/writing in kernel32.dll.
	/// Inspired by similar class found here: http://www.codeproject.com/Articles/1966/An-INI-file-handling-class-using-C
	/// </summary>
	public class IniFile
	{
		// Private Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private const int TEMP_STR_SIZE = 512;
		private readonly StringBuilder tempStr = new StringBuilder(TEMP_STR_SIZE);

		// Public Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public readonly string INIPath;

		// Constructor
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public IniFile(string path)
		{
			this.INIPath = Path.GetFullPath(path);
		}

		// Functions exposed from kernel32.dll
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool WriteBool(string section, string key, bool value)
		{
			return WritePrivateProfileString(section, key, value.ToString(), INIPath) != 0;
		}

		public bool WriteInt(string section, string key, int value)
		{
			return WritePrivateProfileString(section, key, value.ToString(), INIPath) != 0;
		}

		public bool WriteFloat(string section, string key, float value)
		{
			return WritePrivateProfileString(section, key, value.ToString(), INIPath) != 0;
		}

		public bool ReadBool(string section, string key)
		{
			int res = GetPrivateProfileString(section, key, "", tempStr, TEMP_STR_SIZE, INIPath);
			return bool.Parse(tempStr.ToString());
		}

		public int ReadInt(string section, string key)
		{
			int res = GetPrivateProfileString(section, key, "", tempStr, TEMP_STR_SIZE, INIPath);
			return int.Parse(tempStr.ToString());
		}

		public float ReadFloat(string section, string key)
		{
			int res = GetPrivateProfileString(section, key, "", tempStr, TEMP_STR_SIZE, INIPath);
			return float.Parse(tempStr.ToString());
		}
	}
}
