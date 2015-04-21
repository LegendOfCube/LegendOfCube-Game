using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Graphics
{
	public struct PointLight
	{
		public readonly float Reach;
		public readonly Vector4 Color;
		public readonly Vector3 LightPosition;

		public PointLight(float reach, Vector4 color, Vector3 lightPosition)
		{
			this.Reach = reach;
			this.Color = color;
			this.LightPosition = lightPosition;
		}
	}
}
