using LegendOfCube.Engine;

namespace LegendOfCube.Levels
{
	static class LevelConstants
	{
		public static readonly Level LEVEL_1 = new Level1();
		public static readonly Level DEMO_LEVEL = new DemoLevel();
		public static readonly Level CONCEPT_LEVEL = new ConceptLevel();
		public static readonly Level TEST_LEVEL1 = new TestLevel1();
		public static readonly Level WALL_CLIMB_LEVEL = new WallClimbLevel();
		public static readonly Level BEANSTALK_LEVEL = new BeanstalkLevel();
		public static readonly Level LEVEL_13 = new Level13();
		public static readonly Level STAIRWAY_LEVEL = new StairwayLevel();
		public static readonly Level BACKGROUND_LEVEL = new BackgroundLevel();

		public static readonly Level[] LEVELS =
		{
			LEVEL_1,
			DEMO_LEVEL,
			CONCEPT_LEVEL,
			TEST_LEVEL1,
			WALL_CLIMB_LEVEL,
			BEANSTALK_LEVEL,
			LEVEL_13,
			STAIRWAY_LEVEL,
		};
	}
}
