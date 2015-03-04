namespace LegendOfCube.Engine.Events
{
	public struct CollisionEvent
	{
		public Entity Collider;
		public Entity CollidedWith;

		public CollisionEvent(Entity collider, Entity collidedWith)
		{
			Collider = collider;
			CollidedWith = collidedWith;
		}
	}
}
