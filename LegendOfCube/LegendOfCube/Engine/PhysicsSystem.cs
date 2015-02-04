namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly ComponentMask MOVABLE = new ComponentMask(
		                                                         ComponentMask.POSITION |
		                                                         ComponentMask.VELOCITY);
		private static readonly ComponentMask ACCELERATABLE = new ComponentMask(
		                                                               ComponentMask.VELOCITY |
		                                                               ComponentMask.ACCELERATION);
		private static readonly ComponentMask GRAVITY = new ComponentMask(
		                                                         ComponentMask.VELOCITY |
		                                                         ComponentMask.AFFECTED_BY_GRAVITY);
	}
}
