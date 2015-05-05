using System;

namespace LegendOfCube.Engine.Events
{
	static class EventUtils
	{
		private static bool IsPairCombination(CollisionEvent c, Func<Entity, bool> entity1Satisfies, Func<Entity, bool> entity2Satisfies)
		{
			if (entity1Satisfies(c.Collider))
			{
				return entity2Satisfies(c.CollidedWith);
			}
			if (entity2Satisfies(c.CollidedWith))
			{
				return entity1Satisfies(c.Collider);
			}
			return false;
		}

		public static bool PlayerShouldWin(World world, CollisionEvent c)
		{
			return IsPairCombination(c,
				e => e.Id == world.Player.Id,
				e => world.EntityProperties[e.Id].Satisfies((Properties.WIN_ZONE_FLAG)));
		}

		public static bool PlayerShouldDie(World world, CollisionEvent c)
		{
			return IsPairCombination(c,
				e => e.Id == world.Player.Id,
				e => world.EntityProperties[e.Id].Satisfies((Properties.DEATH_ZONE_FLAG)));
		}
	}
}
