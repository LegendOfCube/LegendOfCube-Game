using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
{
	public static class IntersectionsTests
	{
		private const float EPSILON = 0.0001f;

		public static bool Inside(ref Vector3 point, ref OBB box)
		{
			// TODO: Currently really sad implementation. Please fix.
			OBB pointObb = new OBB(point, new Vector3(0,0,1), new Vector3(0,1,0), new Vector3(1,0,0), 0.001f, 0.001f, 0.001f);
			return Intersects(ref pointObb, ref box);
		}

		public static bool Intersects(ref AABB boxA, ref AABB boxB)
		{
			return boxA.Intersects(ref boxB);
		}

		public static bool Intersects(ref OBB boxA, ref OBB boxB)
		{
			// SAT (Separating Axis Theorem) from Real-Time Collision Detection (chapter 4.4.1 OBB-OBB Intersection)

			// Compute rotation matrix expressing boxB in boxA's coordinate frame
			Matrix3x3 R;
			R.M11 = Vector3.Dot(boxA.AxisX, boxB.AxisX);
			R.M12 = Vector3.Dot(boxA.AxisX, boxB.AxisY);
			R.M13 = Vector3.Dot(boxA.AxisX, boxB.AxisZ);
			R.M21 = Vector3.Dot(boxA.AxisY, boxB.AxisX);
			R.M22 = Vector3.Dot(boxA.AxisY, boxB.AxisY);
			R.M23 = Vector3.Dot(boxA.AxisY, boxB.AxisZ);
			R.M31 = Vector3.Dot(boxA.AxisZ, boxB.AxisX);
			R.M32 = Vector3.Dot(boxA.AxisZ, boxB.AxisY);
			R.M33 = Vector3.Dot(boxA.AxisZ, boxB.AxisZ);

			// Compute translation vector t
			Vector3 t = boxB.Position - boxA.Position;
			// Bring translation into boxA's coordinate frame
			t = new Vector3(Vector3.Dot(t, boxA.AxisX), Vector3.Dot(t, boxA.AxisY), Vector3.Dot(t, boxA.AxisZ));

			// Compute common subexpressions. Add in an epsilon term to counteract arithmetic 
			// errors when two edges are parallel and their cross product is (near) null.
			Matrix3x3 AbsR;
			AbsR.M11 = Math.Abs(R.M11) + EPSILON;
			AbsR.M12 = Math.Abs(R.M12) + EPSILON;
			AbsR.M13 = Math.Abs(R.M13) + EPSILON;
			AbsR.M21 = Math.Abs(R.M21) + EPSILON;
			AbsR.M22 = Math.Abs(R.M22) + EPSILON;
			AbsR.M23 = Math.Abs(R.M23) + EPSILON;
			AbsR.M31 = Math.Abs(R.M31) + EPSILON;
			AbsR.M32 = Math.Abs(R.M32) + EPSILON;
			AbsR.M33 = Math.Abs(R.M33) + EPSILON;

			float ra, rb;

			// Test axis L = boxA.AxisX
			ra = boxA.HalfExtentX;
			rb = boxB.HalfExtentX * AbsR.M11 + boxB.HalfExtentY * AbsR.M12 + boxB.HalfExtentZ * AbsR.M13;
			if (Math.Abs(t.X) > ra + rb) return false;

			// Test axis L = boxA.AxisY
			ra = boxA.HalfExtentY;
			rb = boxB.HalfExtentX * AbsR.M21 + boxB.HalfExtentY * AbsR.M22 + boxB.HalfExtentZ * AbsR.M23;
			if (Math.Abs(t.Y) > ra + rb) return false;

			// Test axis L = boxA.AxisZ
			ra = boxA.HalfExtentZ;
			rb = boxB.HalfExtentX * AbsR.M31 + boxB.HalfExtentY * AbsR.M32 + boxB.HalfExtentZ * AbsR.M33;
			if (Math.Abs(t.Z) > ra + rb) return false;

			// Test axis L = boxB.AxisX
			ra = boxA.HalfExtentX * AbsR.M11 + boxA.HalfExtentY * AbsR.M21 + boxA.HalfExtentZ * AbsR.M31;
			rb = boxB.HalfExtentX;
			if (Math.Abs(t.X * R.M11 + t.Y * R.M21 + t.Z * R.M31) > ra + rb) return false;

			// Test axis L = boxB.AxisY
			ra = boxA.HalfExtentX * AbsR.M12 + boxA.HalfExtentY * AbsR.M22 + boxA.HalfExtentZ * AbsR.M32;
			rb = boxB.HalfExtentY;
			if (Math.Abs(t.X * R.M12 + t.Y * R.M22 + t.Z * R.M32) > ra + rb) return false;

			// Test axis L = boxB.AxisZ
			ra = boxA.HalfExtentX * AbsR.M13 + boxA.HalfExtentY * AbsR.M23 + boxA.HalfExtentZ * AbsR.M33;
			rb = boxB.HalfExtentZ;
			if (Math.Abs(t.X * R.M13 + t.Y * R.M23 + t.Z * R.M33) > ra + rb) return false;

			// 6 most important tests done, now for less important tests.

			// Test axis L = boxA.AxisX x boxB.AxisX
			ra = boxA.HalfExtentY * AbsR.M31 + boxA.HalfExtentZ * AbsR.M21;
			rb = boxB.HalfExtentY * AbsR.M13 + boxB.HalfExtentZ * AbsR.M12;
			if (Math.Abs(t.Z * R.M21 - t.Y * R.M31) > ra + rb) return false;

			// Test axis L = boxA.AxisX x boxB.AxisY
			ra = boxA.HalfExtentY * AbsR.M32 + boxA.HalfExtentZ * AbsR.M22;
			rb = boxB.HalfExtentX * AbsR.M13 + boxB.HalfExtentZ * AbsR.M11;
			if (Math.Abs(t.Z * R.M22 - t.Y * R.M32) > ra + rb) return false;
			
			// Test axis L = boxA.AxisX x boxB.AxisZ
			ra = boxA.HalfExtentY * AbsR.M33 + boxA.HalfExtentZ * AbsR.M23;
			rb = boxB.HalfExtentX * AbsR.M12 + boxB.HalfExtentY * AbsR.M11;
			if (Math.Abs(t.Z * R.M23 - t.Y * R.M33) > ra + rb) return false;
			
			// Test axis L = boxA.AxisY x boxB.AxisX
			ra = boxA.HalfExtentX * AbsR.M31 + boxA.HalfExtentZ * AbsR.M11;
			rb = boxB.HalfExtentY * AbsR.M23 + boxB.HalfExtentZ * AbsR.M22;
			if (Math.Abs(t.X * R.M31 - t.Z * R.M11) > ra + rb) return false;

			// Test axis L = boxA.AxisY x boxB.AxisY
			ra = boxA.HalfExtentX * AbsR.M32 + boxA.HalfExtentZ * AbsR.M12;
			rb = boxB.HalfExtentX * AbsR.M23 + boxB.HalfExtentZ * AbsR.M21;
			if (Math.Abs(t.X * R.M32 - t.Z * R.M12) > ra + rb) return false;
			
			// Test axis L = boxA.AxisY x boxB.AxisZ
			ra = boxA.HalfExtentX * AbsR.M33 + boxA.HalfExtentZ * AbsR.M13;
			rb = boxB.HalfExtentX * AbsR.M22 + boxB.HalfExtentY * AbsR.M21;
			if (Math.Abs(t.X * R.M33 - t.Z * R.M13) > ra + rb) return false;
			
			// Test axis L = boxA.AxisZ x boxB.AxisX
			ra = boxA.HalfExtentX * AbsR.M21 + boxA.HalfExtentY * AbsR.M11;
			rb = boxB.HalfExtentY * AbsR.M33 + boxB.HalfExtentZ * AbsR.M32;
			if (Math.Abs(t.Y * R.M11 - t.X * R.M21) > ra + rb) return false;
			
			// Test axis L = boxA.AxisZ x boxB.AxisY
			ra = boxA.HalfExtentX * AbsR.M22 + boxA.HalfExtentY * AbsR.M12;
			rb = boxB.HalfExtentX * AbsR.M33 + boxB.HalfExtentZ * AbsR.M31;
			if (Math.Abs(t.Y * R.M12 - t.X * R.M22) > ra + rb) return false;
			
			// Test axis L = boxA.AxisZ x boxB.AxisZ
			ra = boxA.HalfExtentX * AbsR.M23 + boxA.HalfExtentY * AbsR.M13;
			rb = boxB.HalfExtentX * AbsR.M32 + boxB.HalfExtentY * AbsR.M31;
			if (Math.Abs(t.Y * R.M13 - t.X * R.M23) > ra + rb) return false;

			// No separating axis found, must be intersecting.
			return true;
		}

		// Private helpers
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		private static float At(Vector3 v, uint index)
		{
			switch (index)
			{
				case 1: return v.X;
				case 2: return v.Y;
				case 3: return v.Z;
				default: throw new ArgumentException("Not fulfilled: 1 <= index <= 3");
			}
		}

		private static void MakeAbsEpsilonMatrix(ref Matrix3x3 matrix, out Matrix3x3 result)
		{
			result.M11 = Math.Abs(matrix.M11) + EPSILON;
			result.M12 = Math.Abs(matrix.M12) + EPSILON;
			result.M13 = Math.Abs(matrix.M13) + EPSILON;
			result.M21 = Math.Abs(matrix.M21) + EPSILON;
			result.M22 = Math.Abs(matrix.M22) + EPSILON;
			result.M23 = Math.Abs(matrix.M23) + EPSILON;
			result.M31 = Math.Abs(matrix.M31) + EPSILON;
			result.M32 = Math.Abs(matrix.M32) + EPSILON;
			result.M33 = Math.Abs(matrix.M33) + EPSILON;
		}
	}
}
