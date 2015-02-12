using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	public class StandardEffectParams
	{
		public Texture DiffuseTexture { get; set; }
		public Texture SpecularTexture { get; set; }
		public Texture EmissiveTexture { get; set; }
		public Texture NormalTexture { get; set; }
		public float EmissiveIntensity { get; set; }
	}
}
