using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			// Graphics
			this.Fullscreen = false;
			this.VSync = true;
			this.MultiSampling = true;
			this.InternalResX = 1280;
			this.InternalResY = 720;

			// Controls
			this.RightStickInvertedX = false;
			this.RightStickInvertedY = false;
		}
	}
}
