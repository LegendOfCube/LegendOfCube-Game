using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class ConceptLevel : ILevelFactory
	{
		public World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(1000);

			world.SpawnPoint = new Vector3(0, 5, 0);
			world.LightDirection = Vector3.Normalize(new Vector3(3.0f, -1.0f, -3.0f));
			world.CameraPosition = world.SpawnPoint + new Vector3(-3, 0, 0);
			world.AmbientIntensity = 0.45f;

			var wallDeathEffect = contentCollection.BrickWall.EffectParams.ShallowCopy();
			wallDeathEffect.DiffuseColor = Color.Red.ToVector4();

			var bounceEffect = contentCollection.RustPlatform.EffectParams.ShallowCopy();
			bounceEffect.DiffuseColor = Color.Yellow.ToVector4();

			var platformDeathEffect = contentCollection.RustPlatform.EffectParams.ShallowCopy(); 
			platformDeathEffect.DiffuseColor = Color.DarkRed.ToVector4();

			var playerBuilder = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 0)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG));

			var platformBuilder = new EntityBuilder()
				.WithModelData(contentCollection.RustPlatform);

			var wallBuilder = new EntityBuilder()
				.WithModelData(contentCollection.BrickWall);

			var deathWallBuilder = wallBuilder.Copy()
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.WithStandardEffectParams(wallDeathEffect);

			world.Player = playerBuilder.AddToWorld(world);

			// Starting platform
			platformBuilder.Copy()
				.WithPosition(0, 0, 0)
				.AddToWorld(world);

			// Walls and platform to test length gaining wall jumps
			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2, 10, 8))
				.WithPosition(-5, 0, 30)
				.AddToWorld(world);

			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2, 10, 8))
				.WithPosition(5, 0, 30)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 9, 60)
				.AddToWorld(world);

			// Platform to test normal jump
			platformBuilder.Copy()
				.WithPosition(0, 0, -25)
				.AddToWorld(world);

			// Wall and platform to test height gaining wall jumps
			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(10, 3, 0)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 12, 0)
				.AddToWorld(world);

			//Petor request
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(4, 1, 1))
				.WithPosition(-35, 0, 0)
				.AddToWorld(world);

			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithVelocity(Vector3.UnitX * -7, 0)
				.WithPosition(-55, 0, 0)
				.WithAI(new []{new Vector3(-55, 0, 0), new Vector3(-15, 0, 0)}, true)
				.AddToWorld(world);

			// Bounce test jump
			platformBuilder.Copy()
				.WithPosition(0, 0, -75)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, -25, -50)
				.WithStandardEffectParams(bounceEffect)
				.WithAdditionalProperties(new Properties(Properties.BOUNCE_FLAG))
				.AddToWorld(world);

			//Help bounce platform
			platformBuilder.Copy()
				.WithPosition(-10, 0, -50)
				.AddToWorld(world);

			//Long jump
			platformBuilder.Copy()
				.WithPosition(25, 0, -25)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(63, 0, -25)
				.AddToWorld(world);

			//Small platforms
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(35, 0, -35)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(45, 0, -35)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(0.4f))
				.WithPosition(55, 0, -35)
				.AddToWorld(world);

			//Trick jump
			platformBuilder.Copy()
				.WithPosition(90, 0, -25)
				.AddToWorld(world);

			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(75, 1, -25)
				.AddToWorld(world);

			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(75, 9, -25)
				.AddToWorld(world);

			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(75, 4.5f, -32)
				.AddToWorld(world);

			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(75, 4.5f, -18)
				.AddToWorld(world);

			//Crush trap#1
			platformBuilder.Copy()
				.WithPosition(0, 15, -85)
				.WithVelocity(Vector3.UnitY * 20, 0)
				.WithStandardEffectParams(platformDeathEffect)
				.WithAI(new[] {new Vector3(0, 15, -85), new Vector3(0, 1, -85)}, true )
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 0, -85)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 0, -95)
				.AddToWorld(world);

			//Crush trap#2
			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(-7, 1, -105)
				.WithVelocity(Vector3.UnitX * 10, 0)
				.WithAI(new [] {new Vector3(-7, 0.4f, -105), new Vector3(-0.7f, 0.4f, -105)}, true)
				.AddToWorld(world);

			deathWallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2))
				.WithPosition(7, 1, -105)
				.WithVelocity(Vector3.UnitX * -10, 0)
				.WithAI(new[] { new Vector3(7, 0.4f, -105), new Vector3(0.7f, 0.4f, -105) }, true)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 0, -105)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(0, 0, -115)
				.AddToWorld(world);

			//More wall jumps
			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3))
				.WithPosition(-7, 10, 72)
				.AddToWorld(world);

			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(0, 16, 81)
				.AddToWorld(world);

			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3))
				.WithPosition(7, 22, 72)
				.AddToWorld(world);

			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(3) * Matrix.CreateRotationY(MathHelper.ToRadians(90)))
				.WithPosition(0, 26, 63)
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithPosition(-15, 30, 72)
				.AddToWorld(world);

			//Falling death
			new EntityBuilder()
				.WithTransform(Matrix.CreateScale(1900))
				.WithPosition(0, -2000.0f, 0)
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			return world;
		}
	}
}
