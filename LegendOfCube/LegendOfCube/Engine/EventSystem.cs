using System;
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
				if (world.EntityProperties[collidedWith].Satisfies((Properties.DEATH_ZONE_FLAG)))
				{
					world.Transforms[collider].Translation = world.SpawnPoint;
					world.Velocities[collider] = Vector3.Zero;
				}
				else if (world.EntityProperties[collidedWith].Satisfies(Properties.TELEPORT_FLAG))
				{
					while (true)
					{
						Random rnd = new Random();
						int temp = rnd.Next((int)world.HighestOccupiedId);
						if (world.EntityProperties[temp].Satisfies(Properties.TELEPORT_FLAG))
						{
							Vector3 temp2 = world.Transforms[temp].Translation;
							temp2.Y += 25;
							world.Transforms[collider].Translation = temp2;
							break;
						}
					}
				}
			}
			world.EventBuffer.Flush();
		}
	}
}
