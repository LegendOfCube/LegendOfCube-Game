using Microsoft.Xna.Framework;
using System;

namespace LegendOfCube.Engine
{
	class GameplaySystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
																				Properties.INPUT_FLAG
																				| Properties.ACCELERATION
																				| Properties.VELOCITY);
		private static readonly float ACCELERATION = 30;
		// TODO: make stop_time a function of the velocity
		private static readonly float STOP_TIME = 1f;
		private bool isStopping = false;
		private float stopTimeLeft;

		public void processInputData(World world, float delta)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;
				// Updates velocities according to input
				//TODO: Make it better
				// Movement
				if(world.InputData[i].GetDirection().Length() <= 0.01)
				{
					if (!isStopping)
					{
						stopTimeLeft = STOP_TIME;
					}
					else
					{
						stopTimeLeft -= delta;
						if (stopTimeLeft < 0)
						{
							stopTimeLeft = 0;
						}
					}
					isStopping = true;
					if (stopTimeLeft != 0)
					{
						Vector3 antiVelocity = (-world.Velocities[i]) / stopTimeLeft;
						world.Accelerations[i] = antiVelocity;
					}
					else
					{
						world.Accelerations[i] = Vector3.Zero;
					}
				}
				else
				{
					isStopping = false;
					world.Accelerations[i] = new Vector3(world.InputData[i].GetDirection().X * ACCELERATION, 
						0, -world.InputData[i].GetDirection().Y * ACCELERATION);
				}

				
				/*if (world.Accelerations[i].Length() > ACCELERATION)
				{
					Vector2 temp = new Vector2(world.Accelerations[i].X, world.Accelerations[i].Z);
					temp.Normalize();
					temp *= ACCELERATION;
					world.Accelerations[i].X = temp.X;
					world.Accelerations[i].Z = temp.Y;
				}*/

				// Jumping
				if (world.InputData[i].IsJumping()) world.Velocities[i].Y = 8.0f;

				// For testing, set a light source right above the player
				// TODO: Remove at some point
				world.LightPosition = world.Transforms[e.Id].Translation + 1.5f * Vector3.Up;
			}
		}
	}
}
