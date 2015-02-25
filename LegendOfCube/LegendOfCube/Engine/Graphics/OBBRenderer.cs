using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine.Graphics
{
	public class OBBRenderer
	{
		private readonly GraphicsDevice graphicsDevice;
		private readonly BasicEffect basicEffect;
		private readonly DynamicIndexBuffer indicies;
		private readonly DynamicVertexBuffer vertexBuffer;
		private readonly VertexPositionColor[] vertData;

		public OBBRenderer(GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			this.basicEffect = new BasicEffect(graphicsDevice);
			this.indicies = new DynamicIndexBuffer(graphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);
			this.vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.None);
			this.vertData = new VertexPositionColor[8];

			basicEffect.LightingEnabled = false;
			basicEffect.VertexColorEnabled = false;
			basicEffect.PreferPerPixelLighting = false;

			RefreshIndicies();
		}

		private void RefreshIndicies()
		{
			// TODO: Make these match OBB.Corners(). This works well enough for debugging though.
			indicies.SetData(new ushort[]
			{
				0,2,1, // -x
				1,2,3,

				4,5,6, // +x
				5,7,6,

				0,1,5, // -y
				0,5,4,
				
				2,6,7, // +y
				2,7,3,

				0,4,6, // -z
				0,6,2,
				
				1,3,7, // +z
				1,7,5,
			});
		}

		public void Render(ref OBB boundingBox, ref Matrix view, ref Matrix projection)
		{
			basicEffect.View = view;
			basicEffect.Projection = projection;

			// Let the OBB class do the transform, for testing
			basicEffect.World = Matrix.Identity;

			basicEffect.DiffuseColor = Color.White.ToVector3();

			if (indicies.IsContentLost)
			{
				RefreshIndicies();
			}

			Vector3[] corners = new Vector3[8];
			boundingBox.Corners(corners);

			for (int i = 0; i < vertData.Length; i++)
			{
				vertData[i] = new VertexPositionColor(corners[i], Color.White);
			}

			vertexBuffer.SetData(vertData);

			graphicsDevice.SetVertexBuffer(vertexBuffer);
			graphicsDevice.Indices = indicies;

			var originalRasterizerState = graphicsDevice.RasterizerState;

			var wireRasterizerState = new RasterizerState
			{
				FillMode = FillMode.WireFrame,
				CullMode = CullMode.None
			};

			graphicsDevice.RasterizerState = wireRasterizerState;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);
			}
			graphicsDevice.RasterizerState = originalRasterizerState;
		}
	}
}
