using System;
using System.Collections.Generic;
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
    struct Cube
    {
        public VertexPositionColor[] verts;
        public DynamicVertexBuffer vertexBuffer;
        public DynamicIndexBuffer indexBuffer;
        public Matrix modelToWorld;
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
        private Cube cube;

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
            basicEffect = new BasicEffect(graphics.GraphicsDevice);

            cube = new Cube();
            cube.verts = new VertexPositionColor[8];
            cube.verts[0] = new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f));
            cube.verts[1] = new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 1.0f));
            cube.verts[2] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 1.0f, 0.0f));
            cube.verts[3] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 1.0f, 1.0f));
            cube.verts[4] = new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), new Color(1.0f, 0.0f, 0.0f));
            cube.verts[5] = new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), new Color(1.0f, 0.0f, 1.0f));
            cube.verts[6] = new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), new Color(1.0f, 1.0f, 0.0f));
            cube.verts[7] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f));

            cube.vertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            cube.indexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);
            cube.vertexBuffer.SetData(cube.verts);
            cube.indexBuffer.SetData(cubeIndices);
            cube.modelToWorld = Matrix.Identity;

            Window.ClientSizeChanged += new EventHandler<EventArgs>(SizeChanged);

            base.Initialize();
        }

        void SizeChanged(object o, EventArgs e)
        {
            // For some reason, buffers seem to need updating after resize
            cube.vertexBuffer.SetData(cube.verts);
            cube.indexBuffer.SetData(cubeIndices);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

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

            cube.modelToWorld *= Matrix.CreateRotationY(0.045f);

            Matrix view = Matrix.CreateLookAt(new Vector3(5f, 3f, 0f), Vector3.Zero, new Vector3(0f, 1f, 0f));
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

            Matrix barrelWorld = Matrix.CreateTranslation(0f, 0f, -15f) * Matrix.CreateScale(0.1f);
            barrelModel.Draw(barrelWorld, view, projection);

            spriteBatch.Begin();
            spriteBatch.Draw(cubeImage, new Rectangle(0, 0, 200, 200), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
