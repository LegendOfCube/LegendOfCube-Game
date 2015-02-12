using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
{
	class CollisionDetection
	{
		private const float EPSILON = 0.0001f;

		public static bool Intersects(ref AABB boxA, ref AABB boxB)
		{
			return boxA.Intersects(ref boxB);
		}

		public static bool Intersects(ref OBB boxA, ref OBB boxB)
		{
			// We're using SAT (Separating Axis Theorem). There are 15 axes we need to test to determine if intersection occurred.

			// This thing seems insane, it isn't.
			// Given two change of basis matrices, Ua and Ub (equal to that axises in boxA and B).
			// bToASpace == transpose(Ua) * Ub == inverse(Ua) * Ub == worldToA * bToWorld
			// inverse(Ua) == transpose(Ua) because Ua (and Ub) are orthogonal matrices.
			Matrix bToA = Matrix.CreateScale(1.0f); // TODO: Trololo identity matrix hack, might not be necessary.
			bToA.M11 = Vector3.Dot(boxA.AxisX, boxB.AxisX);
			bToA.M12 = Vector3.Dot(boxA.AxisX, boxB.AxisY);
			bToA.M13 = Vector3.Dot(boxA.AxisX, boxB.AxisZ);
			bToA.M21 = Vector3.Dot(boxA.AxisY, boxB.AxisX);
			bToA.M22 = Vector3.Dot(boxA.AxisY, boxB.AxisY);
			bToA.M23 = Vector3.Dot(boxA.AxisY, boxB.AxisZ);
			bToA.M31 = Vector3.Dot(boxA.AxisZ, boxB.AxisX);
			bToA.M32 = Vector3.Dot(boxA.AxisZ, boxB.AxisY);
			bToA.M33 = Vector3.Dot(boxA.AxisZ, boxB.AxisZ);

			// Computes the translation vector between boxA and B then transforms into into A space
			// Second row is equal to: inverse(Ua) * translVec == transpose(Ua) * translVec
			Vector3 translVec = boxB.CenterPos - boxA.CenterPos;
			translVec = new Vector3(Vector3.Dot(translVec, boxA.AxisX), Vector3.Dot(translVec, boxA.AxisY), Vector3.Dot(translVec, boxA.AxisZ));

			// Common subexpressions apparently.
			// Epsilon term to counteract arithmetic errors when two edges are parallell
			Matrix bToAAbs = Matrix.CreateScale(1.0f); // TODO: indentity hack
			bToAAbs.M11 = Math.Abs(bToA.M11) + EPSILON;
			bToAAbs.M12 = Math.Abs(bToA.M12) + EPSILON;
			bToAAbs.M13 = Math.Abs(bToA.M13) + EPSILON;
			bToAAbs.M21 = Math.Abs(bToA.M21) + EPSILON;
			bToAAbs.M22 = Math.Abs(bToA.M22) + EPSILON;
			bToAAbs.M23 = Math.Abs(bToA.M23) + EPSILON;
			bToAAbs.M31 = Math.Abs(bToA.M31) + EPSILON;
			bToAAbs.M32 = Math.Abs(bToA.M32) + EPSILON;
			bToAAbs.M33 = Math.Abs(bToA.M33) + EPSILON;

			// Tests all 15 axes in order of importance

			float radiusA, radiusB;

			// Axis Ax
			radiusA = boxA.HalfExtentX;
			radiusB = boxB.HalfExtentX * bToAAbs.M11 + boxB.HalfExtentY * bToAAbs.M12 + boxB.HalfExtentZ * bToAAbs.M13;
			if (Math.Abs(translVec.X) > radiusA + radiusB) return false;
			// Axis Ay
			radiusA = boxA.HalfExtentY;
			radiusB = boxB.HalfExtentX * bToAAbs.M21 + boxB.HalfExtentY * bToAAbs.M22 + boxB.HalfExtentZ * bToAAbs.M23;
			if (Math.Abs(translVec.Y) > radiusA + radiusB) return false;
			// Axis Az
			radiusA = boxA.HalfExtentZ;
			radiusB = boxB.HalfExtentX * bToAAbs.M31 + boxB.HalfExtentY * bToAAbs.M32 + boxB.HalfExtentZ * bToAAbs.M33;
			if (Math.Abs(translVec.Z) > radiusA + radiusB) return false;

			// Axis Bx
			radiusA = boxB.HalfExtentX;
			radiusB = boxB.HalfExtentX;


			return true;
		}
	}
}
