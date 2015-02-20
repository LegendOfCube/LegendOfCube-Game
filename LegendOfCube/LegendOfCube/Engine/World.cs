using System;
using System.Collections;
using System.Collections.Generic;
using LegendOfCube.Engine.Graphics;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine.BoundingVolumes;

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
		public UInt32 NumEntities;
		public UInt32 HighestOccupiedId;

		// Describe what components an entity has
		public readonly Properties[] EntityProperties ;
		public readonly Matrix[] Transforms;
		public readonly Vector3[] Velocities;
		public readonly Vector3[] Accelerations;
		public readonly InputData[] InputData;
		public readonly float[] MaxSpeed;
		public readonly float[] MaxAcceleration;
		public OBB[] ModelSpaceBVs;

		public readonly Model[] Models;
		public readonly StandardEffectParams[] StandardEffectParams;

		// Player state
		public PlayerCubeState PlayerCubeState;

		// Shortcut to player entity which there will be one instance of
		public Entity Player;

		// World variables

		public Vector3 SpawnPoint;
		public EventBuffer EventBuffer;

		public Vector3 Gravity;
		public Vector3 LightPosition;
		public Vector3 CameraPosition;

		public DebugState DebugState;

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
			Transforms = new Matrix[MaxNumEntities];
			InputData = new InputData[MaxNumEntities];
			MaxSpeed = new float[MaxNumEntities];
			MaxAcceleration = new float[MaxNumEntities];
			ModelSpaceBVs = new OBB[MaxNumEntities];

			Models = new Model[MaxNumEntities];
			StandardEffectParams = new StandardEffectParams[MaxNumEntities];
			for (UInt32 i = 0; i < MaxNumEntities; i++) {
				Velocities[i] = new Vector3(0, 0, 0);
				Accelerations[i] = new Vector3(0, 0, 0);
				Transforms[i] = Matrix.Identity;
				InputData[i] = null;
				MaxSpeed[i] = 0;
				MaxAcceleration[i] = 0;
				Models[i] = null;
				StandardEffectParams[i] = null;
				ModelSpaceBVs[i] = new OBB(new Vector3(0, 0, 0),
				                   new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
				                   new Vector3(1, 1, 1));
			}
			PlayerCubeState = new PlayerCubeState();
			Gravity = new Vector3(0.0f, -20f, 0.0f);

			SpawnPoint = new Vector3(0, 25, 0);
			EventBuffer = new EventBuffer();
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
			Transforms[entityToDestroy.Id] = Matrix.Identity;

			Models[entityToDestroy.Id] = null;
			StandardEffectParams[entityToDestroy.Id] = null;

			ModelSpaceBVs[entityToDestroy.Id] = new OBB(new Vector3(0, 0, 0),
			                                    new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
			                                    new Vector3(1, 1, 1));
		}

	}
}
