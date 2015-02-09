using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Wrap around a custom shader/XNA effect that could be used for rendering
	/// game objects with our own custom effects.
	/// </summary>
	class StandardEffect
	{
		public Effect Effect
		{
			get;
			private set;
		}
		private readonly EffectParameter worldParam;
		private readonly EffectParameter viewParam;
		private readonly EffectParameter projectionParam;
		private readonly EffectParameter normalMatrixParam;
		private readonly EffectParameter viewSpaceLightPostionParam;
		private readonly EffectParameter diffuseTextureParam;
		private readonly EffectParameter specularTextureParam;
		private readonly EffectParameter emissiveTextureParam;
		private readonly EffectParameter emissiveIntensityParam;

		private Matrix view;

		public StandardEffect(Effect effect)
		{
			this.Effect = effect;

			// Cache these since they are fairly expensive to fetch
			this.worldParam = effect.Parameters["World"];
			this.viewParam = effect.Parameters["View"];
			this.projectionParam = effect.Parameters["Projection"];
			this.normalMatrixParam = effect.Parameters["NormalMatrix"];
			this.viewSpaceLightPostionParam = effect.Parameters["ViewSpaceLightPosition"];
			this.diffuseTextureParam = effect.Parameters["DiffuseTexture"];
			this.specularTextureParam = effect.Parameters["SpecularTexture"];
			this.emissiveTextureParam = effect.Parameters["EmissiveTexture"];
			this.emissiveIntensityParam = effect.Parameters["MaterialEmissiveIntensity"];
		}

		public void SetOncePerFrameParams(ref Matrix view, ref Matrix projection, ref Vector3 lightPosition)
		{
			this.view = view;
			viewParam.SetValue(view);
			projectionParam.SetValue(projection);

			viewSpaceLightPostionParam.SetValue(Vector3.Transform(lightPosition, view));
		}

		public void SetWorld(ref Matrix world)
		{
			worldParam.SetValue(world);
			UpdateNormalMatrix(ref world);
		}

		private void UpdateNormalMatrix(ref Matrix world)
		{
			// Precalculate normal matrix used in effect
			Matrix worldViewMatrix;
			Matrix.Multiply(ref view, ref world, out worldViewMatrix);
			Matrix worldViewInverse;
			Matrix.Invert(ref worldViewMatrix, out worldViewInverse);
			Matrix normalMatrix;
			Matrix.Transpose(ref worldViewInverse, out normalMatrix);

			normalMatrixParam.SetValue(normalMatrix);
		}

		public void SetDiffuseTexture(Texture texture)
		{
			diffuseTextureParam.SetValue(texture);
		}

		public void SetSpecularTexture(Texture texture)
		{
			specularTextureParam.SetValue(texture);
		}

		public void SetEmissiveTexture(Texture texture)
		{
			emissiveTextureParam.SetValue(texture);
		}

		public void SetMaterialEmissiveIntensity(float intensity)
		{
			emissiveIntensityParam.SetValue(intensity);
		}

	}
}
