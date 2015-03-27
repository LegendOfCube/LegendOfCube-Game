using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Graphics
{
	public struct PointLight
	{
		public float reach;
		public Vector4 color;
		public Vector3 lightPosition;

		public PointLight(float reach, Vector4 color, Vector3 lightPosition)
		{
			this.reach = reach;
			this.color = color;
			this.lightPosition = lightPosition;
		}
	}
}
