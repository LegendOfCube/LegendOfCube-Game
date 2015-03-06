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
			world.EventBuffer.Flush();
		}
	}
}
