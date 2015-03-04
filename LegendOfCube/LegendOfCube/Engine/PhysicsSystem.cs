using System;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.BoundingVolumes;
using System.Diagnostics;
using LegendOfCube.Engine.Events;

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
					/*Vector2 groundVelocity = new Vector2(world.Velocities[i].X, world.Velocities[i].Z);
					if (groundVelocity.Length() > world.MaxSpeed[i])
					{
						groundVelocity.Normalize();
						groundVelocity *= world.MaxSpeed[i];
						world.Velocities[i].X = groundVelocity.X;
						world.Velocities[i].Z = groundVelocity.Y;
					}*/
				}

				// Apply gravity
				if (properties.Satisfies(HAS_GRAVITY))
				{
					world.Velocities[i] += (world.Gravity * delta);
				}

				// Update position
				if (properties.Satisfies(MOVABLE))
				{
					Vector3 oldPosition = worldSpaceOBBs[i].Position;
					Vector3 diff = (world.Velocities[i] * delta);
					worldSpaceOBBs[i].Position += diff;

					// Iterate until object no longer intersects with anything.
					float timeLeft = delta;
					UInt32 intersectionId = intersectionId = findIntersection(world, i);
					int iterations = 0;
					while (intersectionId != UInt32.MaxValue)
					{
						/*if (iterations >= 10) break;
						iterations++;
						Debug.Assert(intersectionId != i);

						// Collision axis
						Vector3 axis = findCollisionAxis(ref worldSpaceOBBs[intersectionId], ref worldSpaceOBBs[i]);

						// Move OBB to collision point
						worldSpaceOBBs[i].Position -= diff;
						float timeUntilCol = findTimeUntilIntersection(ref worldSpaceOBBs[intersectionId],
						                     ref worldSpaceOBBs[i], world.Velocities[i], timeLeft, 2);
						diff = world.Velocities[i] * timeUntilCol;
						worldSpaceOBBs[i].Position += diff;

						// Add Collision Event to EventBuffer
						CollisionEvent ce = new CollisionEvent(new Entity(i), new Entity(intersectionId), axis);
						world.EventBuffer.AddEvent(ref ce);

						// Update timeLeft
						timeLeft -= timeUntilCol;
						if (timeLeft < 0.0f) break;

						// Collision response
						float collidingSum = Vector3.Dot(world.Velocities[i], axis);
						world.Velocities[i] -= (collidingSum * axis);
						world.PlayerCubeState.InAir = false; // Super ugly hack, but neat.
						//Debug.Assert(!worldSpaceOBBs[i].Intersects(ref worldSpaceOBBs[intersectionId]));

						// Attempt to move for remaining time
						diff = world.Velocities[i] * timeLeft;
						worldSpaceOBBs[i].Position += diff;

						// Do it all again
						//intersectionId = UInt32.MaxValue;
						intersectionId = findIntersection(world, i);*/

						Vector3 axis = findCollisionAxis(ref worldSpaceOBBs[intersectionId], ref worldSpaceOBBs[i]);
						pushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId], ref axis);
						world.PlayerCubeState.InAir = false;

						intersectionId = findIntersection(world, i);
					}

					/*while (intersectionId != UInt32.MaxValue)
					{
						pushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId]);

						intersectionId = findIntersection(world, i);
					}


					// Small hack, basically we rather want the cube to stop completely than to intersect for now.
					if (findIntersection(world, i) != UInt32.MaxValue)
					{
						//worldSpaceOBBs[i].Position = oldPosition;
					}*/

					// Update translation in transform
					Vector3 obbDiff = worldSpaceOBBs[i].Position - oldPosition;
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

		private Vector3 findCollisionAxis(ref OBB target, ref OBB colliderPost)
		{
			return Vector3.UnitY;
		}

		/*private Vector3 findCollisionAxis(ref OBB target, ref Vector3 colliderPos)
		{
			// Note: This whole algorithm assumes that the collider is a cube.
			// If collider is not a cube it will probably not work well at all.

			// Calculate distance vector from target to collider
			Vector3 toCollider = colliderPos - target.Position;

			// Projects distance vector on each of targets axes
			float toColliderXProj = Vector3.Dot(toCollider, target.AxisX);
			float toColliderYProj = Vector3.Dot(toCollider, target.AxisY);
			float toColliderZProj = Vector3.Dot(toCollider, target.AxisZ);

			// Abs value of projections
			float toColliderXProjAbs = Math.Abs(toColliderXProj);
			float toColliderYProjAbs = Math.Abs(toColliderYProj);
			float toColliderZProjAbs = Math.Abs(toColliderZProj);

			// Checks which axes collider is outside of
			bool xOutside = toColliderXProjAbs > target.HalfExtentX;
			bool yOutside = toColliderYProjAbs > target.HalfExtentY;
			bool zOutside = toColliderZProjAbs > target.HalfExtentZ;

			// Counts how many axes collider is outside of
			int outsideCount = 0;
			if (xOutside) outsideCount++;
			if (yOutside) outsideCount++;
			if (zOutside) outsideCount++;

			// If outside of 2 or more axes we "don't have" (lol) a collision axis.
			if (outsideCount >= 2) return Vector3.Zero;

			// Return collision axis.
			if (yOutside) return Math.Sign(toColliderYProj) * target.AxisY;
			if (xOutside) return Math.Sign(toColliderXProj) * target.AxisX;
			if (zOutside) return Math.Sign(toColliderZProj) * target.AxisZ;

			// If no collision axis it means we're inside an object. Default to not being able to move in world y-axis.
			//Debug.Assert(false);
			return Vector3.UnitY;
		}*/

		private Vector3 findClosestOBBAxis(ref OBB obb, ref Vector3 direction)
		{
			float xDot = Vector3.Dot(direction, obb.AxisX);
			float yDot = Vector3.Dot(direction, obb.AxisY);
			float zDot = Vector3.Dot(direction, obb.AxisZ);
			float xDotAbs = Math.Abs(xDot);
			float yDotAbs = Math.Abs(yDot);
			float zDotAbs = Math.Abs(zDot);

			if (yDotAbs >= xDotAbs && yDotAbs >= zDotAbs)
			{
				return ((float)Math.Sign(yDot)) * obb.AxisY;
			}
			else if (xDotAbs >= yDotAbs && xDotAbs >= zDotAbs)
			{
				return ((float)Math.Sign(xDot)) * obb.AxisX;
			}
			else if (zDotAbs >= xDotAbs && zDotAbs >= yDotAbs)
			{
				return ((float)Math.Sign(zDot)) * obb.AxisZ;
			}

			Debug.Assert(false);
			return Vector3.Zero;
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

			float timeUntilCol = findTimeUntilIntersection(ref target, ref collider, -axisOut * accumulatedStep, 1.0f, 5);
			collider.Position += (timeUntilCol * accumulatedStep * -axisOut);
		}
	}
}
