using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine.Events
{
	struct CollisionEvent
	{
		public Entity Collider;
		public Entity CollidedWith;
	}
}
