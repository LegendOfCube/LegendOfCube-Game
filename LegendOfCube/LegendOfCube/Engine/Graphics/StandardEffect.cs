using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	/// <summary>
	/// Wrap around a custom shader/XNA effect that could be used for rendering
	/// game objects with our own custom effects.
	/// </summary>
	class StandardEffect
	{
		private readonly Effect effect;

		// Matrices
		private readonly EffectParameter worldParam;
		private readonly EffectParameter viewParam;
		private readonly EffectParameter projectionParam;
		private readonly EffectParameter normalMatrixParam;

		// Lights
		private readonly EffectParameter dirLight0ViewSpaceDirParam;
		private readonly EffectParameter dirLight0ColorParam;
		private readonly EffectParameter dirLight0ShadowMapParam;
		private readonly EffectParameter dirLight0ShadowMatrixParam;

		private readonly EffectParameter ambientIntensity;

		// Material properties

		private readonly EffectParameter useDiffuseTextureParam;
		private readonly EffectParameter diffuseTextureParam;
		private readonly EffectParameter materialDiffuseColorParam;

		private readonly EffectParameter useSpecularTextureParam;
		private readonly EffectParameter specularTextureParam;
		private readonly EffectParameter materialSpecularColorParam;

		private readonly EffectParameter useEmissiveTextureParam;
		private readonly EffectParameter emissiveTextureParam;
		private readonly EffectParameter materialEmissiveColorParam;

		private readonly EffectParameter normalTextureParam;

		private readonly EffectTechnique defaultTechnique;
		private readonly EffectTechnique normalMapTechnique;
		private readonly EffectTechnique shadowMapTechnique;

		// Cache this, for determining normal matrix (model to view) after
		// world has been set
		private Matrix view;

		private StandardEffect(Effect effect)
		{
			this.effect = effect;

			// Cache handles to parameteres

			// Matrices
			this.worldParam = effect.Parameters["World"];
			this.viewParam = effect.Parameters["View"];
			this.projectionParam = effect.Parameters["Projection"];
			this.normalMatrixParam = effect.Parameters["NormalMatrix"];
	
			// Lights
			this.dirLight0ViewSpaceDirParam = effect.Parameters["DirLight0ViewSpaceDir"];
			this.dirLight0ColorParam = effect.Parameters["DirLight0Color"];
			this.dirLight0ShadowMapParam = effect.Parameters["DirLight0ShadowMap"];
			this.dirLight0ShadowMatrixParam = effect.Parameters["DirLight0ShadowMatrix"];

			this.ambientIntensity = effect.Parameters["AmbientIntensity"];

			// Material properties
			
			this.useDiffuseTextureParam = effect.Parameters["UseDiffuseTexture"];
			this.diffuseTextureParam = effect.Parameters["DiffuseTexture"];
			this.materialDiffuseColorParam = effect.Parameters["MaterialDiffuseColor"];

			this.useSpecularTextureParam = effect.Parameters["UseSpecularTexture"];
			this.specularTextureParam = effect.Parameters["SpecularTexture"];
			this.materialSpecularColorParam = effect.Parameters["MaterialSpecularColor"];

			this.useEmissiveTextureParam = effect.Parameters["UseEmissiveTexture"];
			this.emissiveTextureParam = effect.Parameters["EmissiveTexture"];
			this.materialEmissiveColorParam = effect.Parameters["MaterialEmissiveColor"];

			this.normalTextureParam = effect.Parameters["NormalTexture"];

			// Get handles to techniques
			this.defaultTechnique = this.effect.Techniques["DefaultTechnique"];
			this.normalMapTechnique = this.effect.Techniques["NormalMapTechnique"];
			this.shadowMapTechnique = this.effect.Techniques["ShadowMapTechnique"];
		}

		public void SetViewProjection(ref Matrix view, ref Matrix projection)
		{
			this.view = view;
			viewParam.SetValue(view);
			projectionParam.SetValue(projection);
		}

		public void SetDirLight0Properties(ref Vector3 direction, ref Vector4 lightColor)
		{
			dirLight0ViewSpaceDirParam.SetValue(Vector3.TransformNormal(direction, view));
			dirLight0ColorParam.SetValue(lightColor);
		}

		public void SetDirLight0ShadowMap(Texture shadowMap)
		{
			dirLight0ShadowMapParam.SetValue(shadowMap);
		}

		public void SetDirLight0ShadowMatrix(ref Matrix shadowMatrix)
		{
			dirLight0ShadowMatrixParam.SetValue(shadowMatrix);
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
			Matrix.Multiply(ref world, ref view, out worldViewMatrix);
			Matrix worldViewInverse;
			Matrix.Invert(ref worldViewMatrix, out worldViewInverse);
			Matrix normalMatrix;
			Matrix.Transpose(ref worldViewInverse, out normalMatrix);

			normalMatrixParam.SetValue(normalMatrix);
		}

		/// <summary>
		/// Set the intensity of ambient light in the scene. The diffuse color
		/// will be used for ambient as well. A value of 0.0 mean no ambient
		/// light, and 1.0 would mean that the object appears fully lit without
		/// being lit by light source.
		/// </summary>
		/// <param name="intensity">The intensity, should be in range [0, 1]</param>
		public void SetAmbientIntensity(float intensity)
		{
			ambientIntensity.SetValue(intensity);
		}
 
		public void SetDiffuseColor(Vector4 color)
		{
			materialDiffuseColorParam.SetValue(color);
		}

		public void SetSpecularColor(Vector4 color)
		{
			materialSpecularColorParam.SetValue(color);
		}

		public void SetEmissiveColor(Vector4 color)
		{
			materialEmissiveColorParam.SetValue(color);
		}

		/// <summary>
		/// Set diffuse texture. Set this to null to use only the diffuse
		/// color for diffuse appearence.
		/// </summary>
		/// <param name="texture">The texture, could be null</param>
		public void SetDiffuseTexture(Texture texture)
		{
			useDiffuseTextureParam.SetValue(texture != null);
			diffuseTextureParam.SetValue(texture);
		}

		/// <summary>
		/// Set diffuse texture. Set this to null to use only the diffuse
		/// color for diffuse appearence.
		/// </summary>
		/// <param name="texture">The texture, could be null</param>
		public void SetSpecularTexture(Texture texture)
		{
			useSpecularTextureParam.SetValue(texture != null);
			specularTextureParam.SetValue(texture);
		}

		/// <summary>
		/// Set diffuse texture. Set this to null to use only the diffuse
		/// color for diffuse appearence.
		/// </summary>
		/// <param name="texture">The texture, could be null</param>
		public void SetEmissiveTexture(Texture texture)
		{
			useEmissiveTextureParam.SetValue(texture != null);
			emissiveTextureParam.SetValue(texture);
		}

		/// <summary>
		/// Set diffuse texture. Set this to null to use only the diffuse
		/// color for diffuse appearence.
		/// </summary>
		/// <param name="texture">The texture, could be null</param>
		public void SetNormalTexture(Texture texture)
		{
			effect.CurrentTechnique = texture != null ? normalMapTechnique : defaultTechnique;
			normalTextureParam.SetValue(texture);
		}

		public void SetShadowMapRendering(bool shadowMapRender)
		{
			effect.CurrentTechnique = shadowMapRender ? shadowMapTechnique : defaultTechnique;
		}

		public void ApplyOnModel(Model model)
		{
			foreach (var mesh in model.Meshes)
			{
				foreach (var meshPart in mesh.MeshParts)
				{
					meshPart.Effect = effect;
				}
			}
		}

		public static StandardEffect LoadEffect(ContentManager contentManager)
		{
			return new StandardEffect(contentManager.Load<Effect>("Effects/standardEffect"));
		}
	}
}
