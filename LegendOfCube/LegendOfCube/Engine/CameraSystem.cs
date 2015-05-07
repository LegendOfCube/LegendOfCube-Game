using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class CameraSystem
	{
		// Difines how much camera can look up/down
		private const float MAX_TILT = MathHelper.PiOver2 - 0.3f;

		// The distance above player to actually look at
		private const float TARGET_Y_OFFSET = 0.4f;

		// Scale input
		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = 4.0f;

		// How close camera can be to target without correcting
		private const float MIN_DISTANCE = 5.0f;
		private const float MAX_DISTANCE = 8.0f;

		private const float TILT_CORRECT_SPEED = 3.0f;

		// Modify how fast it takes for camera to center on player in y-direction
		private const float TARGET_UP_TRACK_SPEED = 8.0f;

		// Defines to what tilt the camera will aim to see target from
		private const float BASE_TILT = 30.0f;

		// Define what part of a semicircle in which the camera won't adjust when moving backwards, in [0, 180]
		private const float REVERSE_LOCK_ANGLE = 30.0f;

		// Time in seconds until manual adjustments starts to reset
		private const double CAMERA_RESET_TIME = 0.1;

		private double lastManualAdjustTime = double.MinValue;
		private Vector3 lastKnownDirection;
		private bool movedSinceManualControl = false;

		private bool lastFreeCam = false;
		private Camera beforeFreeNormalCam;

		public void OnStart(World world)
		{
			var target = GetPlayerTarget(world);
			var viewDirection = world.InitialViewDirection;
			if (viewDirection.Length() < 0.01f)
			{
				viewDirection = Vector3.Down;
			}
			lastKnownDirection = viewDirection;
			var camera = new Camera(target - MAX_DISTANCE * viewDirection, target) { Fov = GlobalConfig.Instance.Fov };
			world.Camera = camera;
		}

		public void Update(World world, GameTime gameTime, float delta)
		{
			if (!world.DebugState.FreeCamera)
			{
				NormalPlayerCamUpdate(world, gameTime, delta);
			}
			else
			{
				FreeCamUpdate(world, delta);
			}
			lastFreeCam = world.DebugState.FreeCamera;
		}

		private void NormalPlayerCamUpdate(World world, GameTime gameTime, float delta)
		{
			double now = gameTime.TotalGameTime.TotalSeconds;

			if (lastFreeCam)
			{
				// Restore old camera
				world.Camera = beforeFreeNormalCam;
			}

			// Make various variables related to camera easily accessible
			var oldCamera = world.Camera;
			Vector3 oldPosition = oldCamera.Position;
			Vector3 oldTarget = oldCamera.Target;
			Vector3 oldPosRelOldTarget = oldPosition - oldTarget;
			Matrix playerTransform = world.Transforms[world.Player.Id];
			Vector3 playerPosition = playerTransform.Translation;

			float oldDistance, oldTiltAngle, oldGroundAngle;
			ToSpherical(oldPosRelOldTarget, out oldDistance, out oldTiltAngle, out oldGroundAngle);

			// Fetch input data
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();
			bool inputOverThreshold = cameraModifierInput.Length() > 0.05f;

			// Set what's to be the target
			Vector3 newTarget = GetPlayerTarget(world);

			newTarget.Y = MathUtils.ClampLerp(TARGET_UP_TRACK_SPEED * delta, oldTarget.Y, newTarget.Y);

			// Now fetch position relative to new target
			Vector3 oldRelNewTargetPos = oldPosition - newTarget;
			float oldRelNewTargetDistance, oldRelNewTargetTiltAngle, oldRelNewTargetGroundAngle;
			ToSpherical(oldRelNewTargetPos, out oldRelNewTargetDistance, out oldRelNewTargetTiltAngle,
				out oldRelNewTargetGroundAngle);

			// Determine direction player appears to be heading in
			Vector3 targetCameraDirection;
			Vector3 movementDirection = world.Velocities[world.Player.Id];
			Vector2 groundMovement = new Vector2(movementDirection.X, movementDirection.Z);
			Vector2 groundMovementDirection = Vector2.Normalize(groundMovement);
			Vector2 oldCameraRelNewTargetDir = Vector2.Normalize(new Vector2(oldRelNewTargetPos.X, oldRelNewTargetPos.Z));
			bool moveTowardCamera = Vector2.Dot(oldCameraRelNewTargetDir, groundMovementDirection) >
			                        Math.Cos(MathHelper.ToRadians(REVERSE_LOCK_ANGLE/2.0f));

			// If under threshold, use previous value
			bool moveAlongGround = groundMovement.Length() > 0.5f;
			if (moveAlongGround)
			{
				targetCameraDirection = movementDirection;
				movedSinceManualControl = true;
			}
			else
			{
				targetCameraDirection = lastKnownDirection;
			}

			// Ugly way to check if camera shouldn't try to catch up, as when going through teleport portal or respawning
			bool playerTeleport = (oldTarget - newTarget).Length() > 5.0f;

			// Determine point where camera will rest
			float targetTiltAngle = world.PlayerCubeState.OnWall
				? MathHelper.ToRadians(BASE_TILT)
				: ClampTilt(MathHelper.ToRadians(BASE_TILT) - GetTiltAngle(targetCameraDirection));
			float targetGroundAngle = (float) Math.Atan2(targetCameraDirection.Z, targetCameraDirection.X);

			// Check if camera should stay at the manually set position
			bool manualControlLock = ((now - lastManualAdjustTime) < CAMERA_RESET_TIME) || !movedSinceManualControl;

			float newGroundAngle;
			float newTiltAngle;
			if (inputOverThreshold)
			{
				// Full manual control relative to player
				newTiltAngle = ClampTilt(oldTiltAngle + delta * Y_SCALE * cameraModifierInput.Y);
				newGroundAngle = (oldGroundAngle + delta * X_SCALE * cameraModifierInput.X) % MathHelper.TwoPi;

				lastManualAdjustTime = now;
				movedSinceManualControl = false;
			}
			else if (moveTowardCamera || manualControlLock || playerTeleport)
			{
				// Keep manually set position for a while
				newTiltAngle = oldTiltAngle;
				newGroundAngle = oldGroundAngle;
			}
			else
			{
				if (world.PlayerCubeState.OnGround || world.PlayerCubeState.OnWall)
				{
					// Drift toward target tilt angle
					newTiltAngle = MathUtils.ClampLerp(TILT_CORRECT_SPEED * delta, oldRelNewTargetTiltAngle, targetTiltAngle);
				}
				else
				{
					newTiltAngle = MathUtils.ClampLerp(TILT_CORRECT_SPEED / 3 * delta, oldRelNewTargetTiltAngle,
						MathHelper.ToRadians(BASE_TILT));
				}
				newGroundAngle = oldRelNewTargetGroundAngle;
			}

			// Smoothly change distance from target
			float newDistance;
			if (playerTeleport)
			{
				newDistance = MIN_DISTANCE;
			}
			else if (oldRelNewTargetDistance > MAX_DISTANCE)
			{
				newDistance = MAX_DISTANCE;
			}
			else if (oldRelNewTargetDistance < MIN_DISTANCE)
			{
				newDistance = MIN_DISTANCE;
			}
			else
			{
				newDistance = oldRelNewTargetDistance;
			}

			// Set new camera in world
			var newPosition = newTarget + ToCartesian(newDistance, newTiltAngle, newGroundAngle);
			Camera newCamera = new Camera(newPosition, newTarget) { Fov = GlobalConfig.Instance.Fov };
			world.Camera = newCamera;

			lastKnownDirection = targetCameraDirection;
		}

		private void FreeCamUpdate(World world, float delta)
		{
			if (!lastFreeCam)
			{
				// Save old camera
				beforeFreeNormalCam = world.Camera;
			}
			// Fetch input data
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();
			Vector2 moveInput = inputData.GetDirection();

			Vector3 dir = Vector3.Normalize(world.Camera.Target - world.Camera.Position);

			const float FREE_CAM_X_SCALE = -5.0f;
			const float FREE_CAM_Y_SCALE = -5.0f;
			const float FREE_CAM_FORWARD_SCALE = 18.0f;
			dir = Vector3.TransformNormal(dir, Matrix.CreateRotationY(FREE_CAM_X_SCALE * delta * cameraModifierInput.X));
			dir = Vector3.TransformNormal(dir,
				Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Up, dir), FREE_CAM_Y_SCALE * delta * cameraModifierInput.Y));

			world.Camera.Position += FREE_CAM_FORWARD_SCALE * delta * moveInput.Y * dir;
			world.Camera.Target = world.Camera.Position + dir;
			world.Camera.Fov = GlobalConfig.Instance.Fov;
		}

		private static Vector3 GetPlayerTarget(World world)
		{
			var playerTransform = world.Transforms[world.Player.Id];
			return playerTransform.Translation + 0.5f * playerTransform.Up + TARGET_Y_OFFSET * Vector3.Up;
		}

		private static float ClampTilt(float tiltAngle)
		{
			return MathHelper.Clamp(tiltAngle, -MAX_TILT, MAX_TILT);
		}

		private static void ToSpherical(Vector3 vector, out float radius, out float tiltAngle, out float groundAngle)
		{
			radius = vector.Length();
			tiltAngle = GetTiltAngle(vector);
			groundAngle = (float)Math.Atan2(vector.Z, vector.X);
		}

		private static float GetTiltAngle(Vector3 vector)
		{
			return (float)Math.Asin(vector.Y / vector.Length());
		}

		private static Vector3 ToCartesian(float radius, float tiltAngle, float groundAngle)
		{
			return new Vector3
			{
				X = (float)(radius * Math.Cos(tiltAngle) * Math.Cos(groundAngle)),
				Y = (float)(radius * Math.Sin(tiltAngle)),
				Z = (float)(radius * Math.Cos(tiltAngle) * Math.Sin(groundAngle))
			};
		}

	}
}
