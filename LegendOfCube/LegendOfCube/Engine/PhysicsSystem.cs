using System;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties MOVABLE = new Properties(
		                                                         Properties.TRANSFORM |
		                                                         Properties.VELOCITY);
		private static readonly Properties ACCELERATABLE = new Properties(
		                                                               Properties.VELOCITY |
		                                                               Properties.ACCELERATION);
		private static readonly Properties GRAVITY = new Properties(
		                                                         Properties.VELOCITY |
		                                                         Properties.AFFECTED_BY_GRAVITY);

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
