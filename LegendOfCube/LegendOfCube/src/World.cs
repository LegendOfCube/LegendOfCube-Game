using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube
{
	/** Class responsible for keeping track of all entities. */
	public class World
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly ComponentMask NO_COMPONENTS = new ComponentMask(ComponentMask.NO_COMPONENTS);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		public readonly UInt32 MAX_NUM_ENTITIES;
		public UInt32 NumEntities;
		public ComponentMask[] ComponentMasks;
		public Vector3[] Positions;
		public Vector3[] Velocities;
		public Vector3[] Accelerations;
		public Model[] Models;
		public Matrix[] Transforms;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
		
		public World(UInt32 maxNumEntities)
		{
			MAX_NUM_ENTITIES = maxNumEntities;
			NumEntities = 0;
			ComponentMasks = new ComponentMask[MAX_NUM_ENTITIES];
			for (UInt32 i = 0; i < MAX_NUM_ENTITIES; i++) {
				ComponentMasks[i] = NO_COMPONENTS;
			}

			// Components
			Positions = new Vector3[MAX_NUM_ENTITIES];
			Velocities = new Vector3[MAX_NUM_ENTITIES];
			Accelerations = new Vector3[MAX_NUM_ENTITIES];
			Models = new Model[MAX_NUM_ENTITIES];
			Transforms = new Matrix[MAX_NUM_ENTITIES];
			for (UInt32 i = 0; i < MAX_NUM_ENTITIES; i++) {
				Positions[i] = new Vector3(0, 0, 0);
				Velocities[i] = new Vector3(0, 0, 0);
				Accelerations[i] = new Vector3(0, 0, 0);
				Models[i] = null;
				Transforms[i] = Matrix.Identity;
			}
		}

		// Public Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public bool canCreateMoreEntities()
		{
			return NumEntities < MAX_NUM_ENTITIES;
		}

		public Entity createEntity(ComponentMask wantedComponents)
		{
			if (wantedComponents == NO_COMPONENTS) {
				throw new ArgumentException("Entity must contain at least one component.");
			}
			if (!canCreateMoreEntities()) {
				throw new InvalidOperationException("Can't create more entites.");
			}
			UInt32 entity;
			for (entity = 0; entity < MAX_NUM_ENTITIES; entity++) {
				if (ComponentMasks[entity] == NO_COMPONENTS) {
					break;
				}
			}
			if (entity >= MAX_NUM_ENTITIES) {
				throw new InvalidOperationException("Something went terribly wrong.");
			}
			ComponentMasks[entity] = wantedComponents;
			NumEntities++;
			return new Entity(entity);
		}

		public void destroyEntity(Entity entityToDestroy)
		{
			if (entityToDestroy.ID >= MAX_NUM_ENTITIES) {
				throw new ArgumentException("Entity to be destroyed doesn't exist.");
			}
			ComponentMasks[entityToDestroy.ID] = NO_COMPONENTS;
			NumEntities--;

			// Clean-up components
			Positions[entityToDestroy.ID] = new Vector3(0, 0, 0);
			Velocities[entityToDestroy.ID] = new Vector3(0, 0, 0);
			Accelerations[entityToDestroy.ID] = new Vector3(0, 0, 0);
			Models[entityToDestroy.ID] = null;
			Transforms[entityToDestroy.ID] = Matrix.Identity;
		}
	}
}
