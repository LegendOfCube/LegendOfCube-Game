using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LegendOfCube.Engine
{
	class GameplaySystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Movement constants
		private const float MOVEMENT_ACCELERATION = 15.0f;

		// Ground jump constants
		private const float MAX_JUMP_HEIGHT = 6.0f;
		private const float MIN_JUMP_HEIGHT = 4.0f;
		private const float MAX_DECISION_HEIGHT = 1.0f;

		// Variables
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Vector3 currentMovementVelocity = Vector3.Zero;
		private Vector3 targetMovementVelocity = Vector3.Zero;
		
		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ProcessInputData(World world, float delta)
		{
			UInt32 i = world.Player.Id;
			if (!world.EntityProperties[i].Satisfies(Properties.TRANSFORM |
			                                         Properties.INPUT |
			                                         Properties.ACCELERATION |
			                                         Properties.VELOCITY))
			{
				Debug.Assert(false);
			}

			// Ground jump constants (that depend on gravity in some way)
			float MIN_JUMP_SPEED = (float)Math.Sqrt(-2.0f * MIN_JUMP_HEIGHT * world.Gravity.Y);
			float MAX_JUMP_SPEED = (float)Math.Sqrt(-2.0f * (MAX_JUMP_HEIGHT - MAX_DECISION_HEIGHT) * world.Gravity.Y);
			float JUMP_DECISION_ACCELERATION = ((MAX_JUMP_SPEED * MAX_JUMP_SPEED - MIN_JUMP_SPEED * MIN_JUMP_SPEED) / (2 * MAX_DECISION_HEIGHT)) - world.Gravity.Y;
			float MAX_DECISION_TIME = (-MIN_JUMP_SPEED / world.Gravity.Y) - (float)Math.Sqrt(((4.0f * MIN_JUMP_SPEED*MIN_JUMP_SPEED)/(JUMP_DECISION_ACCELERATION*JUMP_DECISION_ACCELERATION)) + (2*MAX_DECISION_HEIGHT/world.Gravity.Y));

			// Movement
			{
				// If cubes total velocity is less than currentMovementVelocity adjust currentMovementVelocity accordingly
				if (Math.Abs(world.Velocities[i].X) < Math.Abs(currentMovementVelocity.X)) currentMovementVelocity.X = world.Velocities[i].X;
				if (Math.Abs(world.Velocities[i].Z) < Math.Abs(currentMovementVelocity.Z)) currentMovementVelocity.Z = world.Velocities[i].Z;

				// Remove currentMovementVelocity from cubes total velocity
				world.Velocities[i].X -= currentMovementVelocity.X;
				world.Velocities[i].Z -= currentMovementVelocity.Z;

				// Calculate targetMovementVelocity
				Vector2 inputDir = world.InputData[i].GetDirection();
				Vector3 rotatedInputDir = Rotate2DDirectionRelativeCamera(world, ref inputDir);
				if (inputDir.Length() > 0.05f) targetMovementVelocity = rotatedInputDir * world.MaxSpeed[i];
				else targetMovementVelocity = Vector3.Zero;

				// Move currentMovementVelocity towards target velocity
				Vector3 dirToTarget = targetMovementVelocity - currentMovementVelocity;
				if (dirToTarget != Vector3.Zero) // Only mess with currentMovementVelocity if necessary
				{
					dirToTarget.Normalize();
					currentMovementVelocity += dirToTarget * MOVEMENT_ACCELERATION * delta;

					// If we passed targetMovementVelocity we clamp to it.
					if (Vector3.Dot(targetMovementVelocity - currentMovementVelocity, dirToTarget) <= 0)
					{
						currentMovementVelocity = targetMovementVelocity;
					}
				}

				// Add currentMovementVelocity to cubes total velocity
				world.Velocities[i].X += currentMovementVelocity.X;
				world.Velocities[i].Z += currentMovementVelocity.Z;
			}

			/*for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.EntityProperties[i].Satisfies(MOVEMENT_INPUT)) continue;
				// Updates velocities according to input
				//TODO: Make it better
				// Movement
				if(world.InputData[i].GetDirection().Length() <= 0.01 && world.PlayerCubeState.OnGround)
				{
					if (!isStopping)
					{
						stopTimeLeft = world.StopTime;
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

					Vector2 inputDir = world.InputData[i].GetDirection();
					Vector3 rotatedInput = Rotate2DDirectionRelativeCamera(world, ref inputDir);

					if (!world.PlayerCubeState.OnGround)
					{
						world.Accelerations[i] = new Vector3(
							rotatedInput.X*world.MaxAcceleration[i]*world.AirMovement,
							0,
							rotatedInput.Z*world.MaxAcceleration[i]*world.AirMovement
							);

					}
					else
					{
						world.Accelerations[i] = new Vector3(
							rotatedInput.X*world.MaxAcceleration[i],
							0,
							rotatedInput.Z*world.MaxAcceleration[i]
							);
					}

					if (world.PlayerCubeState.OnWall)
					{
						float wallAxisAcc = Vector3.Dot(world.Accelerations[i], world.PlayerCubeState.WallAxis);
						world.Accelerations[i] -= wallAxisAcc*world.PlayerCubeState.WallAxis;
						world.Velocities[i] -= 5.0f*world.PlayerCubeState.WallAxis;
					}
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
					if (world.InputData[i].NewJump() && world.PlayerCubeState.OnWall)
					{
						world.Velocities[i].Y = 0;
						world.Velocities[i] += world.PlayerCubeState.WallAxis * 20;
						world.Velocities[i].Y += 1f*world.BaseJump;
						world.PlayerCubeState.OnWall = false;
						world.PlayerCubeState.OnGround = false;
					}
					//If the jump button is pressed and the cube is on the ground initiate new jump
					else if (world.InputData[i].NewJump() && world.PlayerCubeState.OnGround)
					{
						world.Velocities[i].Y = world.BaseJump;
						jumpTimeLeft = JUMP_TIME;
						world.PlayerCubeState.OnGround = false;
					}
					//If the player is mid jump apply more jumpspeed
					else if (world.InputData[i].IsJumping() && jumpTimeLeft > 0)
					{
						world.Velocities[i].Y += 1.5f*world.BaseJump*delta;
						jumpTimeLeft -= delta;
					}
				}
			}*/

			SetCubeColor(world, world.Player.Id);
		}

		// Private functions: Helpers
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Vector3 Rotate2DDirectionRelativeCamera(World world, ref Vector2 direction)
		{
			Vector3 cameraDiff = world.CameraPosition - world.Transforms[world.Player.Id].Translation;
		
			// Calculate angle formed along ground by the cameras position relative the player
			float offset = (float)Math.Atan2(cameraDiff.X, cameraDiff.Z);

			// Invert y input
			direction.Y = -direction.Y;

			// Rotate in 3D, since don't have 2x2 matrix class
			Vector3 directionInput3D = new Vector3(direction.X, 0, direction.Y);
			Vector3 rotatedInput = Vector3.Transform(directionInput3D, Matrix.CreateRotationY(offset));
			return rotatedInput;
		}

		private void SetCubeColor(World world, UInt32 i)
		{
			// Color cube sides if on wall
			var playerEffect = world.StandardEffectParams[i];
			var cubeState = world.PlayerCubeState;

			Color newColor;
			if (cubeState.OnWall) newColor = Color.OrangeRed;
			else if (cubeState.OnGround) newColor = Color.Cyan;
			else newColor = Color.ForestGreen;
			float speed = world.Velocities[i].Length();
			float brightness = MathUtils.ClampLerp(speed, 0.2f, 1.0f, 0.0f, world.MaxSpeed[i]);
			playerEffect.EmissiveColor = (newColor * brightness).ToVector4();
		}
	}
}
