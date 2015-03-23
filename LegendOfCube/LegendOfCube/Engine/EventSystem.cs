using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LegendOfCube.Engine
{
	class EventSystem
	{
		private static readonly float GROUND_WALL_ANGLE = MathHelper.ToRadians(75.0f); // 0 < angle < 90
		private static readonly float ON_WALL_LIMIT = (float)Math.Sin(GROUND_WALL_ANGLE);
		private static readonly float ON_GROUND_LIMIT = (float)Math.Cos(GROUND_WALL_ANGLE);

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
				float xDot = Math.Abs(e.Axis.X);
				float zDot = Math.Abs(e.Axis.Z);
				float wallDot = (float)Math.Sqrt(xDot * xDot + zDot * zDot);
				if (e.Collider.Id == world.Player.Id || e.CollidedWith.Id == world.Player.Id)
				{
					if (e.Axis.Y > ON_GROUND_LIMIT)
					{
						world.PlayerCubeState.OnGround = true;
						world.PlayerCubeState.GroundAxis = (e.Collider.Id == world.Player.Id ? 1.0f : -1.0f) * e.Axis;
					}
					else if (wallDot > ON_WALL_LIMIT)
					{
						world.PlayerCubeState.OnWall = true;
						world.PlayerCubeState.WallAxis = (e.Collider.Id == world.Player.Id ? 1.0f : -1.0f) * e.Axis;
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
						RespawnPlayer(world);
					}
				}
				else if (world.EntityProperties[collider].Satisfies(((Properties.DEATH_ZONE_FLAG))))
				{
					if (collidedWith == world.Player.Id)
					{
						RespawnPlayer(world);
					}
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.CHECKPOINT_FLAG))
				{
					if (collider == world.Player.Id)
					{
						world.SpawnPoint = world.Transforms[collidedWith].Translation;
					}
				}
				else if (world.EntityProperties[collider].Satisfies(Properties.CHECKPOINT_FLAG))
				{
					if (collidedWith == world.Player.Id)
					{
						world.SpawnPoint = world.Transforms[collider].Translation;
					}
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.TELEPORT_FLAG))
				{
					Random rnd = new Random();
					var dest = world.EnumerateEntities(new Properties(Properties.TELEPORT_FLAG)).Where(entity => entity.Id != collidedWith).ToList();
					int teleportTo = (int) dest[rnd.Next(dest.Count)].Id;
					world.Transforms[collider].Translation = world.Transforms[teleportTo].Translation - 5*collisionEvent.Axis;
					world.Velocities[collider] = collisionEvent.ColliderVelocity;
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.BOUNCE_FLAG))
				{
					world.Velocities[collider] = Vector3.Reflect(collisionEvent.ColliderVelocity, collisionEvent.Axis);
					world.PlayerCubeState.OnGround = false;
					world.PlayerCubeState.OnWall = false;
				}
				if (world.EntityProperties[collidedWith].Satisfies(Properties.WIN_ZONE_FLAG))
				{
					//world.EntityProperties[collider].Subtact(Properties.GRAVITY_FLAG | Properties.ACCELERATION | Properties.DYNAMIC_VELOCITY_FLAG | Properties.VELOCITY);
					world.WinState = true;
				}
			}
		}

		public static void RespawnPlayer(World world)
		{
			// Look toward where you died
			// TODO: Refine this, view direction per spawn point?
			world.CameraPosition = world.SpawnPoint - 2.0f * Vector3.Normalize(world.Transforms[world.Player.Id].Translation - world.SpawnPoint);
			world.CameraPosition.Y = world.SpawnPoint.Y + 2.0f;

			world.Transforms[world.Player.Id].Translation = world.SpawnPoint;
			world.Velocities[world.Player.Id] = Vector3.Zero;
			//world.EntityProperties[world.Player.Id].Add(Properties.GRAVITY_FLAG | Properties.ACCELERATION | Properties.DYNAMIC_VELOCITY_FLAG | Properties.VELOCITY);
			world.WinState = false;
		}
	}
}
