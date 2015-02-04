namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVABLE = new Properties(
		                                                         Properties.POSITION |
		                                                         Properties.VELOCITY);
		private static readonly Properties ACCELERATABLE = new Properties(
		                                                               Properties.VELOCITY |
		                                                               Properties.ACCELERATION);
		private static readonly Properties GRAVITY = new Properties(
		                                                         Properties.VELOCITY |
		                                                         Properties.AFFECTED_BY_GRAVITY);
	}
}
