using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	class GameScreen : Screen
	{

		private readonly InputSystem inputSystem;
		private readonly GameplaySystem gameplaySystem;
		private readonly PhysicsSystem physicsSystem;

		private Entity playerEntity;
		private Entity[] otherCubes;
		private Entity ground;

		public GameScreen(Game game) : base(game)
		{
			World = new World(1002);
			inputSystem = new InputSystem(game);
			gameplaySystem = new GameplaySystem();
			physicsSystem = new PhysicsSystem();
		}

		protected internal override void Update(GameTime gameTime, SwitcherSystem switcher)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			inputSystem.ApplyInput(gameTime, World, switcher);
			gameplaySystem.ProcessInputData(World, delta);
			physicsSystem.ApplyPhysics(delta, World); // Note, delta should be fixed time step.
		}

		protected internal override void Draw(GameTime gameTime, RenderSystem renderSystem)
		{
			Game.GraphicsDevice.Clear(Color.CornflowerBlue);
			Game.GraphicsDevice.BlendState = BlendState.Opaque;
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			renderSystem.RenderWorld(World);
		}

		internal override void LoadContent()
		{

			var cubeModel = Game.Content.Load<Model>("Models/cube_plain");

			var playerEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				DiffuseTexture = Game.Content.Load<Texture>("Models/cube_diff"),
				SpecularTexture = Game.Content.Load<Texture>("Models/cube_specular"),
				EmissiveTexture = Game.Content.Load<Texture>("Models/cube_emissive"),
				NormalTexture = Game.Content.Load<Texture>("Models/cube_normal"),
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
					.AddToWorld(World);

			otherCubes = new Entity[1000];
			Random rnd = new Random(0);
			for (int i = 0; i < otherCubes.Length; i++)
			{
				otherCubes[i] =
					new EntityBuilder().WithModel(cubeModel)
						.WithTransform(Matrix.CreateScale(rnd.Next(1, 25)))
						.WithPosition(new Vector3(rnd.Next(-500, 500), rnd.Next(0, 1), rnd.Next(-500, 500)))
						.WithStandardEffectParams(otherCubeEffect)
						.AddToWorld(World);
			}

			// This is definitely the most natural way to represent the ground
			ground =
				new EntityBuilder().WithModel(cubeModel)
					.WithTransform(Matrix.CreateScale(1000.0f))
					.WithPosition(new Vector3(0, -1000.0f, 0))
					.WithStandardEffectParams(groundEffect)
					.AddToWorld(World);
		}
	}
}
