using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{	class CameraSystem
	{
		private const float DISTANCE_ABOVE_PLAYER = 1.8f;

		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = -1.0f;

		private const float MIN_DISTANCE = 2.0f;
		private const float MAX_DISTANCE = 10.0f;

		private const float MIN_ANGLE = -MathHelper.PiOver4;
		private const float MAX_ANGLE = MathHelper.Pi / 3.0f;

		private float distanceFactor = 0.5f;

		public void OnUpdate(World world, float delta)
		{
			var oldCameraPosition = world.CameraPosition;

			// Fetch input data
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();

			var playerPosition = world.Transforms[world.Player.Id].Translation;
			var cameraTarget = playerPosition;
			cameraTarget.Y += DISTANCE_ABOVE_PLAYER;

			// Let the ground angle be dependant on the old camera position and new 
			// target location, will make it fall behind player over time
			var cameraPosRelTarget = oldCameraPosition - cameraTarget;
			float groundAngle = (float)Math.Atan2(cameraPosRelTarget.Z, cameraPosRelTarget.X);

			// Modify ground angle depending on input
			groundAngle += (delta * X_SCALE * cameraModifierInput.X) % (2.0f * MathHelper.Pi);

			// Calculate a distance factor, between 0.0 and 1.0, which depend on input
			distanceFactor = MathHelper.Clamp(distanceFactor + delta * Y_SCALE * cameraModifierInput.Y, 0.0f, 1.0f);
			float tiltAngle;
			float distance;

			CalcZoomTrejectory(distanceFactor, out distance, out tiltAngle);

			// Translate to cartesian coords
			Vector3 newCameraRelTarget = new Vector3(
				(float)(distance * Math.Cos(tiltAngle) * Math.Cos(groundAngle)),
				(float)(distance * Math.Sin(tiltAngle)),
				(float)(distance * Math.Cos(tiltAngle) * Math.Sin(groundAngle))
			);

			world.CameraPosition = cameraTarget + newCameraRelTarget;
			world.CameraTarget = cameraTarget;
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
