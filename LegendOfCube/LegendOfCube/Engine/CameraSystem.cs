using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{	class CameraSystem
	{
		private const float UP_OFFSET = 1.0f;

		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = -4.0f;

		private const float MIN_DISTANCE = 2.0f;
		private const float MAX_DISTANCE = 10.0f;

		private const float MIN_ANGLE = -MathHelper.PiOver4;
		private const float MAX_ANGLE = MathHelper.Pi / 3.0f;

		private const float BASE_TILT = 10.0f;
		private const float BASE_DISTANCE = 8.0f;

		private float distanceFactor = 0.5f;

		private Vector3 lastKnownDirection = Vector3.Down;

		public void OnUpdate(World world, float delta)
		{
			// Make various variables related to camera easily accessible
			var oldCamera = world.Camera;
			Vector3 oldPosition = oldCamera.Position;
			Vector3 oldTarget = oldCamera.Target;
			Vector3 oldPosRelOldTarget = oldPosition - oldTarget;
			Matrix playerTransform = world.Transforms[world.Player.Id];
			Vector3 playerPosition = playerTransform.Translation;

			// Fetch input data
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();

			// Set what's to be the target
			Vector3 cameraTarget = playerPosition + 0.5f * playerTransform.Up + UP_OFFSET * Vector3.Up;
			Vector3 oldPosRelNewTarget = oldPosition - cameraTarget;

			// Check which direction we appear to be travelling in, only update if above a certain limit
			Vector3 movementDirection = world.Velocities[world.Player.Id];
			if (movementDirection.Length() < 0.5f)
			{
				movementDirection = lastKnownDirection;
			}

			// Two modes for the camera, one where the player is activelly adjusting 
			// camera, and one where it drifts back to behind the player
			Vector3 newPosition;
			if (cameraModifierInput.Length() < 0.01f)
			{
				// Drift toward point opposite of movement direction
				
				Vector3 targetCameraDirection = -movementDirection;

				// Determine point where camera will rest
				float targetTiltAngle = MathHelper.ToRadians(BASE_TILT) + MathHelper.ToRadians(BASE_TILT) + (float)Math.Asin(targetCameraDirection.Y / targetCameraDirection.Length());
				float targetGroundAngle = (float)Math.Atan2(targetCameraDirection.Z, targetCameraDirection.X);

				Vector3 targetCamPos = cameraTarget + new Vector3(
					(float)(BASE_DISTANCE * Math.Cos(targetTiltAngle) * Math.Cos(targetGroundAngle)),
					(float)(BASE_DISTANCE * Math.Sin(targetTiltAngle)),
					(float)(BASE_DISTANCE * Math.Cos(targetTiltAngle) * Math.Sin(targetGroundAngle))
				);

				// Interpolate toward the target, will cause the camera to catch up faster when far away
				newPosition = MathUtils.Lerp(2 * delta, oldPosition, targetCamPos);
			}
			else
			{
				// Full manual control of camera around player

				float distance = oldPosRelOldTarget.Length();
				float tiltAngle = (float)Math.Asin(oldPosRelOldTarget.Y / oldPosRelOldTarget.Length());
				float groundAngle = (float)Math.Atan2(oldPosRelOldTarget.Z, oldPosRelOldTarget.X);

				// Modify angles depending on input
				tiltAngle = MathHelper.Clamp(tiltAngle + delta * Y_SCALE * cameraModifierInput.Y, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);
				groundAngle += (delta * X_SCALE * cameraModifierInput.X) % (MathHelper.TwoPi);

				newPosition = cameraTarget + new Vector3(
					(float)(distance * Math.Cos(tiltAngle) * Math.Cos(groundAngle)),
					(float)(distance * Math.Sin(tiltAngle)),
					(float)(distance * Math.Cos(tiltAngle) * Math.Sin(groundAngle))
				);
			}

			Camera newCamera = new Camera(newPosition, cameraTarget);
			world.Camera = newCamera;
			lastKnownDirection = movementDirection;
		}

		/// <summary>
		/// Defines the trejectory of the camera moving closer to the player.
		/// </summary>
		/// <param name="t">a number between 0.0 and 1.0, where a higher number means further away from target</param>
		/// <param name="distance">the distance from target, part of output</param>
		/// <param name="tiltAngle">the inclination angle, 0.0  means parallel to ground plane</param>
		private static void CalcZoomTrejectory(float t, out float distance, out float tiltAngle)
		{
			distance = MathHelper.Lerp(MIN_DISTANCE, MAX_DISTANCE, t);
			tiltAngle = MathHelper.Lerp(MIN_ANGLE, MAX_ANGLE, t);
		}
	}
}
