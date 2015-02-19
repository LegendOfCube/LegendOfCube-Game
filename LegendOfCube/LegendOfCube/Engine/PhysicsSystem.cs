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
					bool collisionOccured = false;
					UInt32 collisionIndex = 0;
					OBB collisionBox;
					for (UInt32 j = 0; j < world.HighestOccupiedId; j++)
					{
						if (i == j) continue;
						if (!world.EntityProperties[j].Satisfies(Properties.MODEL_SPACE_BV | Properties.TRANSFORM)) continue;
						collisionBox = OBB.TransformOBB(ref world.ModelSpaceBVs[j], ref world.Transforms[j]);

						if (collisionBox.Intersects(ref worldSpaceOBB))
						{
							collisionOccured = true;
							collisionIndex = j;
							Debug.WriteLine("collision at" + collisionBox.Position + " with " + worldSpaceOBB.Position);
							break;
						}
					}

					if (!collisionOccured) // No collision occured
					{
						world.Transforms[i] = newTransform;
					}
					else // Collision occured
					{
						// Temporary collision response, more correct would be reflecting
						// around normal vector (but we probably rather want sticking to the wall)
						world.Velocities[i] = -world.Velocities[i];

						// Visual effect for debugging
						if (world.EntityProperties[i].Satisfies(Properties.FULL_LIGHT_EFFECT))
						{
							world.StandardEffectParams[i].EmissiveColor = Vector4.One - world.StandardEffectParams[i].EmissiveColor;
						}
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
	}
}
