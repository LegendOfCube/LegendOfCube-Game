using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Events;

namespace LegendOfCube.Engine
{
	class EventSystem
	{
		public void HandleEvents(World world)
		{
			EventBuffer eventBuffer = world.EventBuffer;
			foreach (var collisionEvent in eventBuffer.CollisionEvents)
			{
				var collidedWith = collisionEvent.CollidedWith.Id;
				var collider = collisionEvent.Collider.Id;
				/* Example Collision handling
				if (world.EntityProperties[collidedWith].Satisfies(Properties.MODEL) && 
					world.EntityProperties[collider].Satisfies(Properties.ACCELERATION))
				{
					world.Velocities[collider].Y += 100;
				}
				 */
			}
			world.EventBuffer.Flush();
		}
	}
}
