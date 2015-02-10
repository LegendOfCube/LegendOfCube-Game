using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Contains information that only the cube needs.
	/// </summary>
	public struct PlayerCubeState
	{
		public const int MAXJUMPS = 2;
		public int CurrentJumps;
		public bool OnWall;
	}
}
