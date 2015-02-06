using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class GameplaySystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
																				Properties.INPUT_FLAG);
		private static readonly float ACCELERATION = 30;

		public void processInputData(World world)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;

				// Updates velocities according to input
				//TODO: Make it better
                // Movement
				world.Accelerations[i] = new Vector3(world.InputData[i].GetDirection().X * ACCELERATION, 0, -world.InputData[i].GetDirection().Y * ACCELERATION);
				/*if (world.Accelerations[i].Length() > ACCELERATION)
				{
					Vector2 temp = new Vector2(world.Accelerations[i].X, world.Accelerations[i].Z);
					temp.Normalize();
					temp *= ACCELERATION;
					world.Accelerations[i].X = temp.X;
					world.Accelerations[i].Z = temp.Y;
				}*/
				// Jumping
                if (world.InputData[i].IsJumping()) world.Accelerations[i].Y = 18.0f;

			}

		}
	}
}
