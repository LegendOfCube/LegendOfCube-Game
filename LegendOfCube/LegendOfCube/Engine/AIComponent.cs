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
			return (lastWaypoint == 0 ? 1 : 0);
		}

		private enum PatrolDirection
		{
			BACK,
			FORTH
		};
	}
}
