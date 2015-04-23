using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	static class UiUtils
	{
		public static string UIFormat(Vector3 value)
		{
			return String.Format("(X: {0}, Y: {1}, Z: {2})", UIFormat(value.X), UIFormat(value.Y), UIFormat(value.Z));
		}

		public static string UIFormat(float value)
		{
			return String.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}", value);
		}
	}
}
