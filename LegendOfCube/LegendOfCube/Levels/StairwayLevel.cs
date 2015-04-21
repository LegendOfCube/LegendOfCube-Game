using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	class StairwayLevel : Level
	{
		public StairwayLevel() : base("Test: Stairway Level") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000)
			{
				SpawnPoint = new Vector3(0, 0, 0)
			};
			world.Camera.Position = world.SpawnPoint + new Vector3(-1.0f, 3.0f, 0.0f);
			world.LightDirection = Vector3.Normalize(new Vector3(1, -0.3f, 1));
			world.AmbientIntensity = 0.3f;

			var player = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 0)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG))
				.AddToWorld(world);
			world.Player = player;

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.GreenYellow.ToVector4()
			};

			// Add ground
			new EntityBuilder()
				.WithModelData(contentCollection.PlainCube)
				.WithStandardEffectParams(groundEffect)
				.WithTransform(Matrix.CreateTranslation(0.0f, -0.5f, 0.0f) * Matrix.CreateScale(5000.0f, 1.0f, 5000.0f))
				.AddToWorld(world);

			var stepBuilder1 = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(new StandardEffectParams { DiffuseColor = Color.BlueViolet.ToVector4() })
				.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100.0f));

			var stepBuilder2 = stepBuilder1.Copy()
				.WithStandardEffectParams(new StandardEffectParams { DiffuseColor = Color.AliceBlue.ToVector4() })
				.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100.0f));

			for (int i = 1; i <= 100; i++)
			{
				float x = i * 3.0f;
				float y = (float)Math.Pow(i, 1.2) * 0.2f;
				var stairBuilder = i % 2 == 0 ? stepBuilder1 : stepBuilder2;
				stairBuilder.Copy()
					.WithPosition(new Vector3(x, y, 0))
					.AddToWorld(world);
			}

			return world;
		}
	}
}
