using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class GameObjectTemplate
	{
		public Model Model { get; set; }
		public OBB Obb { get; set; }
		public StandardEffectParams EffectParams { get; set; }
	}
}
