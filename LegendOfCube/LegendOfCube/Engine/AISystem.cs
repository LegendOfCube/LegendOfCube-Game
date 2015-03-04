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

		internal void update(World world, float delta)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(AI_CONTROLLED)) continue;

				AIComponent AI = world.AIComponents[i];
				Vector3 next = AI.waypoints[AI.getNextWayPoint()];

				if (Vector3.Dot((next - world.Transforms[i].Translation), world.Velocities[i]) <= 0)
				{
					
					// Whan patrolling, change direciton at the endpoints.
					if (AI.isPatrolling())
					{
						if (AI.getDirection() == AIComponent.PatrolDirection.FORTH)
						{
							if (AI.lastWaypoint == AI.waypoints.Length - 2)
								AI.changeDirection();
						}
						else
						{
							if (AI.lastWaypoint == 0)
								AI.changeDirection();
						}
					}

					// Reposition and set new velocity towards the next waypoint.
					world.Transforms[i].Translation = next;
					AI.lastWaypoint = Array.IndexOf(AI.waypoints, next);
					Vector3 newVel = AI.waypoints[AI.getNextWayPoint()] - next;
					newVel = Vector3.Normalize(newVel);
					world.Velocities[i] = world.Velocities[i].Length() * newVel;
				}
			}
		}
	}
}
