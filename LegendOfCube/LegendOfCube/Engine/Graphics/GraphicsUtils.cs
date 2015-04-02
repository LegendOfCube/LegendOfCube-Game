using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	class GraphicsUtils
	{
		public static void ApplyEffectOnModel(Model model, Effect effect)
		{
			foreach (var mesh in model.Meshes)
			{
				foreach (var meshPart in mesh.MeshParts)
				{
					meshPart.Effect = effect;
				}
			}
		}
	}
}
