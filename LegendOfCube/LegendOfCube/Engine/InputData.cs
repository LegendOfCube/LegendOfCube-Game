using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public class InputData
	{
		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		private Vector2 direction;
		private Vector2 cameraDirection;
		private bool isJumping, newJump;
		public bool BufferedJump { get; set; }

		public Vector2 GetDirection()
		{
			return direction;
		}

		public void SetDirection(Vector2 direction)
		{
			this.direction = direction;
		}

		public Vector2 GetCameraDirection()
		{
			return cameraDirection;
		}

		public void SetCameraDirection(Vector2 cameraDirection)
		{
			this.cameraDirection = cameraDirection;
		}

		public bool IsJumping()
		{
			return isJumping;
		}

		public bool NewJump()
		{
			return newJump;
		}

		public void SetStateOfJumping(bool isJumping)
		{
			this.isJumping = isJumping;
		}

		public void SetNewJump(bool isNewJump)
		{
			this.newJump = isNewJump;
		}
	}
}
