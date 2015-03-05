using System;
using System.Diagnostics;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Events;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties ACCELERATABLE = new Properties(Properties.VELOCITY |
		                                                                  Properties.ACCELERATION);

		private static readonly Properties HAS_GRAVITY = new Properties(Properties.VELOCITY |
		                                                                Properties.GRAVITY_FLAG);

		private static readonly Properties MOVABLE = new Properties(Properties.TRANSFORM |
		                                                            Properties.VELOCITY |
		                                                            Properties.MODEL_SPACE_BV);

		private static readonly Properties MOVABLE_NO_BV = new Properties(Properties.TRANSFORM |
		                                                                  Properties.VELOCITY);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private OBB[] worldSpaceOBBs;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public PhysicsSystem(uint maxNumEntities)
		{
			worldSpaceOBBs = new OBB[maxNumEntities];
		}

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyPhysics(float delta, World world)
		{
			// Calculate all World Space OBBs
			for (UInt32 i = 0; i <= world.HighestOccupiedId; i++)
			{
				if (!world.EntityProperties[i].Satisfies(Properties.MODEL_SPACE_BV | Properties.TRANSFORM)) continue;
				worldSpaceOBBs[i] = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref world.Transforms[i]);
			}

			for (UInt32 i = 0; i <= world.HighestOccupiedId; i++)
			{
				Properties properties = world.EntityProperties[i];

				// Check if velocity should be updated
				if (properties.Satisfies(ACCELERATABLE))
				{
					world.Velocities[i] += (world.Accelerations[i] * delta);

					// Clamp velocity in X and Y direction
					Vector2 groundVelocity = new Vector2(world.Velocities[i].X, world.Velocities[i].Z);
					if (groundVelocity.Length() > world.MaxSpeed[i])
					{
						groundVelocity.Normalize();
						groundVelocity *= world.MaxSpeed[i];
						world.Velocities[i].X = groundVelocity.X;
						world.Velocities[i].Z = groundVelocity.Y;
					}
				}

				// Apply gravity
				if (properties.Satisfies(HAS_GRAVITY))
				{
					if (world.PlayerCubeState.OnWall)
					{
						world.Velocities[i] += (world.Gravity*delta*0.5f);
					}
					else
					{
						world.Velocities[i] += (world.Gravity*delta);
					}
				}

				// Update position
				if (properties.Satisfies(MOVABLE))
				{
					OBB oldObb = worldSpaceOBBs[i];
					Vector3 diff = (world.Velocities[i] * delta);
					worldSpaceOBBs[i].Position += diff;

					// Player specific collision response part 1: Reading PlayerCubeState
					bool playerInAir = false;
					bool playerOnWall = false;
					bool playerOnGround = false;
					Vector3 playerWallAxis = Vector3.Zero;
					Vector3 playerGroundAxis = Vector3.Zero;
					if (i == world.Player.Id)
					{
						playerInAir = world.PlayerCubeState.InAir;
						playerOnWall = world.PlayerCubeState.OnWall;
						playerOnGround = world.PlayerCubeState.OnGround;
						playerWallAxis = world.PlayerCubeState.WallAxis;
						playerGroundAxis = world.PlayerCubeState.GroundAxis;

						if (Vector3.Dot(diff, -Vector3.UnitY) > 0.015f)
						{
							playerOnGround = false;
							playerGroundAxis = Vector3.Zero;
							playerInAir = true;
						}
					}

					// Iterate until object no longer intersects with anything.
					float timeLeft = delta;
					UInt32 intersectionId = findIntersection(world, i);
					int iterations = 0;
					while (intersectionId != UInt32.MaxValue)
					{
						if (iterations >= 10) break;
						iterations++;
						Debug.Assert(intersectionId != i);

						// Collision axis
						Vector3 axis = findCollisionAxis(ref worldSpaceOBBs[intersectionId], ref oldObb);
						Debug.Assert(!float.IsNaN(axis.X));

						// Add Collision Event to EventBuffer
						CollisionEvent ce = new CollisionEvent(new Entity(i), new Entity(intersectionId), axis, world.Velocities[i]);
						world.EventBuffer.AddEvent(ref ce);

						// Move OBB to collision point
						worldSpaceOBBs[i].Position -= diff;
						float timeUntilCol = findTimeUntilIntersection(ref worldSpaceOBBs[intersectionId],
						                     ref worldSpaceOBBs[i], world.Velocities[i], timeLeft, 2);
						diff = world.Velocities[i] * timeUntilCol;
						worldSpaceOBBs[i].Position += diff;

						// Update timeLeft
						timeLeft -= timeUntilCol;
						if (timeLeft <= 0.0f) break;

						// Collision response
						float collidingSum = Vector3.Dot(world.Velocities[i], axis);
						world.Velocities[i] -= (collidingSum * axis);

						// Player specific collision response part 2
						if (i == world.Player.Id)
						{
							float wallDotX = Math.Abs(Vector3.Dot(axis, Vector3.UnitX));
							float wallDotZ = Math.Abs(Vector3.Dot(axis, Vector3.UnitZ));
							float wallDot = wallDotX + wallDotZ;
							if (wallDot > 0.9f)
							{
								playerOnWall = true;
								playerWallAxis = worldSpaceOBBs[intersectionId].ClosestAxis(ref axis);
							}

							float groundDot = Vector3.Dot(axis, Vector3.UnitY);
							if (groundDot > 0.75f)
							{
								playerOnGround = true;
								playerInAir = false;
								playerGroundAxis = worldSpaceOBBs[intersectionId].ClosestAxis(ref axis);

								playerOnWall = false;
								playerWallAxis = Vector3.Zero;
							}
							world.PlayerCubeState.InAir = false; // Super ugly hack, but neat.
						}

						// Attempt to move for remaining time
						diff = world.Velocities[i] * timeLeft;
						worldSpaceOBBs[i].Position += diff;

						// Do it all again
						//intersectionId = UInt32.MaxValue;
						intersectionId = findIntersection(world, i);
					}

					// Attempt to resolve remaining intersections
					iterations = 0;
					while (intersectionId != UInt32.MaxValue)
					{
						if (iterations > 15) break;
						iterations++;
						Vector3 axis = findCollisionAxis(ref worldSpaceOBBs[intersectionId], ref worldSpaceOBBs[i]);
						pushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId], ref axis);
						intersectionId = findIntersection(world, i);
					}

					// Emergency step. If we haven't resolved collisions until now we just move collider to the top.
					iterations = 0;
					while (intersectionId != UInt32.MaxValue)
					{
						if (iterations > 100) break;
						iterations++;
						Vector3 emergencyAxis = Vector3.UnitY;
						pushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId], ref emergencyAxis);
						intersectionId = findIntersection(world, i);
					}

					// Player specific collision response part 3: Setting PlayerCubeState
					if (i == world.Player.Id)
					{
						world.PlayerCubeState.InAir = playerInAir;
						world.PlayerCubeState.OnWall = playerOnWall;
						world.PlayerCubeState.OnGround = playerOnGround;
						world.PlayerCubeState.WallAxis = playerWallAxis;
						world.PlayerCubeState.GroundAxis = playerGroundAxis;
					}

					// Update translation in transform
					Vector3 obbDiff = worldSpaceOBBs[i].Position - oldObb.Position;
					world.Transforms[i].Translation += obbDiff;
				}
				else if (properties.Satisfies(MOVABLE_NO_BV))
				{
					world.Transforms[i].Translation += (world.Velocities[i] * delta);
				}
			}
		}

		// Private functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Precondition: param entity must satisfy MOVABLE
		// Returns UInt32.MaxValue if no intersections are found, otherwise index of entity collided with.
		private UInt32 findIntersection(World world, UInt32 entity)
		{
			for (UInt32 i = 0; i <= world.HighestOccupiedId; i++)
			{
				if (i == entity) continue;
				if (!world.EntityProperties[i].Satisfies(Properties.MODEL_SPACE_BV | Properties.TRANSFORM)) continue;
				if (worldSpaceOBBs[i].Intersects(ref worldSpaceOBBs[entity])) return i;
			}

			// No collisions found.
			return UInt32.MaxValue;
		}

		// Returns an approximation of the maximum amount of time collider can move.
		// Collider is guaranteed to not collide with target if moved the amount of time returned.
		// Preconditions: timeSlice > 0, collider must hit target if moved entire timeSlice
		private float findTimeUntilIntersection(ref OBB target, ref OBB collider, Vector3 colliderVelocity, float timeSlice, int maxIterations)
		{
			Vector3 originalPosition = collider.Position;
			float currentTimeSlice = timeSlice;
			float time = 0.0f;

			for (int itr = 0; itr < maxIterations; itr++)
			{
				currentTimeSlice = currentTimeSlice / 2.0f;
				time += currentTimeSlice;
				Vector3 diff = colliderVelocity * time;
				collider.Position += diff;

				if (collider.Intersects(ref target))
				{
					time -= currentTimeSlice;
				}
				collider.Position -= diff;
			}

			collider.Position = originalPosition;
			return time;
		}

		private void gatherPoints(ref OBB obb, Vector3[] pointsOut)
		{
			obb.Corners(pointsOut);
			pointsOut[8] = obb.Position + (obb.HalfExtentX * obb.AxisX);
			pointsOut[9] = obb.Position - (obb.HalfExtentX * obb.AxisX);
			pointsOut[10] = obb.Position + (obb.HalfExtentY * obb.AxisY);
			pointsOut[11] = obb.Position - (obb.HalfExtentY * obb.AxisY);
			pointsOut[12] = obb.Position + (obb.HalfExtentZ * obb.AxisZ);
			pointsOut[13] = obb.Position - (obb.HalfExtentZ * obb.AxisZ);
			pointsOut[14] = obb.Position;
		}

		Vector3[] obbPointsCollider = new Vector3[15];

		private Vector3 findCollisionAxis(ref OBB target, ref OBB collider)
		{
			// Calculate points for the collider to test against target.
			gatherPoints(ref collider, obbPointsCollider);

			// Finds the closest point on the target OBB and which point on the
			// collider that was closest.
			Vector3 closestTargetPoint = Vector3.Zero;
			int colliderPointIndex = -1;
			float shortestDist = float.MaxValue;
			for (int iCol = 0; iCol < 15; iCol++)
			{
				Vector3 temp = target.ClosestPointOnOBB(ref obbPointsCollider[iCol]);
				float dist = (obbPointsCollider[iCol] - temp).Length();
				if (0.001f < dist && dist <= shortestDist)
				{
					shortestDist = dist;
					colliderPointIndex = iCol;
					closestTargetPoint = temp;
				}
			}

			// This happens if collider is completely inside target, will probably result in
			// the resulting axis being filled with NaN's.
			//Debug.Assert(colliderPointIndex != -1);
			if (colliderPointIndex == -1) return Vector3.UnitY;

			Vector3 resultDir = obbPointsCollider[colliderPointIndex] - closestTargetPoint;
			//resultDir.Normalize();
			//return resultDir;
			return target.ClosestAxis(ref resultDir);
		}

		/*private void pushOut(ref OBB collider, ref OBB target)
		{
			Vector3 colliderPos = collider.Position;
			Vector3 axisOut = findCollisionAxis(ref target, ref colliderPos);
			if (axisOut == Vector3.Zero)
			{
				Vector3 toCollider = collider.Position - target.Position;
				axisOut = findClosestOBBAxis(ref target, ref toCollider);
			}

			pushOut(ref collider, ref target, ref axisOut);
		}*/

		private void pushOut(ref OBB collider, ref OBB target, ref Vector3 axisOut)
		{
			float averageHalfExtent = (collider.HalfExtentX + collider.HalfExtentY + collider.HalfExtentZ) / 3.0f;
			float stepSize = averageHalfExtent / 2.0f;
			float accumulatedStep = 0.0f;

			while (collider.Intersects(ref target))
			{
				collider.Position += axisOut * stepSize;
				accumulatedStep += stepSize;
			}

			float timeUntilCol = findTimeUntilIntersection(ref target, ref collider, -axisOut * accumulatedStep, 1.0f, 4);
			collider.Position += (timeUntilCol * accumulatedStep * -axisOut);
		}
	}
}
