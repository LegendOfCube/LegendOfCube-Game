namespace LegendOfCube.Engine
{
	class GameplaySystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVEMENT_INPUT = new Properties(Properties.TRANSFORM |
		                                                                   Properties.INPUT_FLAG);

		public void ProcessInputData(World world)
		{
			foreach (var e in world.EnumerateEntities(MOVEMENT_INPUT))
			{
				// Updates velocities according to input
				//TODO: Make it better
				// Movement
				world.Velocities[e.Id].X = 10 * world.InputData[e.Id].GetDirection().X;
				world.Velocities[e.Id].Z = - 10 * world.InputData[e.Id].GetDirection().Y;
				// Jumping
				if (world.InputData[e.Id].IsJumping()) world.Velocities[e.Id].Y = 8.0f;
			}
		}
	}
}
