using System;
using System.Collections;
using System.Collections.Generic;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Events;
using LegendOfCube.Engine.Graphics;
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
		public UInt32 NumEntities;
		public UInt32 HighestOccupiedId;

		// Describe what components an entity has
		public readonly Properties[] EntityProperties;
		public readonly Matrix[] Transforms; // TRANSFORM
		public readonly Vector3[] Velocities; // VELOCITY
		public readonly float[] MaxSpeed; // VELOCITY
		public readonly Vector3[] Accelerations; // ACCELERATION
		public readonly InputData[] InputData; // INPUT
		public readonly OBB[] ModelSpaceBVs; // MODEL_SPACE_BV
		public readonly AIComponent[] AIComponents; // AI_FLAG
		public readonly Model[] Models; // MODEL
		public readonly StandardEffectParams[] StandardEffectParams; // STANDARD_EFFECT

		// Player state
		public PlayerCubeState PlayerCubeState;

		//GameOver state
		public bool WinState;
		public float TimeSinceGameOver;

		//Game statistics
		public GameStats GameStats;

		// Shortcut to player entity which there will be one instance of
		public Entity Player;

		// World variables

		public Vector3 SpawnPoint;
		public readonly EventBuffer EventBuffer;

		public Vector3 Gravity;
		public readonly float AirMovement;
		public readonly float StopTime;
		public readonly float BaseJump;

		public Camera Camera;

		public Vector3 LightDirection;
		public float AmbientIntensity;

		public bool PointLight0Enabled;
		public PointLight PointLight0;

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
			ModelSpaceBVs = new OBB[MaxNumEntities];
			AIComponents = new AIComponent[MaxNumEntities];

			Models = new Model[MaxNumEntities];
			StandardEffectParams = new StandardEffectParams[MaxNumEntities];
			for (UInt32 i = 0; i < MaxNumEntities; i++) {
				Velocities[i] = Vector3.Zero;
				MaxSpeed[i] = 0;
				Accelerations[i] = Vector3.Zero;
				Transforms[i] = Matrix.Identity;
				InputData[i] = null;
				Models[i] = null;
				StandardEffectParams[i] = null;
				ModelSpaceBVs[i] = new OBB(Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One);
			}
			
			PlayerCubeState = new PlayerCubeState();
			PlayerCubeState.OnWall = false;
			PlayerCubeState.OnGround = false;
			PlayerCubeState.WallAxis = Vector3.Zero;
			PlayerCubeState.GroundAxis = Vector3.Zero;

			WinState = false;
			TimeSinceGameOver = 0;

			GameStats = new GameStats();

			Gravity = new Vector3(0.0f, -20f, 0.0f);
			AirMovement = 0.4f;
			StopTime = 0.05f;
			BaseJump = 12f;
			SpawnPoint = new Vector3(0, 25, 0);

			LightDirection = new Vector3(0, -1, 0);
			AmbientIntensity = 0.5f;
			PointLight0Enabled = false;

			Camera = Camera.DEFAULT_CAMERA;

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
			Velocities[entityToDestroy.Id] = Vector3.Zero;
			MaxSpeed[entityToDestroy.Id] = 0;
			Accelerations[entityToDestroy.Id] = Vector3.Zero;
			Transforms[entityToDestroy.Id] = Matrix.Identity;
			InputData[entityToDestroy.Id] = null;
			Models[entityToDestroy.Id] = null;
			StandardEffectParams[entityToDestroy.Id] = null;
			ModelSpaceBVs[entityToDestroy.Id] = new OBB(Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One);
		}
	}
}
