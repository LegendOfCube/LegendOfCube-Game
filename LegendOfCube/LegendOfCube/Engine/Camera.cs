using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public struct Camera
	{
		public const float DEFAULT_FOV = 70.0f;
		public static readonly Camera DEFAULT_CAMERA = new Camera(Vector3.Zero, Vector3.Zero);
		
		public float Fov;
		public Vector3 Position;
		public Vector3 Target;
		public Vector3 Up;

		public Camera(Vector3 position, Vector3 target, float fov = DEFAULT_FOV)
		{
			Position = position;
			Target = target;
			Fov = fov;
			Up = Vector3.Up;
		}
	}
}
