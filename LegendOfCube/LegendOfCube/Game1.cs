using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LegendOfCube
{
	struct GfxObj
	{
		public VertexPositionColor[] verts;
		public DynamicVertexBuffer vertexBuffer;
		public DynamicIndexBuffer indexBuffer;
		public Matrix modelToWorld;
		public Vector3 vel;
	}
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Model barrelModel;
		private BasicEffect basicEffect;
		private float rotation;
		private Texture2D cubeImage;
		private GfxObj cube;
		private GfxObj ground;

		private readonly ushort[] cubeIndices =
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
		};

		// Used for detecting that a key recently has been pressed
		private KeyboardState oldKeyState;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Window.AllowUserResizing = true;
			graphics.PreferMultiSampling = true;
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			graphics.ApplyChanges();

			basicEffect = new BasicEffect(graphics.GraphicsDevice);

			cube = new GfxObj();
			cube.verts = new VertexPositionColor[8];
			cube.verts[0] = new VertexPositionColor(new Vector3(-0.5f, 0.0f, -0.5f), new Color(0.0f, 0.0f, 0.0f));
			cube.verts[1] = new VertexPositionColor(new Vector3(-0.5f, 0.0f, 0.5f), new Color(0.0f, 0.0f, 1.0f));
			cube.verts[2] = new VertexPositionColor(new Vector3(-0.5f, 1.0f, -0.5f), new Color(0.0f, 1.0f, 0.0f));
			cube.verts[3] = new VertexPositionColor(new Vector3(-0.5f, 1.0f, 0.5f), new Color(0.0f, 1.0f, 1.0f));
			cube.verts[4] = new VertexPositionColor(new Vector3(0.5f, 0.0f, -0.5f), new Color(1.0f, 0.0f, 0.0f));
			cube.verts[5] = new VertexPositionColor(new Vector3(0.5f, 0.0f, 0.5f), new Color(1.0f, 0.0f, 1.0f));
			cube.verts[6] = new VertexPositionColor(new Vector3(0.5f, 1.0f, -0.5f), new Color(1.0f, 1.0f, 0.0f));
			cube.verts[7] = new VertexPositionColor(new Vector3(0.5f, 1.0f, 0.5f), new Color(1.0f, 1.0f, 1.0f));

			cube.vertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
			cube.indexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);
			cube.vertexBuffer.SetData(cube.verts);
			cube.indexBuffer.SetData(cubeIndices);
			cube.modelToWorld = Matrix.Identity;

			ground = new GfxObj();
			ground.verts = new VertexPositionColor[4];
			ground.verts[0] = new VertexPositionColor(new Vector3(-1000, 0f, -1000), Color.SlateGray);
			ground.verts[1] = new VertexPositionColor(new Vector3(-1000, 0f, 1000), Color.SlateGray);
			ground.verts[2] = new VertexPositionColor(new Vector3(1000, 0f, 1000), Color.SlateGray);
			ground.verts[3] = new VertexPositionColor(new Vector3(1000, 0f, -1000), Color.SlateGray);

			ground.vertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
			ground.indexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 6, BufferUsage.WriteOnly);

			ground.vertexBuffer.SetData(ground.verts);
			ground.indexBuffer.SetData(new ushort[]
			{
				0,2,1,
				3,2,0
			});
			ground.modelToWorld = Matrix.Identity;

			Window.ClientSizeChanged += SizeChanged;

			base.Initialize();
		}

		void SizeChanged(object o, EventArgs e)
		{
			// For some reason, buffers seem to need updating after resize
			cube.vertexBuffer.SetData(cube.verts);
			cube.indexBuffer.SetData(cubeIndices);

			ground.vertexBuffer.SetData(ground.verts);
			ground.indexBuffer.SetData(new ushort[]
			{
				0,2,1,
				3,2,0  
			});
		}


		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			barrelModel = Content.Load<Model>("barrel");
			cubeImage = Content.Load<Texture2D>("cube");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyState = Keyboard.GetState();

			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}
			if (keyState.IsKeyDown(Keys.W))
			{
				cube.modelToWorld = Matrix.CreateTranslation(0.1f * cube.modelToWorld.Left) * cube.modelToWorld;
			}
			if (keyState.IsKeyDown(Keys.A))
			{
				cube.modelToWorld = Matrix.CreateTranslation(0.1f * cube.modelToWorld.Backward) * cube.modelToWorld;
			}
			if (keyState.IsKeyDown(Keys.S))
			{
				cube.modelToWorld = Matrix.CreateTranslation(0.1f * cube.modelToWorld.Right) * cube.modelToWorld;
			}
			if (keyState.IsKeyDown(Keys.D))
			{
				cube.modelToWorld = Matrix.CreateTranslation(0.1f * cube.modelToWorld.Forward) * cube.modelToWorld;
			}
			if (keyState.IsKeyDown(Keys.Space) && cube.modelToWorld.Translation.Y == 0)
			{
				cube.vel.Y += 0.18f;
			}

			oldKeyState = keyState;

			// Epic physics handling
			cube.vel.Y -= 0.01f;
			cube.modelToWorld = Matrix.CreateTranslation(cube.vel) * cube.modelToWorld;
			Vector3 pos = cube.modelToWorld.Translation;
			if (pos.Y < 0)
			{
				pos.Y = 0;
				cube.vel.Y = 0;
			}
			cube.modelToWorld.Translation = pos;

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			float fov = 45;

			Matrix view = Matrix.CreateLookAt(new Vector3(7f, 2f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f));
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

			GraphicsDevice.Indices = cube.indexBuffer;
			GraphicsDevice.SetVertexBuffer(cube.vertexBuffer);

			basicEffect.World = cube.modelToWorld;
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.VertexColorEnabled = true;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 36);
			}

			basicEffect.World = ground.modelToWorld;
			GraphicsDevice.Indices = ground.indexBuffer;
			GraphicsDevice.SetVertexBuffer(ground.vertexBuffer);

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
			}

			Matrix barrelWorld = Matrix.CreateTranslation(0f, 1f, -15f) * Matrix.CreateScale(0.1f);
			barrelModel.Draw(barrelWorld, view, projection);

			spriteBatch.Begin();
			spriteBatch.Draw(cubeImage, new Rectangle(0, 0, 200, 200), Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
