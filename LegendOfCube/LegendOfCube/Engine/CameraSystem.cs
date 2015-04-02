using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{	class CameraSystem
	{
		private const float DISTANCE_ABOVE_PLAYER = 1.8f;

		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = -4.0f;

		private const float MIN_DISTANCE = 2.0f;
		private const float MAX_DISTANCE = 10.0f;

		private const float MIN_ANGLE = -MathHelper.PiOver4;
		private const float MAX_ANGLE = MathHelper.Pi / 3.0f;

		private float distanceFactor = 0.5f;

		public void OnUpdate(World world, float delta)
		{
			var oldCamera = world.Camera;

			var oldPosition = oldCamera.Position;
			var oldTarget = oldCamera.Target;
			var positionRelOldTarget = oldPosition - oldTarget;
			var playerPosition = world.Transforms[world.Player.Id].Translation;
			float oldDistance = positionRelOldTarget.Length();

			var lastGroundAngle = (float)Math.Atan2(positionRelOldTarget.Z, positionRelOldTarget.X);
			var lastTiltAngle = (float)Math.Asin(positionRelOldTarget.Y / oldDistance);

			// Fetch input data
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();

			var cameraTarget = playerPosition;
			// Let the ground angle be dependant on the old camera position and new 
			// target location, will make it fall behind player over time
			var cameraPosRelTarget = oldCamera.Position - cameraTarget;

			// Modify ground angle depending on input
			var groundAngle = lastGroundAngle + (delta * X_SCALE * cameraModifierInput.X) % (2.0f * MathHelper.Pi);
			var tiltAngle = MathHelper.Clamp(lastTiltAngle + delta * Y_SCALE * cameraModifierInput.Y, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

			// Calculate a distance factor, between 0.0 and 1.0, which depend on input
			//distanceFactor = MathHelper.Clamp(distanceFactor + delta * Y_SCALE * cameraModifierInput.Y, 0.0f, 1.0f);
			distanceFactor = 0.5f;

			float zoomTilt;
			float zoomDistance;

			CalcZoomTrejectory(distanceFactor, out zoomDistance, out zoomTilt);

			// Translate to cartesian coords
			Vector3 newPosRelTarget = new Vector3(
				(float)(zoomDistance * Math.Cos(tiltAngle) * Math.Cos(groundAngle)),
				(float)(zoomDistance * Math.Sin(tiltAngle)),
				(float)(zoomDistance * Math.Cos(tiltAngle) * Math.Sin(groundAngle))
			);

			Camera newCamera = new Camera(cameraTarget + newPosRelTarget, cameraTarget);
			//newCamera.Fov = MathUtils.ClampLerp(world.Velocities[world.Player.Id].Length(), 60.0f, 90.0f, 0.0f, 20.0f);

			world.Camera = newCamera;
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
