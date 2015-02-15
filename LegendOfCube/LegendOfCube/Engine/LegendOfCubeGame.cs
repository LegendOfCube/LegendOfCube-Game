using System;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine.BoundingVolumes;

namespace LegendOfCube.Engine
{

	public class LegendOfCubeGame : Microsoft.Xna.Framework.Game
	{

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private World world;
		private InputSystem inputSystem;
		private PhysicsSystem physicsSystem;
		private RenderSystem renderSystem;
		private GameplaySystem gameplaySystem;

		private Entity playerEntity;
		private Entity[] otherCubes;
		private Entity ground;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public LegendOfCubeGame()
		{
			world = new World(1002);
			inputSystem = new InputSystem(this);
			renderSystem = new RenderSystem(this);
			physicsSystem = new PhysicsSystem();
			gameplaySystem = new GameplaySystem();
			Content.RootDirectory = "Content";
		}

		//Temp entityFactory with an empty prop.
		public Entity CreateEntity(Properties props)
		{
			return world.CreateEntity(props);
		}

		// Overriden XNA methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			renderSystem.Initialize();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			var cubeModel = Content.Load<Model>("Models/cube_plain");

			var playerEffect = new StandardEffectParams
			{
				DiffuseTexture = Content.Load<Texture>("Models/cube_diff"),
				EmissiveTexture = Content.Load<Texture>("Models/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Content.Load<Texture>("Models/cube_normal"),
				SpecularColor = Color.White.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.Gray.ToVector4(),
				SpecularColor = 0.5f * Color.White.ToVector4()
			};

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(Vector3.Zero)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG | Properties.FRICTION_FLAG))
					.AddToWorld(world);

			// TODO: Ugly hack to add bv to cube.
			world.EntityProperties[playerEntity.Id].Add(Properties.BOUNDING_VOLUME);
			world.BVs[playerEntity.Id] = new OBB(world.Transforms[playerEntity.Id].Translation,
												 new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
												 new Vector3(1, 1, 1));

			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				bool noOverlap;
				OBB bv;
				do
				{
					noOverlap = true;
					float scale = rnd.Next(1, 25);
					bv = new OBB(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)),
					             new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
					             new Vector3(scale, scale, scale));

					for (int j = 1; j < i; j++) // TODO: Hack. Starts at 1 to avoid player entity.
					{
						if (IntersectionsTests.Intersects(ref bv, ref world.BVs[j]))
						{
							noOverlap = false;
							break;
						}
					}

				} while (!noOverlap);

				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(bv.ExtentX))
						.WithPosition(bv.Position)
						.WithStandardEffectParams(otherCubeEffect)
						.AddToWorld(world);

				world.BVs[i] = bv; // TODO: Hack, add properly to EntityBuilder
				world.EntityProperties[i].Add(Properties.BOUNDING_VOLUME);
			}

			// This is definitely the most natural way to represent the ground
			ground =
				new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(1000.0f))
					.WithPosition(new Vector3(0, -1000.0f, 0))
					.WithStandardEffectParams(groundEffect)
					.AddToWorld(world);
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
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			inputSystem.ApplyInput(gameTime, world);
			gameplaySystem.ProcessInputData(world, delta);
			physicsSystem.ApplyPhysics(delta, world); // Note, delta should be fixed time step.

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

			renderSystem.RenderWorld(world);
			base.Draw(gameTime);
		}

		// Helper methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


	}
}
