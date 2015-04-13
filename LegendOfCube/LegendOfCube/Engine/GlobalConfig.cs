using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine
{
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
			
		}
	}
}
