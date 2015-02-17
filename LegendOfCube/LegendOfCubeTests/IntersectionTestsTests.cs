using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LegendOfCubeTests
{
	[TestClass]
	public class IntersectionTestsTests
	{
		[TestMethod]
		public void TestOBBvsOBB()
		{
			OBB obb1 = new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));
			OBB obb2 = new OBB(new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));
			OBB obb3 = new OBB(new Vector3(2.2f, 2.2f, 2.2f), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));

			Assert.IsTrue(obb1.Intersects(ref obb2));
			Assert.IsTrue(obb2.Intersects(ref obb3));
			Assert.IsFalse(obb1.Intersects(ref obb3));
		}

		[TestMethod]
		public void TestOBBTransform()
		{
			OBB obb1 = new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1));

			Matrix identity = Matrix.Identity;
			OBB test1 = OBB.TransformOBB(ref obb1, ref identity);
			Assert.IsTrue(approxEqu(ref test1, ref obb1));

			Matrix scale = Matrix.CreateScale(2.0f);
			OBB test2 = OBB.TransformOBB(ref obb1, ref scale);
			Assert.IsTrue(approxEqu(obb1.Position, test2.Position));
			Assert.IsTrue(approxEqu(obb1.AxisX, test2.AxisX));
			Assert.IsTrue(approxEqu(obb1.AxisY, test2.AxisY));
			Assert.IsTrue(approxEqu(obb1.AxisZ, test2.AxisZ));
			Assert.IsTrue(approxEqu(test2.ExtentX, 2.0f));
			Assert.IsTrue(approxEqu(test2.ExtentY, 2.0f));
			Assert.IsTrue(approxEqu(test2.ExtentZ, 2.0f));
			
			Vector3 translPos = new Vector3(2, 4, 1);
			Matrix transl = Matrix.CreateTranslation(translPos);
			OBB test3 = OBB.TransformOBB(ref obb1, ref transl);
			Assert.IsTrue(approxEqu(translPos, test3.Position));
			Assert.IsTrue(approxEqu(obb1.AxisX, test3.AxisX));
			Assert.IsTrue(approxEqu(obb1.AxisY, test3.AxisY));
			Assert.IsTrue(approxEqu(obb1.AxisZ, test3.AxisZ));
			Assert.IsTrue(approxEqu(test3.ExtentX, 1.0f));
			Assert.IsTrue(approxEqu(test3.ExtentY, 1.0f));
			Assert.IsTrue(approxEqu(test3.ExtentZ, 1.0f));
		}

		private const float EPSILON = 0.001f;

		private bool approxEqu(float lhs, float rhs)
		{
			return lhs <= rhs + EPSILON && lhs >= rhs - EPSILON;
		}

		private bool approxEqu(Vector3 lhs, Vector3 rhs)
		{
			if (!approxEqu(lhs.X, rhs.X)) return false;
			if (!approxEqu(lhs.Y, rhs.Y)) return false;
			if (!approxEqu(lhs.Z, rhs.Z)) return false;
			return true;
		}

		private bool approxEqu(ref OBB lhs, ref OBB rhs)
		{
			if (!approxEqu(lhs.Position, rhs.Position)) return false;
			if (!approxEqu(lhs.AxisX, rhs.AxisX)) return false;
			if (!approxEqu(lhs.AxisY, rhs.AxisY)) return false;
			if (!approxEqu(lhs.AxisZ, rhs.AxisZ)) return false;
			if (!approxEqu(lhs.HalfExtents, rhs.HalfExtents)) return false;
			return true;
		}
	}
}
