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
		public VertexPositionColor[] Verts;
		public DynamicVertexBuffer VertexBuffer;
		public DynamicIndexBuffer IndexBuffer;
		public Matrix ModelToWorld;
		public Vector3 Vel;
	}
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private readonly GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Model barrelModel;
		private BasicEffect basicEffect;
		private Texture2D cubeImage;
		private GfxObj cube;
		private GfxObj ground;
		private Matrix[] barrels;

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

		private bool doubleJump = true;
		private float fov = 45.0f;

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
			barrels = new Matrix[100];

			Random rnd = new Random();
			for (int i = 0; i < barrels.Length; i++)
			{
				barrels[i] = Matrix.CreateTranslation(rnd.Next(-1000, 1000), 1f, rnd.Next(-1000, 1000)) * Matrix.CreateScale(0.1f);
			}

			Window.AllowUserResizing = true;
			graphics.PreferMultiSampling = true;
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			graphics.ApplyChanges();

			basicEffect = new BasicEffect(graphics.GraphicsDevice);

			cube = new GfxObj {Verts = new VertexPositionColor[8]};
			cube.Verts[0] = new VertexPositionColor(new Vector3(-0.5f, 0.0f, -0.5f), new Color(0.0f, 0.0f, 0.0f));
			cube.Verts[1] = new VertexPositionColor(new Vector3(-0.5f, 0.0f, 0.5f), new Color(0.0f, 0.0f, 1.0f));
			cube.Verts[2] = new VertexPositionColor(new Vector3(-0.5f, 1.0f, -0.5f), new Color(0.0f, 1.0f, 0.0f));
			cube.Verts[3] = new VertexPositionColor(new Vector3(-0.5f, 1.0f, 0.5f), new Color(0.0f, 1.0f, 1.0f));
			cube.Verts[4] = new VertexPositionColor(new Vector3(0.5f, 0.0f, -0.5f), new Color(1.0f, 0.0f, 0.0f));
			cube.Verts[5] = new VertexPositionColor(new Vector3(0.5f, 0.0f, 0.5f), new Color(1.0f, 0.0f, 1.0f));
			cube.Verts[6] = new VertexPositionColor(new Vector3(0.5f, 1.0f, -0.5f), new Color(1.0f, 1.0f, 0.0f));
			cube.Verts[7] = new VertexPositionColor(new Vector3(0.5f, 1.0f, 0.5f), new Color(1.0f, 1.0f, 1.0f));

			cube.VertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
			cube.IndexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);
			cube.VertexBuffer.SetData(cube.Verts);
			cube.IndexBuffer.SetData(cubeIndices);
			cube.ModelToWorld = Matrix.Identity;

			ground = new GfxObj {Verts = new VertexPositionColor[4]};
			ground.Verts[0] = new VertexPositionColor(new Vector3(-1000, 0f, -1000), Color.SlateGray);
			ground.Verts[1] = new VertexPositionColor(new Vector3(-1000, 0f, 1000), Color.SlateGray);
			ground.Verts[2] = new VertexPositionColor(new Vector3(1000, 0f, 1000), Color.SlateGray);
			ground.Verts[3] = new VertexPositionColor(new Vector3(1000, 0f, -1000), Color.SlateGray);

			ground.VertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
			ground.IndexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 6, BufferUsage.WriteOnly);

			ground.VertexBuffer.SetData(ground.Verts);
			ground.IndexBuffer.SetData(new ushort[]
			{
				0,2,1,
				3,2,0
			});
			ground.ModelToWorld = Matrix.Identity;

			Window.ClientSizeChanged += SizeChanged;

			base.Initialize();
		}

		void SizeChanged(object o, EventArgs e)
		{
			// For some reason, buffers seem to need updating after resize
			cube.VertexBuffer.SetData(cube.Verts);
			cube.IndexBuffer.SetData(cubeIndices);

			ground.VertexBuffer.SetData(ground.Verts);
			ground.IndexBuffer.SetData(new ushort[]
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
				cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Forward) * cube.ModelToWorld;
			}
			if (keyState.IsKeyDown(Keys.A))
			{
				cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Left) * cube.ModelToWorld;
			}
			if (keyState.IsKeyDown(Keys.S))
			{
				cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Backward) * cube.ModelToWorld;
			}
			if (keyState.IsKeyDown(Keys.D))
			{
				cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Right) * cube.ModelToWorld;
			}
			if (keyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space))
			{
				if (cube.ModelToWorld.Translation.Y == 0)
				{
					cube.Vel.Y += 0.21f;
				}
				else if (cube.ModelToWorld.Translation.Y > 0 && doubleJump) 
				{
					cube.Vel.Y += 0.21f;
					doubleJump = false;
				}
			}

			oldKeyState = keyState;

			// Epic physics handling
			cube.Vel.Y -= 0.01f;
			cube.ModelToWorld = Matrix.CreateTranslation(cube.Vel) * cube.ModelToWorld;
			Vector3 pos = cube.ModelToWorld.Translation;
			if (pos.Y < 0)
			{
				pos.Y = 0;
				cube.Vel.Y = 0;
				doubleJump = true;
			}
			cube.ModelToWorld.Translation = pos;

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
			
			Vector3 cameraPos = new Vector3(cube.ModelToWorld.Translation.X+1.5f, 4f, cube.ModelToWorld.Translation.Z+8f);

			Matrix view = Matrix.CreateLookAt(cameraPos, cube.ModelToWorld.Translation, new Vector3(0f, 1f, 0f));
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

			GraphicsDevice.Indices = cube.IndexBuffer;
			GraphicsDevice.SetVertexBuffer(cube.VertexBuffer);

			basicEffect.World = cube.ModelToWorld;
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.VertexColorEnabled = true;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 36);
			}

			basicEffect.World = ground.ModelToWorld;
			GraphicsDevice.Indices = ground.IndexBuffer;
			GraphicsDevice.SetVertexBuffer(ground.VertexBuffer);

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
			}

			Matrix barrelWorld1 = Matrix.CreateTranslation(25f, 1f, -25f) * Matrix.CreateScale(0.1f);
			Matrix barrelWorld2 = Matrix.CreateTranslation(25f, 1f, 25f) * Matrix.CreateScale(0.1f);
			Matrix barrelWorld3 = Matrix.CreateTranslation(-25f, 1f, -25f) * Matrix.CreateScale(0.1f);
			Matrix barrelWorld4 = Matrix.CreateTranslation(-25f, 1f, 25f) * Matrix.CreateScale(0.1f);
			barrelModel.Draw(barrelWorld1, view, projection);
			barrelModel.Draw(barrelWorld2, view, projection);
			barrelModel.Draw(barrelWorld3, view, projection);
			barrelModel.Draw(barrelWorld4, view, projection);

			foreach (Matrix barrel in barrels)
			{
				barrelModel.Draw(barrel, view, projection);	
			}

			spriteBatch.Begin();
			spriteBatch.Draw(cubeImage, new Rectangle(0, 0, 200, 200), Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
