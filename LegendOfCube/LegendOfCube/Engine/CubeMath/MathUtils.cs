using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.CubeMath
{
	public class MathUtils
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

		public static bool ApproxEqu(Matrix lhs, Matrix rhs, float epsilon)
		{
			if (!ApproxEqu(lhs.M11, rhs.M11, epsilon)) return false;
			if (!ApproxEqu(lhs.M12, rhs.M12, epsilon)) return false;
			if (!ApproxEqu(lhs.M13, rhs.M13, epsilon)) return false;
			if (!ApproxEqu(lhs.M14, rhs.M14, epsilon)) return false;

			if (!ApproxEqu(lhs.M21, rhs.M21, epsilon)) return false;
			if (!ApproxEqu(lhs.M22, rhs.M22, epsilon)) return false;
			if (!ApproxEqu(lhs.M23, rhs.M23, epsilon)) return false;
			if (!ApproxEqu(lhs.M24, rhs.M24, epsilon)) return false;

			if (!ApproxEqu(lhs.M31, rhs.M31, epsilon)) return false;
			if (!ApproxEqu(lhs.M32, rhs.M32, epsilon)) return false;
			if (!ApproxEqu(lhs.M33, rhs.M33, epsilon)) return false;
			if (!ApproxEqu(lhs.M34, rhs.M34, epsilon)) return false;

			if (!ApproxEqu(lhs.M41, rhs.M41, epsilon)) return false;
			if (!ApproxEqu(lhs.M42, rhs.M42, epsilon)) return false;
			if (!ApproxEqu(lhs.M43, rhs.M43, epsilon)) return false;
			if (!ApproxEqu(lhs.M44, rhs.M44, epsilon)) return false;

			return true;
		}

		public static float ClampLerp(float value, float minOut, float maxOut, float minIn, float maxIn)
		{
			return MathHelper.Clamp(MathHelper.Lerp(minOut, maxOut, minIn + value / (maxIn - minIn)), minOut, maxOut);
		}

		public static Vector3 Lerp(float value, Vector3 min, Vector3 max)
		{
			return min + (max - min) * value;
		}
	}
}
