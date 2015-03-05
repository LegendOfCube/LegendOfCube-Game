using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

﻿namespace LegendOfCube.Engine
{
	/// <summary>
	/// Contains information that only the cube needs.
	/// </summary>
	public struct PlayerCubeState
	{
		public bool OnWall, OnGround;
		public Vector3 WallAxis, GroundAxis;
	}
}
