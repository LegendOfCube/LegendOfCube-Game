using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.BoundingVolumes
{
	/// <summary>
	/// Specifies an Axis-Aligned Bounding-Box.
	/// </summary>
	struct AABB
	{
		// Private members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private BoundingBox box;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public AABB(Vector3 min, Vector3 max)
		{
			if (min.X >= max.X) throw new ArgumentException("min.X >= max.X");
			if (min.Y >= max.Y) throw new ArgumentException("min.Y >= max.Y");
			if (min.Z >= max.Z) throw new ArgumentException("min.Z >= max.Z");
			box = new BoundingBox(min, max);
		}

		public AABB(Vector3 centerPos, float xExtent, float yExtent, float zExtent)
		{
			if (xExtent <= 0) throw new ArgumentException("xExtent <= 0");
			if (yExtent <= 0) throw new ArgumentException("yExtent <= 0");
			if (zExtent <= 0) throw new ArgumentException("zExtent <= 0");

			float xHalfExtent = xExtent / 2.0f;
			float yHalfExtent = yExtent / 2.0f;
			float zHalfExtent = zExtent / 2.0f;

			Vector3 min = centerPos;
			min.X = min.X - xHalfExtent;
			min.Y = min.Y - yHalfExtent;
			min.Z = min.Z - zHalfExtent;
			
			Vector3 max = centerPos;
			max.X = max.X + xHalfExtent;
			max.Y = max.Y + yHalfExtent;
			max.Z = max.Z + zHalfExtent;

			box = new BoundingBox(min, max);
		}

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool Intersects(ref AABB other)
		{
			bool result;
			this.box.Intersects(ref other.box, out result);
			return result;
		}

		// Public properties
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public Vector3 Min
		{
			get
			{
				return box.Min;
			}
			set
			{
				this.box.Min = value;
			}
		}

		public Vector3 Max
		{
			get
			{
				return box.Max;
			}
			set
			{
				this.box.Max = value;
			}
		}

		public Vector3 CenterPos
		{
			get
			{
				Vector3 pos = box.Min;
				pos.X = pos.X + (ExtentX / 2.0f);
				pos.Y = pos.Y + (ExtentY / 2.0f);
				pos.Z = pos.Z + (ExtentZ / 2.0f);
				return pos;
			}
		}

		public float ExtentX
		{
			get
			{
				return box.Max.X - box.Min.X;
			}
		}

		public float ExtentY
		{
			get
			{
				return box.Max.Y - box.Min.Y;
			}
		}

		public float ExtentZ
		{
			get
			{
				return box.Max.Z - box.Min.Z;
			}
		}
	}
}
