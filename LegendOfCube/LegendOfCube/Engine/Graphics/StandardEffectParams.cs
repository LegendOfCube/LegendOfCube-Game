using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	public class StandardEffectParams
	{
		public StandardEffectParams()
		{
			DiffuseColor = Color.White.ToVector4();
			SpecularColor = Color.Black.ToVector4();
			EmissiveColor = Color.Black.ToVector4();
		}

		public Vector4 DiffuseColor { get; set; }
		public Texture DiffuseTexture { get; set; }
		public Vector4 SpecularColor { get; set; }
		public Texture SpecularTexture { get; set; }
		public Vector4 EmissiveColor { get; set; }
		public Texture EmissiveTexture { get; set; }
		public Texture NormalTexture { get; set; }
	}
}
