using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine.Graphics
{
	public struct DirLight
	{
		public static readonly Vector3 DEFAULT_DIRECTION = Vector3.Down;
		public static readonly Vector3 DEFAULT_COLOR = Microsoft.Xna.Framework.Color.White.ToVector3();
		public const float DEFAULT_INTENSITY = 0.55f;

		public Vector3 Direction;
		public Vector3 Color;
		public float Intensity;

		public DirLight(Vector3 direction) : this(direction, DEFAULT_INTENSITY) {}

		public DirLight(Vector3 direction, float intensity) : this(direction, intensity, DEFAULT_COLOR) {}

		public DirLight(Vector3 direction, float intensity, Vector3 color)
		{
			this.Color = color;
			this.Direction = direction;
			this.Intensity = intensity;
		}
	}
}
