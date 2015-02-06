using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Class responsible for keeping track of all entities.
	/// </summary>
	public class World
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties NO_COMPONENTS = new Properties(Properties.NO_PROPERTIES);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		public readonly UInt32 MaxNumEntities;
		public UInt32 NumEntities { get; private set; }
		public UInt32 HighestOccupiedId { get; private set; }

		// Describe what components an entity has
		public readonly Properties[] EntityProperties;
		public readonly Matrix[] Transforms;
		public readonly Vector3[] Velocities;
		public readonly Vector3[] Accelerations;
		public readonly Model[] Models;
		public readonly InputData[] InputData;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		public World(UInt32 maxNumEntities)
		{
			MaxNumEntities = maxNumEntities;
			NumEntities = 0;
			HighestOccupiedId = 0;
			EntityProperties = new Properties[MaxNumEntities];
			for (UInt32 i = 0; i < MaxNumEntities; i++) {
				EntityProperties[i] = NO_COMPONENTS;
			}

			// Components
			Velocities = new Vector3[MaxNumEntities];
			Accelerations = new Vector3[MaxNumEntities];
			Models = new Model[MaxNumEntities];
			Transforms = new Matrix[MaxNumEntities];
			InputData = new InputData[MaxNumEntities];
			for (UInt32 i = 0; i < MaxNumEntities; i++) {
				Velocities[i] = new Vector3(0, 0, 0);
				Accelerations[i] = new Vector3(0, 0, 0);
				Models[i] = null;
				Transforms[i] = Matrix.Identity;
				InputData[i] = null;
			}
		}

		// Public Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool CanCreateMoreEntities()
		{
			return NumEntities < MaxNumEntities;
		}

		public Entity CreateEntity(Properties wantedComponents)
		{
			if (wantedComponents == NO_COMPONENTS) {
				throw new ArgumentException("Entity must contain at least one component.");
			}
			if (!CanCreateMoreEntities()) {
				throw new InvalidOperationException("Can't create more entites.");
			}
			
			// Find free slot for new entity
			UInt32 entity;
			for (entity = 0; entity < MaxNumEntities; entity++) {
				if (EntityProperties[entity] == NO_COMPONENTS) {
					break;
				}
			}
			if (entity >= MaxNumEntities) {
				throw new InvalidOperationException("Something went terribly wrong.");
			}

			// Set ComponentMask at the free slot to the wanted components to create the entity.
			EntityProperties[entity] = wantedComponents;
			NumEntities++;
			HighestOccupiedId = Math.Max(entity, HighestOccupiedId);
			return new Entity(entity);
		}

		/// <summary>
		/// Enumerator for entities in the world, with a property filter.
		/// </summary>
		/// <example>
		/// <code>
		/// for(var e in EnumerateEntities(property)) {...}
		/// </code>
		/// </example>
		/// <param name="filter">The property filter to apply</param>
		/// <returns>An enumerator object</returns>
		public IEnumerable<Entity> EnumerateEntities(Properties filter)
		{
			for (UInt32 i = 0; i <= HighestOccupiedId; i++)
			{
				if (EntityProperties[i].Satisfies(filter))
				{
					yield return new Entity(i);
				}
			}
		}

		/// <summary>
		/// Enumerator for entities in the world.
		/// </summary>
		/// <example>
		/// <code>
		/// for(var e in EnumerateEntities)) {...}
		/// </code>
		/// </example>
		/// <returns>An enumerator object</returns>
		public IEnumerable EnumerateEntities()
		{
			for (UInt32 i = 0; i <= HighestOccupiedId; i++)
			{
				yield return new Entity(i);
			}
		}

		public void DestroyEntity(Entity entityToDestroy)
		{
			if (entityToDestroy.Id >= MaxNumEntities) {
				throw new ArgumentException("Entity to be destroyed doesn't exist.");
			}

			// Set ComponentMask at entity slot to NO_COMPONENTS to destroy it.
			EntityProperties[entityToDestroy.Id] = NO_COMPONENTS;
			NumEntities--;

			// Clean-up components
			Velocities[entityToDestroy.Id] = new Vector3(0, 0, 0);
			Accelerations[entityToDestroy.Id] = new Vector3(0, 0, 0);
			Models[entityToDestroy.Id] = null;
			Transforms[entityToDestroy.Id] = Matrix.Identity;
		}

	}
}
