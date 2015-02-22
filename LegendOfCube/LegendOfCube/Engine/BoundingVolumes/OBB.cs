using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
{
	public struct OBB {

		// Private members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Vector3 center;
		private Vector3 xAxis, yAxis, zAxis;
		private Vector3 halfExtents;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public OBB(Vector3 centerPos, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Vector3 extents)
		{
			this.center = centerPos;
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.zAxis = zAxis;
			this.halfExtents = new Vector3
			{
				X = extents.X / 2.0f, 
				Y = extents.Y / 2.0f, 
				Z = extents.Z / 2.0f
			};

			EnsureCorrectState();
		}

		public OBB(Vector3 centerPos, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, float xExtent, float yExtent, float zExtent)
		{
			this.center = centerPos;
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.zAxis = zAxis;
			this.halfExtents = new Vector3{
				X = xExtent / 2.0f,
				Y = yExtent / 2.0f,
				Z = zExtent / 2.0f
			};

			EnsureCorrectState();
		}

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool Intersects(ref OBB other)
		{
			return IntersectionsTests.Intersects(ref this, ref other);
		}

		public static OBB TransformOBB(ref OBB obb, ref Matrix transform)
		{
			Vector3 obbX = obb.AxisX;
			obbX.Normalize();
			obbX *= obb.ExtentX;
			Vector3 obbY = obb.AxisY;
			obbY.Normalize();
			obbY *= obb.ExtentY;
			Vector3 obbZ = obb.AxisZ;
			obbZ.Normalize();
			obbZ *= obb.ExtentZ;

			OBB result = new OBB
			{
				center = transform.Translation + Transform(ref transform, ref obb.center),
				xAxis = Transform(ref transform, ref obbX),
				yAxis = Transform(ref transform, ref obbY),
				zAxis = Transform(ref transform, ref obbZ)
			};

			result.halfExtents = new Vector3 {
				X = result.xAxis.Length() / 2.0f,
				Y = result.yAxis.Length() / 2.0f,
				Z = result.zAxis.Length() / 2.0f
			};

			result.xAxis.Normalize();
			result.yAxis.Normalize();
			result.zAxis.Normalize();

			return result;
		}

		private static Vector3 Transform(ref Matrix m, ref Vector3 v)
		{
			Vector3 res = new Vector3
			{
				X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
				Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
				Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z
			};
			return res;
		}
		
		// Public properties
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public Vector3 Position
		{
			get
			{
				return center;
			}
			set
			{
				this.center = value;
			}
		}

		public Vector3 AxisX
		{
			get
			{
				return xAxis;
			}
			set
			{
				value.Normalize();
				xAxis = value;
			}
		}

		public Vector3 AxisY
		{
			get
			{
				return yAxis;
			}
			set
			{
				value.Normalize();
				yAxis = value;
			}
		}

		public Vector3 AxisZ
		{
			get
			{
				return zAxis;
			}
			set
			{
				value.Normalize();
				zAxis = value;
			}
		}

		public Vector3 Extents
		{
			get
			{
				return halfExtents * 2.0f;
			}
			set
			{
				halfExtents = value / 2.0f;
			}
		}

		public float ExtentX
		{
			get
			{
				return halfExtents.X * 2.0f;
			}
			set
			{
				halfExtents.X = value / 2.0f;
			}
		}

		public float ExtentY
		{
			get
			{
				return halfExtents.Y * 2.0f;
			}
			set
			{
				halfExtents.Y = value / 2.0f;
			}
		}

		public float ExtentZ
		{
			get
			{
				return halfExtents.Z * 2.0f;
			}
			set
			{
				halfExtents.Z = value / 2.0f;
			}
		}

		public Vector3 HalfExtents
		{
			get
			{
				return halfExtents;
			}
			set
			{
				halfExtents = value;
			}
		}

		public float HalfExtentX
		{
			get
			{
				return halfExtents.X;
			}
			set
			{
				halfExtents.X = value;
			}
		}

		public float HalfExtentY
		{
			get
			{
				return halfExtents.Y;
			}
			set
			{
				halfExtents.Y = value;
			}
		}

		public float HalfExtentZ
		{
			get
			{
				return halfExtents.Z;
			}
			set
			{
				halfExtents.Z = value;
			}
		}

		public void Corners(Vector3[] corners)
		{
			corners[0] = Position - AxisX*HalfExtentX - AxisY*HalfExtentY - AxisZ*HalfExtentZ; // Back-bottom-left
			corners[1] = Position - AxisX*HalfExtentX - AxisY*HalfExtentY + AxisZ*HalfExtentZ; // Front-bottom-left
			corners[2] = Position - AxisX * HalfExtentX + AxisY * HalfExtentY - AxisZ * HalfExtentZ; // Back-top-left
			corners[3] = Position - AxisX * HalfExtentX + AxisY * HalfExtentY + AxisZ * HalfExtentZ; // Front-top-left
			corners[4] = Position + AxisX * HalfExtentX - AxisY * HalfExtentY - AxisZ * HalfExtentZ; // Back-bottom-right
			corners[5] = Position + AxisX * HalfExtentX - AxisY * HalfExtentY + AxisZ * HalfExtentZ; // Front-bottom-right
			corners[6] = Position + AxisX * HalfExtentX + AxisY * HalfExtentY - AxisZ * HalfExtentZ; // Back-top-right
			corners[7] = Position + AxisX * HalfExtentX + AxisY * HalfExtentY + AxisZ * HalfExtentZ; // Front-top-right
		}

		public override string ToString()
		{
			return "Pos: " + Position +
			       "\nAxisX: " + AxisX +
			       "\nAxisY: " + AxisY +
			       "\nAxisZ: " + AxisZ +
			       "\nExtents: " + Extents;
		}

		// Private functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private bool ApproxEqu(float lhs, float rhs, float epsilon)
		{
			return (lhs - epsilon) <= rhs && (lhs + epsilon) >= rhs; 
		}

		private bool ApproxEqu(Vector3 lhs, Vector3 rhs, float epsilon)
		{
			if (!ApproxEqu(lhs.X, rhs.X, epsilon)) return false;
			if (!ApproxEqu(lhs.Y, rhs.Y, epsilon)) return false;
			if (!ApproxEqu(lhs.Z, rhs.Z, epsilon)) return false;
			return true;
		}

		private void EnsureCorrectState()
		{
			// Axes are orthogonal
			const float EPSILON = 0.001f;
			if (!ApproxEqu(Vector3.Dot(xAxis, yAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");
			if (!ApproxEqu(Vector3.Dot(xAxis, zAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");
			if (!ApproxEqu(Vector3.Dot(yAxis, zAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");

			// Extents
			if (halfExtents.X <= 0) throw new ArgumentException("halfExtents.X <= 0");
			if (halfExtents.Y <= 0) throw new ArgumentException("halfExtents.Y <= 0");
			if (halfExtents.Z <= 0) throw new ArgumentException("halfExtents.Z <= 0");
		}
	}
}
