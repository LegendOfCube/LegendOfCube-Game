using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class CameraSystem
	{
		// Difines how much camera can look up/down
		private const float MAX_TILT = MathHelper.PiOver2 - 0.2f;

		// The distance above player to actually look at
		private const float TARGET_Y_OFFSET = 0.0f;

		// Scale input
		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = -4.0f;

		// How close camera can be to target without correcting
		private const float MIN_DISTANCE = 5.0f;
		private const float MAX_DISTANCE = 8.0f;

		private const float TILT_CORRECT_SPEED = 3.0f;

		// Modify how fast it takes for camera to center on player in y-direction
		private const float TARGET_TRACK_Y_SPEED = 5.0f;

		// Defines to what tilt the camera will aim to see target from
		private const float BASE_TILT = 30.0f;

		// Time in seconds until manual adjustments starts to reset
		private const double CAMERA_RESET_TIME = 0.1;

		// UNUSED: Angles to view player from at different zoom levels
		private const float ZOOM_MIN_ANGLE = -MathHelper.PiOver4;
		private const float ZOOM_MAX_ANGLE = MathHelper.Pi / 3.0f;

		// UNUSED: Remember zoom level user has chosen
		private float distanceFactor = 0.5f;

		private double lastManualAdjustTime = double.MinValue;
		private Vector3 lastKnownDirection = Vector3.Down;
		private bool movedSinceManualControl = false;

		public void OnUpdate(World world, GameTime gameTime, float delta)
		{
			double now = gameTime.TotalGameTime.TotalSeconds;

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
			Vector3 newTarget = playerPosition + 0.5f * playerTransform.Up + TARGET_Y_OFFSET * Vector3.Up;

			newTarget.Y = MathUtils.ClampLerp(TARGET_TRACK_Y_SPEED * delta, oldTarget.Y, newTarget.Y);

			// Now fetch position relative to new target
			Vector3 oldRelNewTargetPos = oldPosition - newTarget;
			float oldRelNewTargetDistance, oldRelNewTargetTiltAngle, oldRelNewTargetGroundAngle;
			ToSpherical(oldRelNewTargetPos, out oldRelNewTargetDistance, out oldRelNewTargetTiltAngle, out oldRelNewTargetGroundAngle);

			// Determine direction player appears to be heading in
			Vector3 targetCameraDirection;
			Vector3 movementDirection = world.Velocities[world.Player.Id];
			// If under threshold, use previous value
			if (movementDirection.Length() > 0.5f)
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
			float targetTiltAngle = ClampTilt(MathHelper.ToRadians(BASE_TILT) - GetTiltAngle(targetCameraDirection));
			float targetGroundAngle = (float)Math.Atan2(targetCameraDirection.Z, targetCameraDirection.X);

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
			else if (manualControlLock || playerTeleport)
			{
				// Keep manually set position for a while
				newTiltAngle = oldTiltAngle;
				newGroundAngle = oldGroundAngle;
			}
			else
			{
				if (world.PlayerCubeState.OnGround)
				{
					// Drift toward target tilt angle
					newTiltAngle = MathUtils.ClampLerp(TILT_CORRECT_SPEED * delta, oldRelNewTargetTiltAngle, targetTiltAngle);
				}
				else
				{
					newTiltAngle = oldRelNewTargetTiltAngle;
				}
				newGroundAngle = oldRelNewTargetGroundAngle;
			}

			// Set new camera in world
			var newPosition = newTarget + ToCartesian(newDistance, newTiltAngle, newGroundAngle);
			Camera newCamera = new Camera(newPosition, newTarget);
			world.Camera = newCamera;

			lastKnownDirection = targetCameraDirection;
		}

		/// <summary>
		/// Defines the trejectory of the camera moving closer to the player.
		/// </summary>
		/// <param name="t">a number between 0.0 and 1.0, where a higher number means further away from target</param>
		/// <param name="distance">the distance from target, part of output</param>
		/// <param name="tiltAngle">the inclination angle, 0.0  means parallel to ground plane</param>
		private static void CalcZoomTrejectory(float t, out float distance, out float tiltAngle)
		{
			distance = MathUtils.ClampLerp(t, MIN_DISTANCE, MAX_DISTANCE);
			tiltAngle = MathUtils.ClampLerp(t, ZOOM_MIN_ANGLE, ZOOM_MAX_ANGLE);
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
