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
			// We're using SAT (Separating Axis Theorem). There are 15 axes we need to test to determine if intersection occurred.

			Matrix3x3 aToWorld = Matrix3x3.CreateChangeOfBasis(boxA.AxisX, boxA.AxisY, boxA.AxisZ);
			Matrix3x3 bToWorld = Matrix3x3.CreateChangeOfBasis(boxB.AxisX, boxB.AxisY, boxB.AxisZ);
			Matrix3x3 worldToA;
			Matrix3x3.Transpose(ref aToWorld, out worldToA); // Transpose is equal to inverse since orthogonal matrix
			Matrix3x3 worldToB;
			Matrix3x3.Transpose(ref bToWorld, out worldToB);

			Matrix3x3 bToA;
			Matrix3x3.Multiply(ref worldToA, ref bToWorld, out bToA);
			Matrix3x3 aToB;
			Matrix3x3.Multiply(ref worldToB, ref aToWorld, out aToB);

			// Epsilon term to counteract arithmetic errors when two edges are parallell
			Matrix3x3 bToAAbs;
			MakeAbsEpsilonMatrix(ref bToA, out bToAAbs);
			Matrix3x3 aToBAbs;
			MakeAbsEpsilonMatrix(ref aToB, out aToBAbs);

			// Computes translation vector between boxA and B and transforms it into A space
			Vector3 translVec = boxB.Position - boxA.Position;
			Vector3 translVecA;
			Matrix3x3.Transform(ref worldToA, ref translVec, out translVecA);

			// Test all 15 axes in order of importance
			float radiusA, radiusB;

			// Axes Ax, Ay and Az
			for (uint i = 1; i <= 3; i++)
			{
				radiusA = At(boxA.HalfExtents, i);
				radiusB = Vector3.Dot(bToAAbs.RowAt(i), boxB.HalfExtents);
				if (Math.Abs(At(translVecA, i)) > radiusA + radiusB) return false;
			}

			// Axes Bx, By and Bz
			for (uint i = 1; i <= 3; i++ )
			{
				radiusA = Vector3.Dot(aToBAbs.RowAt(i), boxA.HalfExtents);
				radiusB = At(boxB.HalfExtents, i);
				if (Math.Abs(Vector3.Dot(aToB.RowAt(i), translVecA)) > radiusA + radiusB) return false;
			}

			// TODO: 9 more axes to test
			// This means that about 15% of the time intersection will be reported when it's not actually happening.
			// Test is still conservative.

			// Since we made it this far we have not disproven intersection, so it might be true.
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
