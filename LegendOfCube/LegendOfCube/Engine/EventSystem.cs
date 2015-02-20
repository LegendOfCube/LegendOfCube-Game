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
				if (world.EntityProperties[collisionEvent.Colided.Id].Satisfies(Properties.MODEL) && 
					world.EntityProperties[collisionEvent.Collider.Id].Satisfies(Properties.ACCELERATION)
				{
					
				}

			}
		}
	}
}
