using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Events
{
	public struct CollisionEvent
	{
		public Entity Collider;
		public Entity CollidedWith;
		public Vector3 Axis;

		public CollisionEvent(Entity collider, Entity collidedWith, Vector3 axis)
		{
			Collider = collider;
			CollidedWith = collidedWith;
			Axis = axis;
		}
	}
}
