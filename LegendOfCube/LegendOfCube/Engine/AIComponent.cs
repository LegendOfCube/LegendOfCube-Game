using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine
{
	public class AIComponent
	{
		// Entities will move between the waypoints in order.
		public Vector3[] waypoints;
		public int lastWaypoint;
		private bool patrolling;
		private PatrolDirection dir;

		public AIComponent(Vector3[] waypoints, bool patrolling)
		{
			this.waypoints = waypoints;
			lastWaypoint = 0;
			this.patrolling = patrolling;
			dir = PatrolDirection.FORTH;
		}

		public int getNextWayPoint() {
			if (waypoints.Length == 2)
				return (lastWaypoint == 0 ? 1 : 0);

			if (PatrolDirection.BACK == dir)
			{
				if (lastWaypoint == 0)
				{
					changeDirection(); // Should not be here
					return 1;
				}
				return (lastWaypoint - 1);
			}
			else // Going forth
				return ((lastWaypoint + 1) % waypoints.Length);
		}

		public void changeDirection()
		{
			dir = (dir == PatrolDirection.FORTH ? PatrolDirection.BACK : PatrolDirection.FORTH);
		}

		public void setDirection(PatrolDirection dir)
		{
			this.dir = dir;
		}

		public PatrolDirection getDirection()
		{
			return dir;
		}

		public bool isPatrolling()
		{
			return patrolling;
		}

		public enum PatrolDirection
		{
			BACK,
			FORTH
		};
	}
}
