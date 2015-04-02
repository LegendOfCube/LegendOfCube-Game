using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using LegendOfCube.Engine.BoundingVolumes;

namespace LegendOfCube.Engine
{
	class MovementSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Movement constants
		private const float MOVEMENT_ACCELERATION = 40.0f;
		private const float MOVEMENT_AIR_ACCELERATION = 15.0f;
		private const float WALL_ANTI_GRAVITY_FACTOR = 0.5f;
		private const float ROTATIONAL_SPEED = 360;
		private static readonly float ROTATIONAL_SPEED_RAD = MathHelper.ToRadians(ROTATIONAL_SPEED);

		// Ground jump constants
		private const float MAX_JUMP_HEIGHT = 7f;
		private const float MIN_JUMP_HEIGHT = 2f;
		private const float MAX_DECISION_HEIGHT = 2f;

		// Wall jump constants
		private const float WALL_JUMP_MIN_OUT_SPEED = 5.0f;
		private const float WALL_JUMP_MAX_OUT_SPEED = 8.0f;

		private const float MIN_WALL_JUMP_HEIGHT = 1f;

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
			float MIN_WALL_JUMP_SPEED = (float)Math.Sqrt(-2.0f * MIN_WALL_JUMP_HEIGHT * world.Gravity.Y);

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
				else if(world.PlayerCubeState.OnGround) targetMovementVelocity = Vector3.Zero;

				// Move currentMovementVelocity towards target velocity
				Vector3 dirToTarget = targetMovementVelocity - currentMovementVelocity;
				if (dirToTarget != Vector3.Zero) // Only mess with currentMovementVelocity if necessary
				{
					dirToTarget.Normalize();
					float movementAcc = (world.PlayerCubeState.OnGround) ? MOVEMENT_ACCELERATION : MOVEMENT_AIR_ACCELERATION;
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

			// Cube rotation
			if (currentMovementVelocity != Vector3.Zero)
			{
				Vector3 movementDir = currentMovementVelocity;
				movementDir.Normalize();

				OBB wsOBB = OBB.TransformOBB(ref world.ModelSpaceBVs[i], ref world.Transforms[i]);
				OBBAxis closestAxisEnum = wsOBB.ClosestAxisEnum(ref movementDir);
				Vector3 closestAxis = wsOBB.ClosestAxis(ref movementDir);
				closestAxis.Normalize();

				float angleBetween = angleRadBetweenTwoNormalizedVectors(ref movementDir, ref closestAxis);
				float angleToMove = ROTATIONAL_SPEED_RAD * delta;
				if (angleToMove > angleBetween) angleToMove = angleBetween;

				if (!MathUtils.ApproxEqu(closestAxis, movementDir, 0.01f))
				{
					Vector3 rotationAxis = Vector3.Cross(closestAxis, movementDir);
					rotationAxis.Normalize();
					Matrix3x3 rotationMatrix = Matrix3x3.CreateRotationMatrix(ref rotationAxis, angleToMove);

					OBB oldOBB = wsOBB;
					Vector3 rotatedAxis = rotationMatrix * closestAxis;
					RotateOBB(ref wsOBB, closestAxisEnum, ref rotatedAxis);

					TransformFromOBBs(ref oldOBB, ref wsOBB, ref world.Transforms[i]);
				}
			}

			// WALL SUCK HACK
			if (world.PlayerCubeState.OnWall)
			{
				float wallAxisVel = Vector3.Dot(world.Velocities[i], world.PlayerCubeState.WallAxis);
				world.Velocities[i] -= wallAxisVel * world.PlayerCubeState.WallAxis;

				//Reset velocity against wall
				float wallAxisCurVel = Vector3.Dot(currentMovementVelocity, world.PlayerCubeState.WallAxis);
				currentMovementVelocity -= wallAxisCurVel * world.PlayerCubeState.WallAxis;
				float wallAxisTargetVel = Vector3.Dot(targetMovementVelocity, world.PlayerCubeState.WallAxis);
				targetMovementVelocity -= wallAxisTargetVel * world.PlayerCubeState.WallAxis;

				world.Velocities[i] -= 2.5f * world.PlayerCubeState.WallAxis;
			}

			// WALL ANTI-GRAVITY HACK
			if (world.PlayerCubeState.OnWall)
			{
				if (world.Velocities[i].Y < 0) world.Velocities[i].Y += (-world.Gravity.Y) * delta * WALL_ANTI_GRAVITY_FACTOR;
			}

			// Jumping
			{
				if (world.InputData[i].NewJump())
				{
					if (world.PlayerCubeState.OnGround) // Ground jump
					{
						world.Velocities[i].Y = MIN_JUMP_SPEED;
						if (world.Velocities[i].Y < MIN_JUMP_SPEED) world.Velocities[i].Y = MIN_JUMP_SPEED;
						world.Accelerations[i] = new Vector3(0.0f, JUMP_DECISION_ACCELERATION - world.Gravity.Y, 0.0f);
						jumpTime = delta;
					}
					else if (world.PlayerCubeState.OnWall) // Wall jump
					{
						Vector3 wallAxis = world.PlayerCubeState.WallAxis;
						world.Velocities[i].Y = MIN_WALL_JUMP_SPEED;
						if (world.Velocities[i].Y < MIN_WALL_JUMP_SPEED) world.Velocities[i].Y = MIN_WALL_JUMP_SPEED;
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
			Vector3 cameraDiff = world.Camera.Position - world.Transforms[world.Player.Id].Translation;
		
			// Calculate angle formed along ground by the cameras position relative the player
			float offset = (float)Math.Atan2(cameraDiff.X, cameraDiff.Z);

			// Invert y input
			direction.Y = -direction.Y;

			// Rotate in 3D, since don't have 2x2 matrix class
			Vector3 directionInput3D = new Vector3(direction.X, 0, direction.Y);
			Vector3 rotatedInput = Vector3.Transform(directionInput3D, Matrix.CreateRotationY(offset));
			return rotatedInput;
		}

		private void RotateOBB(ref OBB obbOut, OBBAxis axisEnum, ref Vector3 newAxis)
		{
			switch (axisEnum)
			{
				case OBBAxis.X_PLUS:
				case OBBAxis.X_MINUS:
					obbOut.AxisX = axisEnum.Sign() * newAxis;
					obbOut.AxisY = Vector3.Cross(obbOut.AxisZ, obbOut.AxisX);
					obbOut.AxisZ = Vector3.Cross(obbOut.AxisX, obbOut.AxisY);
					break;
				case OBBAxis.Y_PLUS:
				case OBBAxis.Y_MINUS:
					obbOut.AxisY = axisEnum.Sign() * newAxis;
					obbOut.AxisX = Vector3.Cross(obbOut.AxisY, obbOut.AxisZ);
					obbOut.AxisZ = Vector3.Cross(obbOut.AxisX, obbOut.AxisY);
					break;
				case OBBAxis.Z_PLUS:
				case OBBAxis.Z_MINUS:
					obbOut.AxisZ = axisEnum.Sign() * newAxis;
					obbOut.AxisX = Vector3.Cross(obbOut.AxisY, obbOut.AxisZ);
					obbOut.AxisY = Vector3.Cross(obbOut.AxisZ, obbOut.AxisX);
					break;
			}
		}

		private void TransformFromOBBs(ref OBB oldOBB, ref OBB newOBB, ref Matrix transformOut)
		{
			// Update translation in transform
			Vector3 obbDiff = newOBB.Position - oldOBB.Position;
			transformOut.Translation += obbDiff;
			// Update rotation: This is probably a really stupid way.
			transformOut.Backward = newOBB.AxisZ * transformOut.Forward.Length();
			transformOut.Right = newOBB.AxisX * transformOut.Left.Length();
			transformOut.Up = newOBB.AxisY * transformOut.Up.Length();
		}

		private float angleRadBetweenTwoNormalizedVectors(ref Vector3 a, ref Vector3 b)
		{
			return (float)Math.Acos(Vector3.Dot(a, b));
		}
	}
}
