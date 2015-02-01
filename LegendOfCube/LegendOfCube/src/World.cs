using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube
{
    /** Specifies a combination of Components. */
    public struct ComponentMask
    {
        // Mask Constants
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        // Notes: 'const' implies 'static' in C#.
        // '1 << x' means that the bit at the x:th position is set.

        public const UInt64 NO_COMPONENTS = 0;
        public const UInt64 POSITION = 1 << 0;
        public const UInt64 VELOCITY = 1 << 1;
        public const UInt64 ACCELERATION = 1 << 2;

        // Members
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        public readonly UInt64 Mask;

        // Constructors
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        public ComponentMask(UInt64 mask)
        {
            Mask = mask;
        }

        // Public Methods
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        /** Checks whether this ComponentMask contains all the Components in the param Mask. */
        public bool satisfies(ComponentMask requirements)
        {
            return ((this.Mask & requirements.Mask) == requirements.Mask);
        }

        // Operators
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        public override bool Equals(object obj)
        {
            if (!(obj is ComponentMask)) return false; 
            return this == ((ComponentMask)obj);
        }

        public static bool operator ==(ComponentMask lhs, ComponentMask rhs)
        {
            return lhs.Mask == rhs.Mask;
        }

        public static bool operator !=(ComponentMask lhs, ComponentMask rhs)
        {
            return lhs.Mask != rhs.Mask;
        }
    }

    /** Type used to specify an entity in the World. */
    public struct Entity
    {
        public readonly UInt32 ID;
        public Entity(UInt32 id)
        {
            ID = id;
        }
    }

    /** Class responsible for keeping track of all entities. */
    public class World
    {
        // Constants
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        private static readonly ComponentMask NO_COMPONENTS = new ComponentMask(ComponentMask.NO_COMPONENTS);

        // Members
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        public readonly UInt32 MAX_NUM_ENTITIES;
        public UInt32 numEntities;
        public ComponentMask[] componentMasks;
        public Vector3[] positions;
        public Vector3[] velocities;
        public Vector3[] accelerations;

        // Constructors
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        
        public World(UInt32 maxNumEntities)
        {
            MAX_NUM_ENTITIES = maxNumEntities;
            numEntities = 0;
            componentMasks = new ComponentMask[MAX_NUM_ENTITIES];

            // Components
            positions = new Vector3[MAX_NUM_ENTITIES];
            velocities = new Vector3[MAX_NUM_ENTITIES];
            accelerations = new Vector3[MAX_NUM_ENTITIES];
            for (UInt32 i = 0; i < MAX_NUM_ENTITIES; i++) {
                positions[i] = new Vector3(0, 0, 0);
                velocities[i] = new Vector3(0, 0, 0);
                accelerations[i] = new Vector3(0, 0, 0);
            }
        }

        // Public Methods
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        public bool canCreateMoreEntities()
        {
            return numEntities < MAX_NUM_ENTITIES;
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
                if (componentMasks[entity] == NO_COMPONENTS) {
                    break;
                }
            }
            if (entity >= MAX_NUM_ENTITIES) {
                throw new InvalidOperationException("Something went terribly wrong.");
            }
            numEntities++;
            return new Entity(entity);
        }

        public void destroyEntity(Entity entityToDestroy)
        {
            if (entityToDestroy.ID >= MAX_NUM_ENTITIES) {
                throw new ArgumentException("Entity to be destroyed doesn't exist.");
            }
            componentMasks[entityToDestroy.ID] = NO_COMPONENTS;
            numEntities--;

            // Clean-up components
            positions[entityToDestroy.ID] = new Vector3(0, 0, 0);
            velocities[entityToDestroy.ID] = new Vector3(0, 0, 0);
            accelerations[entityToDestroy.ID] = new Vector3(0, 0, 0);
        }
    }
}
