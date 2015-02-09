using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine.BoundingVolumes
{
	class CollisionDetection
	{
		public static bool Intersects(ref AABB boxA, ref AABB boxB)
		{
			return boxA.Intersects(ref boxB);
		}
	}
}
