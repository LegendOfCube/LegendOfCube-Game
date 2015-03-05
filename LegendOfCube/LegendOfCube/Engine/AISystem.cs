using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class AISystem
	{
		private static readonly Properties AI_CONTROLLED = new Properties(Properties.TRANSFORM |
		                                                                  Properties.AI_FLAG |
		                                                                  Properties.VELOCITY);

		public AISystem()
		{

		}

		internal void Update(World world, float delta)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(AI_CONTROLLED)) continue;

				AIComponent ai = world.AIComponents[i];
				Vector3 next = ai.waypoints[ai.GetNextWayPoint()];

				if (Vector3.Dot((next - world.Transforms[i].Translation), world.Velocities[i]) <= 0)
				{
					
					// Whan patrolling, change direciton at the endpoints.
					if (ai.IsPatrolling())
					{
						if (ai.GetDirection() == AIComponent.PatrolDirection.FORTH)
						{
							if (ai.lastWaypoint == ai.waypoints.Length - 2)
								ai.ChangeDirection();
						}
						else
						{
							if (ai.lastWaypoint == 0)
								ai.ChangeDirection();
						}
					}

					// Reposition and set new velocity towards the next waypoint.
					world.Transforms[i].Translation = next;
					ai.lastWaypoint = Array.IndexOf(ai.waypoints, next);
					Vector3 newVel = ai.waypoints[ai.GetNextWayPoint()] - next;
					newVel = Vector3.Normalize(newVel);
					world.Velocities[i] = world.Velocities[i].Length() * newVel;
				}
			}
		}
	}
}
