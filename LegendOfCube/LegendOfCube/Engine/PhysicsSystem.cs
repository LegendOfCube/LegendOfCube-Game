using System;
using Microsoft.Xna.Framework;

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

		public void ApplyPhysics(GameTime gameTime, World world)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (!world.ComponentMasks[i].Satisfies(GRAVITY)) continue;

				world.Transforms[i] = Matrix.CreateTranslation(world.Velocities[i].Y * world.Transforms[i].Up) * world.Transforms[i];
				world.Velocities[i].Y -= 0.02f;
			}
		}
	}
}
