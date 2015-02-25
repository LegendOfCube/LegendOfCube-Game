using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.CubeMath
{
	public struct Matrix3x3
	{
		// Public members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public float M11, M12, M13;
		public float M21, M22, M23;
		public float M31, M32, M33;

		// Creation functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static Matrix3x3 CreateIdentity()
		{
			Matrix3x3 temp = new Matrix3x3();
			temp.ResetToIdentity();
			return temp;
		}

		public static Matrix3x3 CreateChangeOfBasis(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
		{
			Matrix3x3 result;
			result.M11 = xAxis.X;
			result.M21 = xAxis.Y;
			result.M31 = xAxis.Z;

			result.M12 = yAxis.X;
			result.M22 = yAxis.Y;
			result.M32 = yAxis.Z;

			result.M13 = zAxis.X;
			result.M23 = zAxis.Y;
			result.M33 = zAxis.Z;
			return result;
		}

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ResetToIdentity()
		{
			M11 = 1; M12 = 0; M13 = 0;
			M21 = 0; M22 = 1; M23 = 0;
			M31 = 0; M32 = 0; M33 = 1;
		}

		public Matrix3x3 Transpose()
		{
			Matrix3x3 result;
			result.M11 = M11;
			result.M12 = M21;
			result.M13 = M31;
			result.M21 = M12;
			result.M22 = M22;
			result.M23 = M32;
			result.M31 = M13;
			result.M32 = M23;
			result.M33 = M33;
			return result;
		}

		public static void Transpose(ref Matrix3x3 matrix, out Matrix3x3 result)
		{
			result.M11 = matrix.M11;
			result.M12 = matrix.M21;
			result.M13 = matrix.M31;
			result.M21 = matrix.M12;
			result.M22 = matrix.M22;
			result.M23 = matrix.M32;
			result.M31 = matrix.M13;
			result.M32 = matrix.M23;
			result.M33 = matrix.M33;
		}

		public static Vector3 operator* (Matrix3x3 m, Vector3 v)
		{
			Vector3 result;
			result.X = v.X * m.M11 + v.Y * m.M12 + v.Z * m.M13;
			result.Y = v.Y * m.M21 + v.Y * m.M22 + v.Z * m.M23;
			result.Z = v.Z * m.M31 + v.Y * m.M32 + v.Z * m.M33;
			return result;
		}

		public static Matrix3x3 operator* (Matrix3x3 lhs, Matrix3x3 rhs)
		{
			Matrix3x3 result;
			result.M11 = lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31;
			result.M12 = lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32;
			result.M13 = lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33;

			result.M21 = lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31;
			result.M22 = lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32;
			result.M23 = lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33;

			result.M31 = lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31;
			result.M32 = lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32;
			result.M33 = lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33;

			return result;
		}

		public static void Transform(ref Matrix3x3 m, ref Vector3 v, out Vector3 result)
		{
			result.X = v.X * m.M11 + v.Y * m.M12 + v.Z * m.M13;
			result.Y = v.X * m.M21 + v.Y * m.M22 + v.Z * m.M23;
			result.Z = v.X * m.M31 + v.Y * m.M32 + v.Z * m.M33;
		}

		public static void Multiply(ref Matrix3x3 lhs, ref Matrix3x3 rhs, out Matrix3x3 result)
		{
			result.M11 = lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31;
			result.M12 = lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32;
			result.M13 = lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33;

			result.M21 = lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31;
			result.M22 = lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32;
			result.M23 = lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33;

			result.M31 = lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31;
			result.M32 = lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32;
			result.M33 = lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33;
		}

		// Getters/setters
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public float At(uint i, uint j)
		{
			switch (i)
			{
				case 1:
					switch (j)
					{
						case 1: return M11;
						case 2: return M12;
						case 3: return M13;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				case 2:
					switch (j)
					{
						case 1: return M21;
						case 2: return M22;
						case 3: return M23;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				case 3:
					switch (j)
					{
						case 1: return M31;
						case 2: return M32;
						case 3: return M33;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				default: throw new ArgumentException("Not fulfilled: 1 <= i <= 3");
			}
		}

		public Vector3 RowAt(uint i)
		{
			return new Vector3(At(i, 1), At(i, 2), At(i, 3));
		}

		public Vector3 ColumnAt(uint j)
		{
			return new Vector3(At(1, j), At(2, j), At(3, j));
		}

		public void Set(uint i, uint j, float value)
		{
			switch (i)
			{
				case 1:
					switch (j)
					{
						case 1: M11 = value; return;
						case 2: M12 = value; return;
						case 3: M13 = value; return;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				case 2:
					switch (j)
					{
						case 1: M21 = value; return;
						case 2: M22 = value; return;
						case 3: M23 = value; return;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				case 3:
					switch (j)
					{
						case 1: M31 = value; return;
						case 2: M32 = value; return;
						case 3: M33 = value; return;
						default: throw new ArgumentException("Not fulfilled: 1 <= j <= 3");
					}
				default: throw new ArgumentException("Not fulfilled: 1 <= i <= 3");
			}
		}

		public void SetRow(uint i, Vector3 row)
		{
			Set(i, 1, row.X);
			Set(i, 2, row.Y);
			Set(i, 3, row.Z);
		}

		public void SetColumn(uint j, Vector3 column)
		{
			Set(1, j, column.X);
			Set(2, j, column.Y);
			Set(3, j, column.Z);
		}
	}
}
