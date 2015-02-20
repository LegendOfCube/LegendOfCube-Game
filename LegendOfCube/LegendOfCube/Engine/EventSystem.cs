using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework;

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
				if (world.EntityProperties[collidedWith].Satisfies(new Properties(Properties.DEATH_ZONE_FLAG)))
				{
					world.Transforms[collider].Translation = world.SpawnPoint;
					world.Velocities[collider] = Vector3.Zero;
				}
			}
			world.EventBuffer.Flush();
		}
	}
}
