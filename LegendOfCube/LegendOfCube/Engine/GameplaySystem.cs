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

		public void processInputData(World world)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;

				// Updates velocities according to input
				//TODO: Make it better
                // Movement
				world.Velocities[i] = new Vector3(world.InputData[i].GetDirection().X * 10, world.Velocities[i].Y, -world.InputData[i].GetDirection().Y * 10);
				// Jumping
                if (world.InputData[i].IsJumping()) world.Velocities[i].Y = 8.0f;

				Vector3 pos = world.Transforms[i].Translation;
				Vector3 vel = world.Velocities[i];
				if (vel.Length() > 0.5f)
				{
					vel.Normalize();
					float angle = (float)Math.Atan2(vel.Z, -vel.X) + (float)(Math.PI / 2);
					Matrix.CreateRotationY(angle, out world.Transforms[i]);
					world.Transforms[i].Translation = pos;
				}
			}

		}
	}
}
