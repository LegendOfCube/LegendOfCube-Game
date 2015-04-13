using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;

namespace LegendOfCube.Levels
{
	static class LevelConstants
	{
		public static readonly Level DEMO_LEVEL = new DemoLevel();
		public static readonly Level CONCEPT_LEVEL = new ConceptLevel();
		public static readonly Level TEST_LEVEL1 = new TestLevel1();
		public static readonly Level WALL_CLIMB_LEVEL = new WallClimbLevel();
		public static readonly Level BEAN_STALK_LEVEL = new BeanStalkLevel();
		public static readonly Level LEVEL_13 = new Level13();

		public static readonly Level[] LEVELS =
		{
			DEMO_LEVEL,
			CONCEPT_LEVEL,
			TEST_LEVEL1,
			WALL_CLIMB_LEVEL,
			BEAN_STALK_LEVEL,
			LEVEL_13
		};
	}
}
