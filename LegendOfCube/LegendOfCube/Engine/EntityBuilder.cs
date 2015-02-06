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

		/// <summary>
		/// Assign a model for the entity being built.
		/// </summary>
		/// <param name="model">The XNA 3D model</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithModel(Model model)
		{
			properties.Add(Properties.MODEL);
			this.model = model;
			return this;
		}

		/// <summary>
		/// Assign a position for the entity being built. If WithTransform
		/// has been called, the translation data will be replaced, but the
		/// rest will remain unchanged.
		/// </summary>
		/// <param name="position">The position in world space</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithPosition(Vector3 position)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform.Translation = position;
			return this;
		}

		/// <summary>
		/// Assign a transform matrix for the entity being built. This will override
		/// the effect of having prevously called WithPosition.
		/// </summary>
		/// <param name="transform">The model-to-world matrix</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithTransform(Matrix transform)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform = transform;
			return this;
		}

		/// <summary>
		/// Assign a velocity for the entity being built.
		/// </summary>
		/// <param name="velocity">The initial velocity for the entity</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithVelocity(Vector3 velocity)
		{
			properties.Add(Properties.VELOCITY);
			this.velocity = velocity;
			return this;
		}

		/// <summary>
		/// Assign an acceleration for the entity being built.
		/// </summary>
		/// <param name="acceleration">The initial acceleration for the entity</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithAcceleration(Vector3 acceleration)
		{
			properties.Add(Properties.ACCELERATION);
			this.acceleration = acceleration;
			return this;
		}

		/// <summary>
		/// Add any property flags to the entity being created.
		/// </summary>
		/// <param name="properties">The properties to add</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithAdditionalProperties(Properties properties)
		{
			this.properties.Add(properties);
			return this;
		}

		/// <summary>
		/// Adds an Entity to the world, with the properties given to the builder.
		/// </summary>
		/// <param name="world">The world to add the entity to</param>
		/// <returns>A representation of the Entity</returns>
		public Entity AddToWorld(World world)
		{
			Entity entity = world.CreateEntity(properties);

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
			if (properties.Satisfies(new Properties(Properties.INPUT_FLAG)))
			{
				// Not entirely sure if INPUT_FLAG implies having InputData
				world.InputData[entity.Id] = new InputDataImpl();
			}
			return entity;
		}
	}
}
