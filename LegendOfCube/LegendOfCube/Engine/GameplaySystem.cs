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

		// Ground jump constants
		private const float MAX_JUMP_RELEASE_HEIGHT = 4.0f;
		private const float MIN_JUMP_RELEASE_HEIGHT = 1.5f;
		private const float JUMP_SPEED = 16.0f;
		private const float JUMP_SPEED_AT_APEX = 8.0f; // This value must be close to JUMP_SPEED, otherwise it will look like cube hits ceiling. A lower value will give more precise jumps.
		private const float MAX_JUMP_RELEASE_TIME = MAX_JUMP_RELEASE_HEIGHT / JUMP_SPEED;
		private const float MIN_JUMP_RELEASE_TIME = MIN_JUMP_RELEASE_HEIGHT / JUMP_SPEED;

		// Wall jump constants
		private const float WALL_JUMP_AXIS_SPEED = 30.0f;
		private const float WALL_JUMP_UP_SPEED = 30.0f;

		// Air movement 
		private static readonly Vector2 AIR_MOVEMENT_VELOCITY_DELTA = new Vector2(3.0f, 3.0f);

		// Variables
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private float jumpTime = 0.0f;
		private Vector3 jumpStartMovementVelocity = Vector3.Zero;

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

			// Clean up
			world.Velocities[i].X = 0.0f; // Hack
			world.Velocities[i].Z = 0.0f; // Hack
			
			// Reset many variables if Cube is on ground
			if (world.PlayerCubeState.OnGround)
			{
				jumpTime = 0.0f;
				jumpStartMovementVelocity = Vector3.Zero;
				world.JumpVelocities[i] = Vector3.Zero;
				world.Accelerations[i].Y = 0.0f;
			}

			// Movement input
			if (world.InputData[i].GetDirection().Length() > 0.01f)
			{
				Vector3 inputDir = RotateInputDirectionRelativeCamera(world, i);
				Vector3 inputVelocity = inputDir * world.MaxSpeed[i];
				if (world.PlayerCubeState.OnWall)
				{
					//inputVelocity -= Vector3.Dot(inputVelocity, world.PlayerCubeState.WallAxis) * world.PlayerCubeState.WallAxis;
				}
				else if (!world.PlayerCubeState.OnGround && !world.PlayerCubeState.OnWall)
				{
					Vector3 jumpMoveDir = jumpStartMovementVelocity;
					jumpMoveDir.Normalize();
					float dot = Vector3.Dot(inputDir, jumpMoveDir);
					if (dot < 0.0f) inputVelocity = Vector3.Zero;
				}
				world.MovementVelocities[i] = inputVelocity;
			}
			else
			{
				world.MovementVelocities[i] = Vector3.Zero;
			}

			// Jump
			if (world.InputData[i].NewJump()) // New jump
			{
				if (world.PlayerCubeState.OnGround)
				{
					world.JumpVelocities[i] = new Vector3(0, JUMP_SPEED, 0);
					world.Accelerations[i].Y = -world.Gravity.Y; // Anti-gravity
					jumpStartMovementVelocity = world.MovementVelocities[i];
					jumpTime = delta;
					world.PlayerCubeState.OnGround = false;
				}
				else if (world.PlayerCubeState.OnWall)
				{
					world.JumpVelocities[i] = world.PlayerCubeState.WallAxis * WALL_JUMP_AXIS_SPEED;
					world.JumpVelocities[i].Y += WALL_JUMP_UP_SPEED;
					world.Accelerations[i].Y = -world.Gravity.Y; // Anti-gravity
					if (world.JumpVelocities[i].Y > WALL_JUMP_UP_SPEED) world.JumpVelocities[i] = new Vector3(0, WALL_JUMP_UP_SPEED, 0);
					jumpStartMovementVelocity = world.MovementVelocities[i];
					jumpTime = delta;
					world.PlayerCubeState.OnWall = false;
				}
			}
			else if (jumpTime > 0.0f) // Jump in progress
			{
				jumpTime += delta;
				if (jumpTime > MAX_JUMP_RELEASE_TIME || (jumpTime > MIN_JUMP_RELEASE_TIME && !world.InputData[i].IsJumping()))
				{
					world.Velocities[i].Y += JUMP_SPEED_AT_APEX; // Add jump speed at apex to general velocity.
					jumpTime = 0.0f;
					world.JumpVelocities[i] = Vector3.Zero;
					world.Accelerations[i].Y = 0.0f;
				}
			}

			SetCubeColor(world, i);
			

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
		}

		// Private functions: Helpers
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Vector3 RotateInputDirectionRelativeCamera(World world, UInt32 i)
		{
			Vector2 direction = world.InputData[i].GetDirection();
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

			float speed = world.MovementVelocities[i].Length();
			float brightness = MathUtils.ClampLerp(speed, 0.2f, 1.0f, 0.0f, world.MaxSpeed[i]);

			playerEffect.EmissiveColor = (newColor * brightness).ToVector4();
		}
	}
}
