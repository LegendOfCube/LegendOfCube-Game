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
		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private readonly OBB[] worldSpaceOBBs;

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
			// Flush away old CollisionEvents
			world.EventBuffer.Flush();

			// Updating OBBs
			UpdateWorldSpaceOBBs(world);

			for (UInt32 i = 0; i <= world.HighestOccupiedId; i++)
			{
				// Acceleration
				if (world.EntityProperties[i].Satisfies(Properties.VELOCITY | Properties.ACCELERATION))
				{
					Accelerate(world, i, delta);
				}

				// Gravity
				if (world.EntityProperties[i].Satisfies(Properties.VELOCITY | Properties.GRAVITY_FLAG))
				{
					ApplyGravity(world, i, delta);
				}

				// Moving
				if (world.EntityProperties[i].Satisfies(Properties.TRANSFORM | Properties.VELOCITY | Properties.MODEL_SPACE_BV | Properties.DYNAMIC_VELOCITY_FLAG))
				{
					MoveDynamicWithCollisionChecking(world, i, delta);
				}
				else if (world.EntityProperties[i].Satisfies(Properties.TRANSFORM | Properties.VELOCITY | Properties.MODEL_SPACE_BV))
				{
					MoveStaticWithCollisionChecking(world, i, delta);
				}
				else if (world.EntityProperties[i].Satisfies(Properties.TRANSFORM | Properties.VELOCITY))
				{
					MoveWithoutCollisionChecking(world, i, delta);
				}
			}
		}

		// Private functions: Segments
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private void UpdateWorldSpaceOBBs(World world)
		{
			for (UInt32 i = 0; i <= world.HighestOccupiedId; i++)
			{
				if (!world.EntityProperties[i].Satisfies(Properties.MODEL_SPACE_BV | Properties.TRANSFORM)) continue;
				worldSpaceOBBs[i] = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref world.Transforms[i]);
			}
		}

		private void Accelerate(World world, UInt32 i, float delta)
		{
			world.Velocities[i] += (world.Accelerations[i] * delta);
			if (world.EntityProperties[i].Satisfies(Properties.INPUT))
			{
				world.Velocities[i] += (world.InputAccelerations[i] * delta);
			}

			// Clamp velocity in X and Y direction
			// TODO: REMOVE! THIS SHOULD NOT BE IN PHYSICS SYSTEM. Besides, current implementation too naive.
			/*Vector2 groundVelocity = new Vector2(world.Velocities[i].X, world.Velocities[i].Z);
			if (groundVelocity.Length() > world.MaxSpeed[i])
			{
				groundVelocity.Normalize();
				groundVelocity *= world.MaxSpeed[i];
				world.Velocities[i].X = groundVelocity.X;
				world.Velocities[i].Z = groundVelocity.Y;
			}*/
		}

		private void ApplyGravity(World world, UInt32 i, float delta)
		{
			// Player moves slower when on wall.
			// TODO: REMOVE! THIS SHOULD NOT BE IN PHYSICS SYSTEM.
			/*if (world.PlayerCubeState.OnWall)
			{
				world.Velocities[i] += (world.Gravity * delta * 0.5f);
			}
			else
			{*/
				world.Velocities[i] += (world.Gravity * delta);
			//}
		}
		
		private void MoveDynamicWithCollisionChecking(World world, UInt32 i, float delta)
		{
			OBB oldObb = worldSpaceOBBs[i];
			Vector3 diff = Velocity(world, i) * delta;
			worldSpaceOBBs[i].Position += diff;

			// Iterate until object no longer intersects with anything.
			float timeLeft = delta;
			UInt32 intersectionId = FindIntersection(world, i);
			int iterations = 0;
			while (intersectionId != UInt32.MaxValue)
			{
				if (iterations >= 10) break;
				iterations++;

				// Calculate collision axis and rotate collider
				Vector3 collisionAxis = FindCollisionAxis(ref worldSpaceOBBs[intersectionId], ref oldObb);
				RotateOBB(ref worldSpaceOBBs[i], ref collisionAxis);

				// Add Collision Event to EventBuffer
				CollisionEvent ce = new CollisionEvent(new Entity(i), new Entity(intersectionId), collisionAxis, Velocity(world, i));
				world.EventBuffer.AddEvent(ref ce);

				// Move OBB to collision point
				worldSpaceOBBs[i].Position -= diff;
				float timeUntilCol = FindTimeUntilIntersection(ref worldSpaceOBBs[intersectionId],
									 ref worldSpaceOBBs[i], Velocity(world, i), timeLeft, 2);
				diff = Velocity(world, i) * timeUntilCol;
				worldSpaceOBBs[i].Position += diff;

				// Update timeLeft
				timeLeft -= timeUntilCol;
				if (timeLeft <= 0.0f) break;

				// Remove velocity in colliding axis
				RemoveVelocityInAxis(world, i, ref collisionAxis);

				// Move in remaining velocity for the remaining time and then check for more intersections
				diff = Velocity(world, i) * timeLeft;
				worldSpaceOBBs[i].Position += diff;
				intersectionId = FindIntersection(world, i);
			}

			// Attempt to resolve remaining intersections
			PushOutEntityCollisionAxis(world, i, 15);
			PushOutEntityFixedAxis(world, i, Vector3.UnitY, 25);

			TransformFromOBBs(ref oldObb, ref worldSpaceOBBs[i], ref world.Transforms[i]);
		}

		private void MoveStaticWithCollisionChecking(World world, UInt32 i, float delta)
		{
			OBB oldObb = worldSpaceOBBs[i];
			worldSpaceOBBs[i].Position += (Velocity(world, i) * delta);

			// Iterate until object no longer intersects with anything.
			UInt32 intersectionId = FindIntersection(world, i);
			int iterations = 0;
			while (intersectionId != UInt32.MaxValue)
			{
				if (iterations >= 10) break;
				iterations++;

				// Push out target
				Vector3 collisionAxisInv = -FindCollisionAxis(ref worldSpaceOBBs[intersectionId], ref oldObb);
				if (world.EntityProperties[intersectionId].Satisfies(Properties.DYNAMIC_VELOCITY_FLAG))
				{
					PushOutEntityFixedAxis(world, intersectionId, collisionAxisInv, 10);
				}

				// Add Collision Event to EventBuffer
				CollisionEvent ce = new CollisionEvent(new Entity(i), new Entity(intersectionId), collisionAxisInv, Velocity(world, i));
				world.EventBuffer.AddEvent(ref ce);

				intersectionId = FindIntersection(world, i);
			}

			TransformFromOBBs(ref oldObb, ref worldSpaceOBBs[i], ref world.Transforms[i]);
		}

		private void MoveWithoutCollisionChecking(World world, UInt32 i, float delta)
		{
			world.Transforms[i].Translation += (Velocity(world, i) * delta);
		}

		// Private functions: Helpers
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Precondition: param entity must satisfy MOVABLE
		// Returns UInt32.MaxValue if no intersections are found, otherwise index of entity collided with.
		private UInt32 FindIntersection(World world, UInt32 entity)
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
		private static float FindTimeUntilIntersection(ref OBB target, ref OBB collider, Vector3 colliderVelocity, float timeSlice, int maxIterations)
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

		private Vector3 FindCollisionAxis(ref OBB target, ref OBB collider)
		{
			Vector3 colliderPos = collider.Position;
			Vector3 closestTargetPoint = target.ClosestPointOnOBB(ref colliderPos);
			if (MathUtils.ApproxEqu(colliderPos, closestTargetPoint, 0.001f)) return Vector3.UnitY;
			Vector3 resultDir = colliderPos - closestTargetPoint;
			return target.ClosestAxis(ref resultDir);
		}

		private void PushOut(ref OBB collider, ref OBB target, ref Vector3 axisOut)
		{
			float averageHalfExtent = (collider.HalfExtentX + collider.HalfExtentY + collider.HalfExtentZ) / 3.0f;
			float stepSize = averageHalfExtent / 2.0f;
			float accumulatedStep = 0.0f;

			while (collider.Intersects(ref target))
			{
				collider.Position += axisOut * stepSize;
				accumulatedStep += stepSize;
			}

			float timeUntilCol = FindTimeUntilIntersection(ref target, ref collider, -axisOut * accumulatedStep, 1.0f, 4);
			collider.Position += (timeUntilCol * accumulatedStep * -axisOut);
		}

		private void PushOutEntityCollisionAxis(World world, UInt32 i, int maxNumIterations)
		{
			int iterations = 0;
			UInt32 intersectionId = FindIntersection(world, i);
			while (intersectionId != UInt32.MaxValue)
			{
				if (iterations > maxNumIterations) break;
				iterations++;
				Vector3 axis = FindCollisionAxis(ref worldSpaceOBBs[intersectionId], ref worldSpaceOBBs[i]);
				PushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId], ref axis);
				intersectionId = FindIntersection(world, i);
			}
		}

		private void PushOutEntityFixedAxis(World world, UInt32 i, Vector3 axis, int maxNumIterations)
		{
			int iterations = 0;
			UInt32 intersectionId = FindIntersection(world, i);
			while (intersectionId != UInt32.MaxValue)
			{
				if (iterations > maxNumIterations) break;
				iterations++;
				PushOut(ref worldSpaceOBBs[i], ref worldSpaceOBBs[intersectionId], ref axis);
				intersectionId = FindIntersection(world, i);
			}
		}

		private void RotateOBB(ref OBB obbOut, ref Vector3 axis)
		{
			OBBAxis colAxisEnum = obbOut.ClosestAxisEnum(ref axis);
			switch (colAxisEnum)
			{
				case OBBAxis.X_PLUS:
				case OBBAxis.X_MINUS:
					obbOut.AxisX = colAxisEnum.Sign() * axis;
					obbOut.AxisY = Vector3.Cross(obbOut.AxisZ, obbOut.AxisX);
					obbOut.AxisZ = Vector3.Cross(obbOut.AxisX, obbOut.AxisY);
					break;
				case OBBAxis.Y_PLUS:
				case OBBAxis.Y_MINUS:
					obbOut.AxisY = colAxisEnum.Sign() * axis;
					obbOut.AxisX = Vector3.Cross(obbOut.AxisY, obbOut.AxisZ);
					obbOut.AxisZ = Vector3.Cross(obbOut.AxisX, obbOut.AxisY);
					break;
				case OBBAxis.Z_PLUS:
				case OBBAxis.Z_MINUS:
					obbOut.AxisZ = colAxisEnum.Sign() * axis;
					obbOut.AxisX = Vector3.Cross(obbOut.AxisY, obbOut.AxisZ);
					obbOut.AxisY = Vector3.Cross(obbOut.AxisZ, obbOut.AxisX);
					break;
			}
		}

		private void TransformFromOBBs(ref OBB oldOBB, ref OBB newOBB, ref Matrix transformOut)
		{
			// Update translation in transform
			Vector3 obbDiff = newOBB.Position - oldOBB.Position;
			transformOut.Translation += obbDiff;
			// Update rotation: This is probably a really stupid way.
			transformOut.Forward = newOBB.AxisZ * transformOut.Forward.Length();
			transformOut.Left = newOBB.AxisX * transformOut.Left.Length();
			transformOut.Up = newOBB.AxisY * transformOut.Up.Length();
		}

		private Vector3 Velocity(World world, UInt32 i)
		{
			Vector3 vel = world.Velocities[i];
			if (world.EntityProperties[i].Satisfies(Properties.INPUT))
			{
				vel += world.InputVelocities[i];
			}
			return vel;
		}

		private void RemoveVelocityInAxis(World world, UInt32 i, ref Vector3 axis)
		{
			float collidingSum = Vector3.Dot(world.Velocities[i], axis);
			world.Velocities[i] -= (collidingSum * axis);
			if (world.EntityProperties[i].Satisfies(Properties.INPUT))
			{
				collidingSum = Vector3.Dot(world.InputVelocities[i], axis);
				world.InputVelocities[i] -= (collidingSum * axis);
			}
		}
	}
}
