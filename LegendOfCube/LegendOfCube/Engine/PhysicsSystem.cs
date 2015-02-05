﻿using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        private static readonly Properties ACCELERATABLE = new Properties(
                                                                       Properties.VELOCITY |
                                                                       Properties.ACCELERATION);

        private static readonly Properties HAS_GRAVITY = new Properties(
                                                                 Properties.VELOCITY |
                                                                 Properties.GRAVITY_FLAG);

		private static readonly Properties MOVABLE = new Properties(
		                                                         Properties.TRANSFORM |
		                                                         Properties.VELOCITY);

        private static readonly Vector3 GRAVITY = new Vector3(0.0f, -9.82f, 0.0f);

		public void ApplyPhysics(float delta, World world)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
                Properties properties = world.EntityProperties[i];
               
                // Check if velocity should be updated
                if (properties.Satisfies(ACCELERATABLE))
                {
                    world.Velocities[i] += (world.Accelerations[i] * delta);
                }

                // Apply gravity
                if (properties.Satisfies(HAS_GRAVITY))
                {
                    world.Velocities[i] += (GRAVITY * delta);
                }

                // Update position
                if (properties.Satisfies(MOVABLE))
                {
                    world.Transforms[i].Translation += (world.Velocities[i] * delta);

					// Hacky floor
					if (world.Transforms[i].Translation.Y < 0)
					{
						Vector3 translation = world.Transforms[i].Translation;
						translation.Y = 0.0f;
						world.Transforms[i].Translation = translation;
						world.Velocities[i].Y = 0.0f;
					}
                }
			}
		}
	}
}
