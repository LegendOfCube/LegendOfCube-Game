using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class ModelData
	{
		private OBB obb;
		private bool hasObb = false;

		public Model Model { get; set; }

		public OBB Obb
		{
			get { return obb; }
			set {
				hasObb = true;
				obb = value; 
			}
		}

		public StandardEffectParams EffectParams { get; set; }
	
		public bool HasObb
		{
			get { return hasObb; }
		}
	}
}
