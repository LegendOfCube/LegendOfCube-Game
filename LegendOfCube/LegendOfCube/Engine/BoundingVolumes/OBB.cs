using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			this.halfExtents = new Vector3(0, 0, 0);
			this.halfExtents.X = extents.X / 2.0f;
			this.halfExtents.Y = extents.Y / 2.0f;
			this.halfExtents.Z = extents.Z / 2.0f;

			EnsureCorrectState();
		}

		public OBB(Vector3 centerPos, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, float xExtent, float yExtent, float zExtent)
		{
			this.center = centerPos;
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.zAxis = zAxis;
			this.halfExtents = new Vector3(0, 0, 0);
			this.halfExtents.X = xExtent / 2.0f;
			this.halfExtents.Y = yExtent / 2.0f;
			this.halfExtents.Z = zExtent / 2.0f;

			EnsureCorrectState();
		}

		// Public properties
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool Intersects(ref OBB other)
		{

			return true;
		}
		
		// Public properties
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public Vector3 CenterPos
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
				halfExtents = value * 2.0f;
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
