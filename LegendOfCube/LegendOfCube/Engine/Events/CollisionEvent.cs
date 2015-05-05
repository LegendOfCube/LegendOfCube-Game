using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Events
{
	public struct CollisionEvent
	{
		public Entity Collider;
		public Entity CollidedWith;
		public Vector3 Axis;
		public Vector3 ColliderVelocity;

		public CollisionEvent(Entity collider, Entity collidedWith, Vector3 axis, Vector3 colliderVelocity)
		{
			Collider = collider;
			CollidedWith = collidedWith;
			Axis = axis;
			ColliderVelocity = colliderVelocity;
		}
	}
}
