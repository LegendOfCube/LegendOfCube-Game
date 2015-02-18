using System;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.BoundingVolumes;
using System.Diagnostics;

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

		public void ApplyPhysics(float delta, World world)
		{
			for (UInt32 i = 0; i < world.HighestOccupiedId; i++)
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
					// Calculate new transform
					Vector3 newTranslation = world.Transforms[i].Translation + (world.Velocities[i]*delta);
					if (newTranslation.Y < 0) // UGLY FLOOR HACK
					{
						newTranslation.Y = 0;
						world.Velocities[i].Y = 0.0f;
						world.PlayerCubeState.InAir = false;
					}
					Matrix newTransform = world.Transforms[i];
					newTransform.Translation = newTranslation;

					OBB worldSpaceOBB = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref newTransform);

					// Searches for intersections
					UInt32 collisionIndex = UInt32.MaxValue;
					OBB collisionBox = new OBB();
					for (UInt32 j = 0; j < world.HighestOccupiedId; j++)
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
						OBB worldOBBPre = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref world.Transforms[i]);
						Vector3 axis = findCollisionAxis(ref collisionBox, ref worldOBBPre, ref worldSpaceOBB);

						Debug.WriteLine("Axis: " + axis);

						float collidingSum = Vector3.Dot(world.Velocities[i], axis);
						world.Velocities[i] -= (collidingSum * axis);

						newTranslation = world.Transforms[i].Translation + (world.Velocities[i] * delta);
						world.Transforms[i].Translation = newTranslation;
						world.PlayerCubeState.InAir = false; // Super ugly hack, but neat.

					}

				}
				else if (properties.Satisfies(MOVABLE_NO_BV))
				{
					world.Transforms[i].Translation += (world.Velocities[i] * delta);
					// Hacky floor
					if (world.Transforms[i].Translation.Y < 0)
					{
						Vector3 translation = world.Transforms[i].Translation;
						translation.Y = 0.0f;
						world.Transforms[i].Translation = translation;
						world.Velocities[i].Y = 0.0f;
						//Reset air state
						world.PlayerCubeState.InAir = false;
					}
				}
			}
		}

		private static Vector3 findCollisionAxis(ref OBB target, ref OBB colliderPre, ref OBB colliderPost)
		{
			Debug.Assert(!target.Intersects(ref colliderPre));
			Debug.Assert(target.Intersects(ref colliderPost));

			Vector3 movement = colliderPost.Position - colliderPre.Position;
			float xDot = Vector3.Dot(movement, target.AxisX);
			float yDot = Vector3.Dot(movement, target.AxisY);
			float zDot = Vector3.Dot(movement, target.AxisZ);

			// Making wild guess that the largest dot corresponds to the axis we're after.
			// This is obviously not correct as I can make up scenarios in my mind where
			// this will fail, but it might be good enough for our purposes.
			float xDotAbs = Math.Abs(xDot);
			float yDotAbs = Math.Abs(yDot);
			float zDotAbs = Math.Abs(zDot);
			Vector3 axis = new Vector3();
			if (xDotAbs >= yDotAbs && xDotAbs >= zDotAbs) axis = target.AxisX;
			else if (yDotAbs >= xDotAbs && yDotAbs >= zDotAbs) axis = target.AxisY;
			else if (zDotAbs >= xDotAbs && zDotAbs >= yDotAbs) axis = target.AxisZ;
			else Debug.Assert(false);

			// Now that we have the axis we just want to know the sign.
			Vector3 targetToCollider = colliderPre.Position - target.Position;
			float sign = Vector3.Dot(targetToCollider, axis);

			return Math.Sign(sign) * axis;
		}
	}
}
