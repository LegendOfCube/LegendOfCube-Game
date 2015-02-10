﻿using Microsoft.Xna.Framework;
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
						Vector2 antiVelocity = new Vector2(world.Velocities[i].X, world.Velocities[i].Z);
						antiVelocity /= -stopTimeLeft;
						world.Accelerations[i].X = antiVelocity.X;
						world.Accelerations[i].Z = antiVelocity.Y;
					}
					else
					{
						world.Accelerations[i] = new Vector3(0,world.Accelerations[i].Y,0);
					}
				}
				else
				{
					isStopping = false;
					world.Accelerations[i] = new Vector3(world.InputData[i].GetDirection().X * world.MaxAcceleration[i], 
						0, -world.InputData[i].GetDirection().Y * world.MaxAcceleration[i]);
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
				if (world.InputData[i].IsJumping())
				{
					//Limitation: only the player can jump.
					if (world.PlayerCubeState.CurrentJumps < PlayerCubeState.MAXJUMPS)
					{
						world.Velocities[i].Y = 8.0f;
						world.PlayerCubeState.CurrentJumps++;
					}
				}
				// For testing, set a light source right above the player
				// TODO: Remove at some point
				world.LightPosition = world.Transforms[i].Translation + 1.5f * Vector3.Up;
			}
		}
	}
}
