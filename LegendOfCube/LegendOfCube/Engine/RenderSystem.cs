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

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game)
		{
			this.game = game;
			graphics = new GraphicsDeviceManager(game);
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

			foreach (var e in world.EnumerateEntities(MODEL_AND_TRANSFORM))
			{
				RenderEntity(e, world, ref view, ref projection);
			}
		}

		private void RenderEntity(Entity entity, World world, ref Matrix view, ref Matrix projection)
		{
			var model = world.Models[entity.Id];

			var worldTransform = world.Transforms[entity.Id];
			var transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(transforms);

			foreach (var mesh in model.Meshes)
			{
				foreach (var effect in mesh.Effects)
				{
					var basicEffect = effect as BasicEffect;
					if (basicEffect != null)
					{
						basicEffect.World = transforms[mesh.ParentBone.Index]*worldTransform;
						basicEffect.View = view;
						basicEffect.Projection = projection;
					}
					else
					{
						// Assume any reasonable effect has World, View and Projection
						Matrix worldMatrix = transforms[mesh.ParentBone.Index] * worldTransform;
						effect.Parameters["World"].SetValue(worldMatrix);
						effect.Parameters["View"].SetValue(view);
						effect.Parameters["Projection"].SetValue(projection);

						if (world.EntityProperties[entity.Id].Satisfies(FULL_LIGHT_EFFECT))
						{
							// Precalculate normal matrix used in effect
							Matrix worldViewMatrix = view * worldMatrix;
							Matrix worldViewInverse;
							Matrix.Invert(ref worldViewMatrix, out worldViewInverse);
							Matrix normalMatrix;
							Matrix.Transpose(ref worldViewInverse, out normalMatrix);

							effect.Parameters["NormalMatrix"].SetValue(normalMatrix);
							effect.Parameters["ViewSpaceLightPosition"].SetValue(Vector3.Transform(world.LightPosition, view));
						}
					}
				}
				mesh.Draw();
			}
		}
	}
}
