using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LegendOfCube.Engine
{
	class MovementSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Movement constants
		private const float MOVEMENT_ACCELERATION = 35.0f;
		private const float MOVEMENT_AIR_ACCELERATION = 10.0f;
		private const float WALL_ANTI_GRAVITY_FACTOR = 0.75f;

		// Ground jump constants
		private const float MAX_JUMP_HEIGHT = 5.0f;
		private const float MIN_JUMP_HEIGHT = 1.5f;
		private const float MAX_DECISION_HEIGHT = 1.5f;

		// Wall jump constants
		private const float WALL_JUMP_MIN_OUT_SPEED = 4.0f;
		private const float WALL_JUMP_MAX_OUT_SPEED = 8.0f;

		// Variables
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Vector3 currentMovementVelocity = Vector3.Zero;
		private Vector3 targetMovementVelocity = Vector3.Zero;
		private float jumpTime = 0.0f;
		
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
			float MAX_DECISION_TIME = MAX_DECISION_HEIGHT / (((MAX_JUMP_SPEED - MIN_JUMP_SPEED) / 2.0f) + MIN_JUMP_SPEED);
			float JUMP_DECISION_ACCELERATION = ((MAX_JUMP_SPEED - MIN_JUMP_SPEED) / MAX_DECISION_TIME);

			// Wall jump constants
			float WALL_JUMP_DECISION_OUT_ACCELERATION = ((WALL_JUMP_MAX_OUT_SPEED - WALL_JUMP_MIN_OUT_SPEED) / MAX_DECISION_TIME);

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
					float movementAcc = (world.PlayerCubeState.OnGround || world.PlayerCubeState.OnWall) ? MOVEMENT_ACCELERATION : MOVEMENT_AIR_ACCELERATION;
					currentMovementVelocity += dirToTarget * movementAcc * delta;

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

			// WALL SUCK HACK
			if (world.PlayerCubeState.OnWall)
			{
				float wallAxisVel = Vector3.Dot(world.Velocities[i], world.PlayerCubeState.WallAxis);
				world.Velocities[i] -= wallAxisVel * world.PlayerCubeState.WallAxis;
				world.Velocities[i] -= 2.5f * world.PlayerCubeState.WallAxis;
			}

			// WALL ANTI-GRAVITY HACK
			if (world.PlayerCubeState.OnWall)
			{
				world.Velocities[i].Y += (-world.Gravity.Y) * delta * WALL_ANTI_GRAVITY_FACTOR;
			}

			// Jumping
			{
				if (world.InputData[i].NewJump())
				{
					if (world.PlayerCubeState.OnGround) // Ground jump
					{
						world.Velocities[i].Y += MIN_JUMP_SPEED;
						if (world.Velocities[i].Y < MIN_JUMP_SPEED) world.Velocities[i].Y = MIN_JUMP_SPEED;
						world.Accelerations[i] = new Vector3(0.0f, JUMP_DECISION_ACCELERATION - world.Gravity.Y, 0.0f);
						jumpTime = delta;
					}
					else if (world.PlayerCubeState.OnWall) // Wall jump
					{
						Vector3 wallAxis = world.PlayerCubeState.WallAxis;
						world.Velocities[i].Y += MIN_JUMP_SPEED;
						if (world.Velocities[i].Y < MIN_JUMP_SPEED) world.Velocities[i].Y = MIN_JUMP_SPEED;
						world.Accelerations[i] = new Vector3(0.0f, JUMP_DECISION_ACCELERATION - world.Gravity.Y, 0.0f);
						world.Velocities[i] -= (Vector3.Dot(world.Velocities[i], wallAxis)) * wallAxis;
						world.Velocities[i] += wallAxis * WALL_JUMP_MIN_OUT_SPEED;
						world.Accelerations[i] += wallAxis * WALL_JUMP_DECISION_OUT_ACCELERATION;
						jumpTime = delta;
					}
				}
				else if (jumpTime > 0.0f)
				{
					jumpTime += delta;
					if (jumpTime > MAX_DECISION_TIME || !world.InputData[i].IsJumping())
					{
						jumpTime = 0.0f;
						world.Accelerations[i] = Vector3.Zero;
					}
				}
			}
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
	}
}
