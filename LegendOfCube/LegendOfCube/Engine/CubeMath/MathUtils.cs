using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.CubeMath
{
	class MathUtils
	{
		public static bool ApproxEqu(float lhs, float rhs, float epsilon)
		{
			return (lhs - epsilon) <= rhs && (lhs + epsilon) >= rhs;
		}

		public static bool ApproxEqu(Vector3 lhs, Vector3 rhs, float epsilon)
		{
			if (!ApproxEqu(lhs.X, rhs.X, epsilon)) return false;
			if (!ApproxEqu(lhs.Y, rhs.Y, epsilon)) return false;
			if (!ApproxEqu(lhs.Z, rhs.Z, epsilon)) return false;
			return true;
		}

		public static float ClampLerp(float value, float minOut, float maxOut, float minIn, float maxIn)
		{
			return MathHelper.Clamp(MathHelper.Lerp(minOut, maxOut, minIn + value / (maxIn - minIn)), minOut, maxOut);
		}
	}
}
