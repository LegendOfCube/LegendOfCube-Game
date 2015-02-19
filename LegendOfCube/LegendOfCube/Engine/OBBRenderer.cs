using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class OBBRenderer
	{
		private readonly GraphicsDevice graphicsDevice;
		private readonly BasicEffect basicEffect;
		private DynamicIndexBuffer indicies;

		public OBBRenderer(GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			this.basicEffect = new BasicEffect(graphicsDevice);

			basicEffect.LightingEnabled = false;
			basicEffect.VertexColorEnabled = false;
		}

		public void Render(OBB boundingBox, Matrix view, Matrix projection)
		{
			basicEffect.View = view;
			basicEffect.Projection = projection;

			// Let the OBB class do the transform, for testing
			basicEffect.World = Matrix.Identity;

			basicEffect.DiffuseColor = Color.White.ToVector3();

			indicies = new DynamicIndexBuffer(graphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);

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

			var vertData = new VertexPositionColor[8];
			var vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertData.Length, BufferUsage.None);

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

			var wireRasterizerState = new RasterizerState();
			wireRasterizerState.FillMode = FillMode.WireFrame;
			wireRasterizerState.CullMode = CullMode.None;

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
