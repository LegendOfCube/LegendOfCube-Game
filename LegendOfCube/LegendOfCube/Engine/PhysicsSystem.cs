﻿using System;
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
					world.Velocities[i] += (world.Gravity * delta);
				}

				// Update position
				if (properties.Satisfies(MOVABLE))
				{
					Vector3 diff = (world.Velocities[i] * delta);
					worldSpaceOBBs[i].Position += diff;

					// Iterate until object no longer intersects with anything.
					float timeLeft = delta;
					UInt32 intersectionId = intersectionId = findIntersection(world, i);
					while (intersectionId != UInt32.MaxValue)
					{
						worldSpaceOBBs[i].Position -= diff;
						float timeUntilCol = findTimeUntilIntersection(intersectionId, i, world.Velocities[i], timeLeft);
						diff = (world.Velocities[i]*timeUntilCol);
						worldSpaceOBBs[i].Position += diff;
						intersectionId = UInt32.MaxValue;
						//timeLeft = findTimeUntilIntersection


						//intersectionId = findIntersection(world, i);
					}

					// Update translation in transform
					world.Transforms[i].Translation = worldSpaceOBBs[i].Position; 



					// Calculate new transform
					/*Vector3 newTranslation = world.Transforms[i].Translation + (world.Velocities[i]*delta);
					Matrix newTransform = world.Transforms[i];
					newTransform.Translation = newTranslation;

					OBB worldSpaceOBB = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref newTransform);

					// Searches for intersections
					UInt32 collisionIndex = UInt32.MaxValue;
					OBB collisionBox = new OBB();
					for (UInt32 j = 0; j <= world.HighestOccupiedId; j++)
					{
						if (i == j) continue;
						if (!world.EntityProperties[j].Satisfies(Properties.MODEL_SPACE_BV | Properties.TRANSFORM)) continue;
						collisionBox = OBB.TransformOBB(ref world.ModelSpaceBVs[j], ref world.Transforms[j]);

						if (collisionBox.Intersects(ref worldSpaceOBB))
						{
							collisionIndex = j;
							break;
						}
					}

					if (collisionIndex == UInt32.MaxValue) // No collision occured
					{
						world.Transforms[i] = newTransform;
					}
					else // Collision occured
					{
						CollisionEvent ce = new CollisionEvent(new Entity(i), new Entity(collisionIndex));
						world.EventBuffer.AddEvent(ref ce);
						//Special physics related collision events
						if (world.EntityProperties[collisionIndex].Satisfies((new Properties(Properties.BOUNCE_FLAG))))
						{
							if (world.InputData[i].NewJump())
							{
								world.Velocities[i] *= -1.5f;
							}
							else
							{
								world.Velocities[i] *= -1;	
							}
						}
						else
						{
							OBB worldOBBPre = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref world.Transforms[i]);
							Vector3 axis = findCollisionAxis(ref collisionBox, ref worldOBBPre, ref worldSpaceOBB);

							Debug.WriteLine("Axis: " + axis + "\n\n");

							float collidingSum = Vector3.Dot(world.Velocities[i], axis);
							world.Velocities[i] -= (collidingSum*axis);

							newTranslation = world.Transforms[i].Translation + (world.Velocities[i]*delta);
							world.Transforms[i].Translation = newTranslation;
							world.PlayerCubeState.InAir = false; // Super ugly hack, but neat.
						}
					}*/

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
		private float findTimeUntilIntersection(UInt32 target, UInt32 collider, Vector3 colliderVelocity, float timeSlice)
		{
			float time = timeSlice / 2.0f;

			for (int itr = 0; itr < 2; itr++)
			{
				Vector3 diff = colliderVelocity * time;
				worldSpaceOBBs[collider].Position += diff;

				// TODO: THIS FUNCTION IS NOT DONE!

			}

			return 0.0f;
		}

		private Vector3 findCollisionAxis(ref OBB target, ref OBB colliderPre, ref OBB colliderPost)
		{
			//Debug.Assert(!target.Intersects(ref colliderPre));
			//Debug.Assert(target.Intersects(ref colliderPost));

			// Note: This whole algorithm assumes that the collider is a cube.
			// If collider is not a cube it will probably not work well at all.

			// Calculate distance vector from target to collider
			Vector3 toCollider = colliderPost.Position - target.Position;

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
		}
	}
}
