using System;
using LegendOfCube.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegendOfCubeTests
{
	[TestClass]
	public class PropertiesTests
	{
		[TestMethod]
		public void TestSatisfies()
		{
			// Silly test, sort of circular
			Properties p = new Properties();
			p.Add(Properties.TRANSFORM);
			Assert.IsTrue(p.Satisfies(new Properties(Properties.TRANSFORM)));
			p.Subtact(Properties.TRANSFORM);
			Assert.IsFalse(p.Satisfies(new Properties(Properties.TRANSFORM)));
		}
	}
}
