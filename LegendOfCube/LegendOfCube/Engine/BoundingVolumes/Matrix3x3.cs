using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
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
			Matrix3x3 temp = new Matrix3x3();
			temp.SetColumn(1, xAxis);
			temp.SetColumn(2, yAxis);
			temp.SetColumn(3, zAxis);
			return temp;
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
			Matrix3x3 transp = this;
			for (uint i = 1; i <= 3; i++)
			{
				for (uint j = 1; j <= 3; j++)
				{
					transp.Set(i, j, this.At(j, i));
				}
			}
			return transp;
		}

		public static Vector3 operator* (Matrix3x3 m, Vector3 v)
		{
			Vector3 result = new Vector3();
			result.X = Vector3.Dot(m.RowAt(1), v);
			result.Y = Vector3.Dot(m.RowAt(2), v);
			result.Z = Vector3.Dot(m.RowAt(3), v);
			return result;
		}

		public static Matrix3x3 operator* (Matrix3x3 lhs, Matrix3x3 rhs)
		{
			Matrix3x3 result = new Matrix3x3();
			for (uint i = 1; i <= 3; i++)
			{
				for (uint j = 1; j <= 3; j++)
				{
					result.Set(i, j, Vector3.Dot(lhs.RowAt(i), rhs.ColumnAt(j)));
				}
			}
			return result;
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
