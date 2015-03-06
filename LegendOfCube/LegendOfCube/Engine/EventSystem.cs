using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class EventSystem
	{
		private const float ON_GROUND_LIMIT = 0.70f;
		private const float ON_WALL_LIMIT = 0.90f;

		public static void CalculateCubeState(World world)
		{
			// Defaults to not being on the ground or on the wall
			world.PlayerCubeState.OnGround = false;
			world.PlayerCubeState.OnWall = false;
			world.PlayerCubeState.GroundAxis = Vector3.Zero;
			world.PlayerCubeState.WallAxis = Vector3.Zero;

			// Check if player cube collided with any walls or grounds
			foreach (var e in world.EventBuffer.CollisionEvents)
			{
				if (e.Collider.Id == world.Player.Id)
				{
					if (Math.Abs(e.Axis.Y) > ON_GROUND_LIMIT)
					{
						world.PlayerCubeState.OnGround = true;
						world.PlayerCubeState.GroundAxis = e.Axis;
					}
					else if ((Math.Abs(e.Axis.X) + Math.Abs(e.Axis.Z)) > ON_WALL_LIMIT)
					{
						world.PlayerCubeState.OnWall = true;
						world.PlayerCubeState.WallAxis = e.Axis;
					}
				}
				else if (e.CollidedWith.Id == world.Player.Id)
				{
					if (Math.Abs(e.Axis.Y) > ON_GROUND_LIMIT)
					{
						world.PlayerCubeState.OnGround = true;
						world.PlayerCubeState.GroundAxis = -e.Axis;
					}
					else if ((Math.Abs(e.Axis.X) + Math.Abs(e.Axis.Z)) > ON_WALL_LIMIT)
					{
						world.PlayerCubeState.OnWall = true;
						world.PlayerCubeState.WallAxis = -e.Axis;
					}
				}
			}

			// If Cube is on the ground it can't be on a wall.
			if (world.PlayerCubeState.OnGround)
			{
				world.PlayerCubeState.OnWall = false;
				world.PlayerCubeState.WallAxis = Vector3.Zero;
			}
		}

		public static void HandleEvents(World world)
		{
			EventBuffer eventBuffer = world.EventBuffer;
			foreach (var collisionEvent in eventBuffer.CollisionEvents)
			{
				var collidedWith = collisionEvent.CollidedWith.Id;
				var collider = collisionEvent.Collider.Id;
				if (world.EntityProperties[collidedWith].Satisfies((Properties.DEATH_ZONE_FLAG)))
				{
					if (collider == world.Player.Id)
					{
						world.Transforms[collider].Translation = world.SpawnPoint;
						world.Velocities[collider] = Vector3.Zero;
					}
				}
				else if (world.EntityProperties[collider].Satisfies(((Properties.DEATH_ZONE_FLAG))))
				{
					if (collidedWith == world.Player.Id)
					{
						world.Transforms[collidedWith].Translation = world.SpawnPoint;
						world.Velocities[collidedWith] = Vector3.Zero;
					}
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.TELEPORT_FLAG))
				{
					while (true)
					{
						Random rnd = new Random();
						int temp = rnd.Next((int)world.HighestOccupiedId);
						if (world.EntityProperties[temp].Satisfies(Properties.TELEPORT_FLAG))
						{
							Vector3 temp2 = world.Transforms[temp].Translation;
							temp2.Y += 25;
							world.Transforms[collider].Translation = temp2;
							break;
						}
					}
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.BOUNCE_FLAG))
				{
					world.Velocities[collider] = Vector3.Reflect(collisionEvent.ColliderVelocity, collisionEvent.Axis);
					world.PlayerCubeState.OnGround = false;
					world.PlayerCubeState.OnWall = false;
				}
			}

			//Magic LINQ from resharper
			//var playerAffected = eventBuffer.CollisionEvents.Any(collisionEvent => collisionEvent.Collider.Id == world.Player.Id);

			//if (!playerAffected)
			//{
			//	world.PlayerCubeState.InAir = true;
			//
		}
	}
}
