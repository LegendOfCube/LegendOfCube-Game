using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine.Events
{
	public class EventBuffer
	{
		public readonly List<CollisionEvent> CollisionEvents;

		public EventBuffer()
		{
			CollisionEvents = new List<CollisionEvent>(100);
		}

		public bool AddEvent(ref CollisionEvent e)
		{
			this.CollisionEvents.Add(e);
			return true;
		}

		public void Flush()
		{
			CollisionEvents.Clear();
		}
	}
}
