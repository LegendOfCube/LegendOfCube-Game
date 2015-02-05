using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Helper class to construct complex entities easily.
	/// </summary>
	public class EntityBuilder
	{
		private Properties properties;
		private Model model;
		private Vector3 velocity;
		private Vector3 acceleration;
		private Matrix transform = Matrix.Identity;

		public EntityBuilder WithModel(Model model)
		{
			properties.Add(Properties.MODEL);
			this.model = model;
			return this;
		}

		public EntityBuilder WithPosition(Vector3 position)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform.Translation = position;
			return this;
		}

		public EntityBuilder WithTransform(Matrix transform)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform = transform;
			return this;
		}

		public EntityBuilder WithVelocity(Vector3 velocity)
		{
			properties.Add(Properties.VELOCITY);
			this.velocity = velocity;
			return this;
		}

		public EntityBuilder WithAdditionalProperties(Properties properties)
		{
			this.properties.Add(properties);
			return this;
		}

		public Entity AddToWorld(World world)
		{
			Entity entity = world.CreateEntity(properties);
			Debug.WriteLine(entity.Id);
			Debug.WriteLine(Convert.ToString((int)properties.mask, 2));
			if (properties.Satisfies(new Properties(Properties.TRANSFORM)))
			{
				world.Transforms[entity.Id] = transform;
			}
			if (properties.Satisfies(new Properties(Properties.ACCELERATION)))
			{
				world.Accelerations[entity.Id] = acceleration;
			}
			if (properties.Satisfies(new Properties(Properties.VELOCITY)))
			{
				world.Velocities[entity.Id] = velocity;
			}
			if (properties.Satisfies(new Properties(Properties.MODEL)))
			{
				world.Models[entity.Id] = model;
			}
			return entity;
		}
	}
}
