using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class GameplaySystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
		                                                                        Properties.INPUT_FLAG |
		                                                                        Properties.ACCELERATION |
		                                                                        Properties.VELOCITY);
		// TODO: make stop_time a function of the velocity
		private const float STOP_TIME = 0.3f;
		private const float JUMP_TIME = 0.5f;
		private float jumpTimeLeft = 1f;
		private bool isStopping = false;
		private float stopTimeLeft;
		private const float BASE_JUMP = 12f;

		public void ProcessInputData(World world, float delta)
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

					// Rotate input

					Vector3 cameraDiff = world.CameraPosition - world.Transforms[world.Player.Id].Translation;

					// Calculate angle formed along ground by the cameras position relative the player
					float offset = (float)Math.Atan2(cameraDiff.X, cameraDiff.Z);

					Vector2 directionInput = world.InputData[i].GetDirection();

					// Invert y input
					directionInput.Y = -directionInput.Y;

					// Rotate in 3D, since don't have 2x2 matrix class
					Vector3 directionInput3D = new Vector3(directionInput.X, 0, directionInput.Y);
					Vector3 rotatedInput = Vector3.Transform(directionInput3D, Matrix.CreateRotationY(offset));

					world.Accelerations[i] = new Vector3(
					    rotatedInput.X * world.MaxAcceleration[i],
					    0,
					    rotatedInput.Z * world.MaxAcceleration[i]
					);
				}

				Vector3 pos = world.Transforms[i].Translation;
				Vector3 vel = world.Velocities[i];
				if (vel.Length() > 0.5f)
				{
					vel.Normalize();
					float angle = (float)Math.Atan2(vel.Z, -vel.X) + (float)(Math.PI / 2);
					Matrix.CreateRotationY(angle, out world.Transforms[i]);
					world.Transforms[i].Translation = pos;
				}

				// Jumping
				if (world.InputData[i].IsJumping())
				{
					//If the jump button is pressed and the cube is on the ground initiate new jump
					if (world.InputData[i].NewJump() && world.PlayerCubeState.OnGround)
					{
						world.Velocities[i].Y = BASE_JUMP;
						jumpTimeLeft = JUMP_TIME;
						world.PlayerCubeState.OnGround = false;
					}
					//If the player is mid jump apply more jumpspeed
					else if (world.InputData[i].IsJumping() && jumpTimeLeft > 0)
					{
						world.Velocities[i].Y += 1.5f*BASE_JUMP*delta;
						jumpTimeLeft -= delta;
					}
				}
			}
		}
	}
}
