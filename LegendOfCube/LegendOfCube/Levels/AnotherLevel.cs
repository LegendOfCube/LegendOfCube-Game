using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Levels
{
	class AnotherLevel : ILevelFactory
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
				.WithVelocity(Vector3.Zero, 30)
				.WithAcceleration(Vector3.Zero, 60)
				.WithAdditionalProperties(
					new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG));

			var platformBuilder = new EntityBuilder()
				.WithModelData(contentCollection.RustPlatform);

			var wallBuilder = new EntityBuilder()
				.WithModelData(contentCollection.BrickWall);

			var deathWallBuilder = wallBuilder.Copy()
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.WithStandardEffectParams(wallDeathEffect);

			var sawTrap = wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(1, 2, 1)*Matrix.CreateRotationY(MathHelper.ToRadians(90))*
				               Matrix.CreateRotationZ(MathHelper.ToRadians(45)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.WithStandardEffectParams(new StandardEffectParams()
				{
					DiffuseColor = Color.Gray.ToVector4()
				});

			world.Player = playerBuilder.AddToWorld(world);

			/*
			 * ¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸¸.·´¯`·.´¯`·.¸¸.·´¯`·.¸><(((º>
			 */

			// Starting platform
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(6, 5, 1.5f))
				.WithPosition(Vector3.Zero)
				.AddToWorld(world);

			// First Wall
			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5,30,3))
				.WithPosition(new Vector3(30, 0, 0))
				.AddToWorld(world);

			// Second platform
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(4, 7.5f, 1.5f))
				.WithPosition(new Vector3(-10, 15f, 0))
				.AddToWorld(world);

			// First saw deathtrap
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, -6))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, -4))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, -2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, 0))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, 2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, 4f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 13, 6))
				.AddToWorld(world);

			// Second Wall
			wallBuilder.Copy()
				.WithTransform(Matrix.CreateScale(5, 30, 3))
				.WithPosition(new Vector3(-30, 0, 0))
				.AddToWorld(world);

			// Third platform
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(4, 7.5f, 1.5f))
				.WithPosition(new Vector3(10, 30f, 0))
				.AddToWorld(world);

			// Second saw deathtrap
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, -6))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, -4))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, -2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, 0))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, 2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, 4f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(20, 28, 6))
				.AddToWorld(world);

			// Fourth platform
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2, 5, 1.5f))
				.WithPosition(new Vector3(0, 45f, 0))
				.AddToWorld(world);

			// Third saw deathtrap
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, -6))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, -4))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, -2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, 0))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, 2f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, 4f))
				.AddToWorld(world);
			sawTrap.Copy()
				.WithPosition(new Vector3(8, 43, 6))
				.AddToWorld(world);


			// Finnish 
			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2, 2.5f, 1.5f))
				.WithPosition(new Vector3(-20, 43.75f, 0))
				.AddToWorld(world);

			platformBuilder.Copy()
				.WithTransform(Matrix.CreateScale(2, 2.5f, 1.5f))
				.WithPosition(new Vector3(-20, 45f, 0))
				.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
				.AddToWorld(world);

			//Falling death
			new EntityBuilder()
				.WithPosition(new Vector3(0, -100.0f, 0))
				.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1000, 1, 1000)))
				.WithAdditionalProperties(new Properties(Properties.DEATH_ZONE_FLAG))
				.AddToWorld(world);

			return world;
		}
	}
}
