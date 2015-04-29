using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.CubeMath
{
	public static class MathUtils
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

		/// <summary>
		/// Map a value in one range to value in another, with linear interpolation and clamping at the ends.
		/// </summary>
		public static float MapRangeToRange(float valueIn, float startIn, float endIn, float startOut, float endOut)
		{
			float inFraction = (valueIn - startIn) / (endIn - startIn);
			return ClampLerp(inFraction, startOut, endOut);
		}

		/// <summary>
		/// Perform linear interpolation.
		/// </summary>
		public static float Lerp(float amount, float start, float end)
		{
			return MathHelper.Lerp(start, end, amount);
		}

		/// <summary>
		/// Perform linear interpolation.
		/// </summary>
		public static Vector3 Lerp(float amount, Vector3 start, Vector3 end)
		{
			return start + (end - start) * amount;
		}

		/// <summary>
		/// Perform linear interpolation with clamping at the ends.
		/// </summary>
		public static float ClampLerp(float amount, float start, float end)
		{
			return Lerp(MathHelper.Clamp(amount, 0.0f, 1.0f), start, end);
		}

		/// <summary>
		/// Perform linear interpolation with clamping at the ends.
		/// </summary>
		public static Vector3 ClampLerp(float amount, Vector3 start, Vector3 end)
		{
			return Lerp(MathHelper.Clamp(amount, 0.0f, 1.0f), start, end);
		}

		/// <summary>
		/// Clamps an int value between a min and a max value.
		/// </summary>
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			else if (value > max)
			{
				return max;
			}
			else
			{
				return value;
			}
		}
	}
}
