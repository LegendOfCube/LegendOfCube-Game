using System;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.CubeMath;

namespace LegendOfCube.Engine.BoundingVolumes
{
	public enum OBBAxis
	{
		X_PLUS, X_MINUS, Y_PLUS, Y_MINUS, Z_PLUS, Z_MINUS
	}

	static class OBBAxisMethods
	{
		public static bool IsNegative(this OBBAxis axis)
		{
			switch(axis)
			{
				case OBBAxis.X_PLUS:
				case OBBAxis.Y_PLUS:
				case OBBAxis.Z_PLUS:
					return false;
				case OBBAxis.X_MINUS:
				case OBBAxis.Y_MINUS:
				case OBBAxis.Z_MINUS:
					return true;
			}
			throw new ArgumentException();
		}

		public static float Sign(this OBBAxis axis)
		{
			return axis.IsNegative() ? -1.0f : 1.0f;
		}
	}

	public struct OBB {

		// Public members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static readonly OBB IDENTITY = new OBB(Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1.0f, 1.0f, 1.0f);

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

		public Vector3 ClosestPointOnOBB(ref Vector3 point)
		{
			// Algorithm from Real-Time Collision Detection

			Vector3 distToPoint = point - center;
			Vector3 result = center; // Start at center of Cube.

			// C# is crap so I have to manually unroll this loop.
			float dist;

			dist = Vector3.Dot(distToPoint, xAxis);
			if (dist > halfExtents.X) dist = halfExtents.X;
			if (dist < (-halfExtents.X)) dist = -halfExtents.X;
			result += (dist * xAxis);

			dist = Vector3.Dot(distToPoint, yAxis);
			if (dist > halfExtents.Y) dist = halfExtents.Y;
			if (dist < (-halfExtents.Y)) dist = -halfExtents.Y;
			result += (dist * yAxis);

			dist = Vector3.Dot(distToPoint, zAxis);
			if (dist > halfExtents.Z) dist = halfExtents.Z;
			if (dist < (-halfExtents.Z)) dist = -halfExtents.Z;
			result += (dist * zAxis);

			return result;
		}

		/// <summary>
		/// Finds the axis in this OBB that is closest to specified direction.
		/// This function does NOT care about extents or lengths of vectors in this calculation,
		/// only the directions.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns>closest axis</returns>
		public Vector3 ClosestAxis(ref Vector3 direction)
		{
			Vector3 dir = direction;
			dir.Normalize();

			float xDot = Vector3.Dot(dir, xAxis);
			float yDot = Vector3.Dot(dir, yAxis);
			float zDot = Vector3.Dot(dir, zAxis);
			float xDotAbs = Math.Abs(xDot);
			float yDotAbs = Math.Abs(yDot);
			float zDotAbs = Math.Abs(zDot);

			if (yDotAbs >= xDotAbs && yDotAbs >= zDotAbs)
			{
				return ((float)Math.Sign(yDot)) * yAxis;
			}
			else if (xDotAbs >= yDotAbs && xDotAbs >= zDotAbs)
			{
				return ((float)Math.Sign(xDot)) * xAxis;
			}
			else if (zDotAbs >= xDotAbs && zDotAbs >= yDotAbs)
			{
				return ((float)Math.Sign(zDot)) * zAxis;
			}

			return Vector3.Zero;
		}

		public OBBAxis ClosestAxisEnum(ref Vector3 direction)
		{
			Vector3 dir = direction;
			dir.Normalize();

			float xDot = Vector3.Dot(dir, xAxis);
			float yDot = Vector3.Dot(dir, yAxis);
			float zDot = Vector3.Dot(dir, zAxis);
			float xDotAbs = Math.Abs(xDot);
			float yDotAbs = Math.Abs(yDot);
			float zDotAbs = Math.Abs(zDot);

			if (yDotAbs >= xDotAbs && yDotAbs >= zDotAbs)
			{
				if (((float)Math.Sign(yDot)) >= 0.0f) return OBBAxis.Y_PLUS;
				else return OBBAxis.Y_MINUS;
			}
			else if (xDotAbs >= yDotAbs && xDotAbs >= zDotAbs)
			{
				if (((float)Math.Sign(xDot)) >= 0.0f) return OBBAxis.X_PLUS;
				else return OBBAxis.X_MINUS;
			}
			else if (zDotAbs >= xDotAbs && zDotAbs >= yDotAbs)
			{
				if (((float)Math.Sign(zDot)) >= 0.0f) return OBBAxis.Z_PLUS;
				else return OBBAxis.Z_MINUS;
			}

			throw new ArgumentException("direction must be a direction");
		}

		public bool ApproxEqu(ref OBB other, float epsilon)
		{
			if (!MathUtils.ApproxEqu(center, other.center, epsilon)) return false;
			if (!MathUtils.ApproxEqu(xAxis, other.xAxis, epsilon)) return false;
			if (!MathUtils.ApproxEqu(yAxis, other.yAxis, epsilon)) return false;
			if (!MathUtils.ApproxEqu(zAxis, other.zAxis, epsilon)) return false;
			return true;
		}

		public Matrix IdentityToThisMatrix()
		{
			Matrix m = Matrix.Identity;
			//m.Right = xAxis * halfExtents.X * 2.0f;
			//m.Up = yAxis * halfExtents.Y * 2.0f;
			//m.Backward = zAxis * halfExtents.Z * 2.0f;

			Vector3 x = xAxis * halfExtents.X * 2.0f;
			m.M11 = x.X;
			m.M12 = x.Y;
			m.M13 = x.Z;

			Vector3 y = yAxis * halfExtents.Y * 2.0f;
			m.M21 = y.X;
			m.M22 = y.Y;
			m.M23 = y.Z;

			Vector3 z = zAxis * halfExtents.Z * 2.0f;
			m.M31 = z.X;
			m.M32 = z.Y;
			m.M33 = z.Z;

			m.M41 = center.X;
			m.M42 = center.Y;
			m.M43 = center.Z;

			return m;
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

			OBB result;
			Vector3.Transform(ref obb.center, ref transform, out result.center);
			Vector3.TransformNormal(ref obbX, ref transform, out result.xAxis);
			Vector3.TransformNormal(ref obbY, ref transform, out result.yAxis);
			Vector3.TransformNormal(ref obbZ, ref transform, out result.zAxis);

			result.halfExtents = new Vector3
			{
				X = result.xAxis.Length() / 2.0f,
				Y = result.yAxis.Length() / 2.0f,
				Z = result.zAxis.Length() / 2.0f
			};

			result.xAxis.Normalize();
			result.yAxis.Normalize();
			result.zAxis.Normalize();

			return result;
		}

		public static Matrix TransformFromOBBs(ref OBB pre, ref OBB post)
		{
			return post.IdentityToThisMatrix() * Matrix.Invert(pre.IdentityToThisMatrix());
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

		public Vector3 GetAxis(OBBAxis axis)
		{
			switch (axis)
			{
				case OBBAxis.X_PLUS: return xAxis;
				case OBBAxis.X_MINUS: return -xAxis;
				case OBBAxis.Y_PLUS: return yAxis;
				case OBBAxis.Y_MINUS: return -yAxis;
				case OBBAxis.Z_PLUS: return zAxis;
				case OBBAxis.Z_MINUS: return -zAxis;
			}
			throw new ArgumentException("Should never happen.");
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

		private void EnsureCorrectState()
		{
			const float EPSILON = 0.001f;

			// Axes are normalized
			if (!MathUtils.ApproxEqu(xAxis.Length(), 1.0f, EPSILON)) xAxis.Normalize();
			if (!MathUtils.ApproxEqu(xAxis.Length(), 1.0f, EPSILON)) yAxis.Normalize();
			if (!MathUtils.ApproxEqu(xAxis.Length(), 1.0f, EPSILON)) zAxis.Normalize();

			// Axes are orthogonal
			if (!MathUtils.ApproxEqu(Vector3.Dot(xAxis, yAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");
			if (!MathUtils.ApproxEqu(Vector3.Dot(xAxis, zAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");
			if (!MathUtils.ApproxEqu(Vector3.Dot(yAxis, zAxis), 0.0f, EPSILON)) throw new ArgumentException("Invalid axis.");

			// Extents
			if (halfExtents.X <= 0) throw new ArgumentException("halfExtents.X <= 0");
			if (halfExtents.Y <= 0) throw new ArgumentException("halfExtents.Y <= 0");
			if (halfExtents.Z <= 0) throw new ArgumentException("halfExtents.Z <= 0");
		}

		public static OBB CreateAxisAligned(Vector3 centerPos, float xLength, float yLength , float zLength)
		{
			return new OBB(centerPos, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, xLength, yLength, zLength);
		}
	}
}
