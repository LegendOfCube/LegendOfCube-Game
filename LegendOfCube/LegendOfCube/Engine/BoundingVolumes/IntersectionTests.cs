using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
{
	public class IntersectionsTests
	{
		private const float EPSILON = 0.0001f;

		public static bool Intersects(ref AABB boxA, ref AABB boxB)
		{
			return boxA.Intersects(ref boxB);
		}

		public static bool Intersects(ref OBB boxA, ref OBB boxB)
		{
			// We're using SAT (Separating Axis Theorem). There are 15 axes we need to test to determine if intersection occurred.

			Matrix3x3 aToWorld = Matrix3x3.CreateChangeOfBasis(boxA.AxisX, boxA.AxisY, boxA.AxisZ);
			Matrix3x3 bToWorld = Matrix3x3.CreateChangeOfBasis(boxB.AxisX, boxB.AxisY, boxB.AxisZ);
			Matrix3x3 worldToA = aToWorld.Transpose(); // Transpose is equal to inverse since orthogonal matrix
			Matrix3x3 worldToB = bToWorld.Transpose();

			Matrix3x3 bToA = worldToA * bToWorld;
			Matrix3x3 aToB = worldToB * aToWorld;

			// Epsilon term to counteract arithmetic errors when two edges are parallell
			Matrix3x3 bToAAbs = new Matrix3x3();
			Matrix3x3 aToBAbs = new Matrix3x3();
			for (uint i = 1; i <= 3; i++)
			{
				for (uint j = 1; j <= 3; j++)
				{
					bToAAbs.Set(i, j, Math.Abs(bToA.At(i, j)) + EPSILON);
					aToBAbs.Set(i, j, Math.Abs(aToB.At(i, j)) + EPSILON);
				}
			}

			// Computes translation vector between boxA and B and transforms it into A space
			Vector3 translVecA = worldToA * (boxB.Position - boxA.Position);

			// Test all 15 axes in order of importance
			float radiusA, radiusB;

			// Axes Ax, Ay and Az
			for (uint i = 0; i < 3; i++)
			{
				radiusA = At(boxA.HalfExtents, i);
				radiusB = Vector3.Dot(bToAAbs.RowAt(i), boxB.HalfExtents);
				if (Math.Abs(At(translVecA, i)) > radiusA + radiusB) return false;
			}

			// Axes Bx, By and Bz
			for (uint i = 0; i < 3; i++ )
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
	}
}
