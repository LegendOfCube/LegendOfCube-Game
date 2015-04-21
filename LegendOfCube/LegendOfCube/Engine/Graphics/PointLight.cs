using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Graphics
{
	public struct PointLight
	{
		public float Reach;
		public Vector4 Color;
		public Vector3 LightPosition;

		public PointLight(float reach, Vector4 color, Vector3 lightPosition)
		{
			this.Reach = reach;
			this.Color = color;
			this.LightPosition = lightPosition;
		}
	}
}
