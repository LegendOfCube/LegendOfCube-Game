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

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void resetToIdentity()
		{
			M11 = 1; M12 = 0; M13 = 0;
			M21 = 0; M22 = 1; M23 = 0;
			M31 = 0; M32 = 0; M33 = 1;
		}

		public Matrix3x3 transpose()
		{
			Matrix3x3 transp = this;
			for (uint i = 1; i <= 3; i++)
			{
				for (uint j = 1; j <= 3; j++)
				{
					transp.set(i, j, this.at(j, i));
				}
			}
			return transp;
		}

		public static Vector3 operator* (Matrix3x3 m, Vector3 v)
		{
			Vector3 result = new Vector3();
			result.X = Vector3.Dot(m.rowAt(1), v);
			result.Y = Vector3.Dot(m.rowAt(2), v);
			result.Z = Vector3.Dot(m.rowAt(3), v);
			return result;
		}

		// Getters/setters
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public float at(uint i, uint j)
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

		public Vector3 rowAt(uint i)
		{
			return new Vector3(at(i, 1), at(i, 2), at(i, 3));
		}

		public Vector3 columnAt(uint j)
		{
			return new Vector3(at(1, j), at(2, j), at(3, j));
		}

		public void set(uint i, uint j, float value)
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

		public void setRow(uint i, Vector3 row)
		{
			set(i, 1, row.X);
			set(i, 2, row.Y);
			set(i, 3, row.Z);
		}

		public void setColumn(uint j, Vector3 column)
		{
			set(1, j, column.X);
			set(2, j, column.Y);
			set(3, j, column.Z);
		}
	}
}
