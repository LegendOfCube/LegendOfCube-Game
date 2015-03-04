using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{	class CameraSystem
	{

		private const float DISTANCE_ABOVE_PLAYER = 2.0f;
		private const float CAMERA_DISTANCE = 7.0f;
		private const float X_SCALE = 4.0f;
		private const float Y_SCALE = 4.0f;

		public void Initialize(World world)
		{
			world.CameraPosition = new Vector3(1.0f, 1.0f, 0.0f);
		}

		public void OnUpdate(World world, float delta)
		{
			var inputData = world.InputData[world.Player.Id];
			Vector2 cameraModifierInput = inputData.GetCameraDirection();

			var playerPosition = world.Transforms[world.Player.Id].Translation;

			var cameraTarget = playerPosition;
			cameraTarget.Y += DISTANCE_ABOVE_PLAYER;

			var cameraPosRelTarget = world.CameraPosition - cameraTarget;
			float distance = cameraPosRelTarget.Length();

			// Translate to polar coordinates
			float tiltAngle = (float)Math.Asin(cameraPosRelTarget.Y / distance);
			float groundAngle = (float)Math.Atan2(cameraPosRelTarget.Z, cameraPosRelTarget.X);

			// Modify angles depending on input
			groundAngle += (delta * X_SCALE * cameraModifierInput.X) % (2.0f * MathHelper.Pi);
			tiltAngle = MathHelper.Clamp(tiltAngle + delta * Y_SCALE * cameraModifierInput.Y, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

			// Set distance from target constant at the moment
			distance = CAMERA_DISTANCE;

			// Translate back to cartesian
			Vector3 newCameraRelTarget = new Vector3(
				(float)(distance * Math.Cos(tiltAngle) * Math.Cos(groundAngle)),
				(float)(distance * Math.Sin(tiltAngle)),
				(float)(distance * Math.Cos(tiltAngle) * Math.Sin(groundAngle))
			);	

			world.CameraPosition = cameraTarget + newCameraRelTarget;
		}
	}
}
