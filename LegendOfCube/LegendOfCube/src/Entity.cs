using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	/// <summary>
	/// Type used to specify an entity in the World.
	/// </summary>
	public struct Entity
	{
		public readonly UInt32 Id;
		public Entity(UInt32 id)
		{
			Id = id;
		}
	}
}
