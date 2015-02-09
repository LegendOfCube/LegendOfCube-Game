using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class RenderSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		private static readonly Properties TRANSFORM = new Properties(Properties.TRANSFORM);
		private static readonly Properties MODEL_AND_TRANSFORM = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM);

		private static readonly Properties FULL_LIGHT_EFFECT = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM |
		                                                                Properties.FULL_LIGHT_EFFECT);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;
		private StandardEffect standardEffect;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game)
		{
			this.game = game;
			graphics = new GraphicsDeviceManager(game);

			// TODO: Remove this. It's for unlocking frame rate temporarily.
			graphics.SynchronizeWithVerticalRetrace = false;
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void Initialize()
		{
			game.Window.AllowUserResizing = true;
			graphics.PreferMultiSampling = true;
			game.GraphicsDevice.BlendState = BlendState.Opaque;
			game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			graphics.ApplyChanges();

			this.standardEffect = new StandardEffect(game.Content.Load<Effect>("Effects/standardEffect"));

			// TODO: Load textures somewhere more appropriate
			// For now, override loaded effect with custom shader. Need to
			// manually find texture it seems. It might be possible to write
			// custom "content importer" to handle this instead.
			var cubeDiffuseTexture = game.Content.Load<Texture>("Models/cube_diff");
			var cubeSpecularTexture = game.Content.Load<Texture>("Models/cube_specular");
			var cubeEmissiveTexture = game.Content.Load<Texture>("Models/cube_emissive");

			// TODO: Store texture to use for each entity
			standardEffect.SetDiffuseTexture(cubeDiffuseTexture);
			standardEffect.SetSpecularTexture(cubeSpecularTexture);
			standardEffect.SetEmissiveTexture(cubeEmissiveTexture);
			standardEffect.SetMaterialEmissiveIntensity(0.5f);
		}

		public void RenderWorld(World world)
		{
			Vector3 playerPos = new Vector3();
			//Find player
			foreach (Entity e in world.EnumerateEntities(new Properties(Properties.INPUT_FLAG)))
			{
				playerPos = world.Transforms[e.Id].Translation;
				break;
			}

			Vector3 camPos = playerPos;
			camPos.Y = 4;
			camPos.Z += 5;
			Vector3 camTarget = playerPos;
			Vector3 up = new Vector3(0, 1, 0);
			float fov = 75;

			Matrix view = Matrix.CreateLookAt(camPos, camTarget, up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(fov),
			                        game.GraphicsDevice.Viewport.AspectRatio,
			                        0.1f,
			                        1000.0f);

			var boundingFrustum = new BoundingFrustum(view * projection);

			foreach (var e in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				RenderEntity(e, world, boundingFrustum, ref view, ref projection);
			}
		}

		private void RenderEntity(Entity entity, World world, BoundingFrustum boundingFrustum, ref Matrix view, ref Matrix projection)
		{

			var model = world.Models[entity.Id];
			var worldTransform = world.Transforms[entity.Id];

			// Don't render if entity wouldn't be seen
			if (!ModelInFrustrum(model, boundingFrustum, ref worldTransform))
			{
				return;
			}

			standardEffect.SetOncePerFrameParams(ref view, ref projection, ref world.LightPosition);

			// Not exactly sure about the reason for this, but seems to be the standard way to do it
			var transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(transforms);

			foreach (var mesh in model.Meshes)
			{
				if (world.EntityProperties[entity.Id].Satisfies(FULL_LIGHT_EFFECT))
				{
					foreach (var meshPart in mesh.MeshParts)
					{
						// TODO: Move from here. Ugly and unnecessary to do all the time.
						// Replace with standard effect
						meshPart.Effect = standardEffect.Effect;
					}
					var worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
					standardEffect.SetWorld(ref worldMatrix);
				}
				else
				{
					// Make it still possible to render with the default BasicEffect
					foreach (var effect in mesh.Effects)
					{
						var basicEffect = effect as BasicEffect;
						if (basicEffect != null)
						{
							basicEffect.World = transforms[mesh.ParentBone.Index] * worldTransform;
							basicEffect.View = view;
							basicEffect.Projection = projection;
						}
					}
				}
				mesh.Draw();
			}
		}

		private static bool ModelInFrustrum(Model model, BoundingFrustum boundingFrustum, ref Matrix worldTransform)
		{
			// Go through all BoundingSpheres in Model and check if inside frustrums
			foreach (var mesh in model.Meshes)
			{
				BoundingSphere boundingSphere;
				mesh.BoundingSphere.Transform(ref worldTransform, out boundingSphere);
				var containmentType = boundingFrustum.Contains(boundingSphere);
				if (containmentType != ContainmentType.Disjoint)
				{
					return true;
				}
			}
			return false;
		}
	}
}
