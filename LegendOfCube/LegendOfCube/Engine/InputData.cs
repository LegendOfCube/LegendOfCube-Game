using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{

    public interface InputData
    {
        Vector2 GetDirection();
        Vector2 GetCameraDirection();
        bool IsJumping();

        
    }
    public class InputDataImpl : InputData
    {
        // Members
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        private Vector2 direction;
        private Vector2 cameraDirection;
        private bool isJumping;

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

        public void SetStateOfJumping(bool isJumping)
        {
            this.isJumping = isJumping;
        }
    }
}
