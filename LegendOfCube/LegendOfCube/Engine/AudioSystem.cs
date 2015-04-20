using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LegendOfCube.Engine
{
	class AudioSystem
	{
		private PlayerCubeState oldPlayerCubeState;

		private float pitch;
		private int oldSelection;
		private readonly ContentCollection cc;

		public AudioSystem(ContentCollection contentCollection)
		{
			oldPlayerCubeState = new PlayerCubeState();
			pitch = 0;
			oldSelection = 0;
			cc = contentCollection;
		}

		public void Update(World world)
		{
			foreach (var collisionEvent in world.EventBuffer.CollisionEvents)
			{
				var collider = collisionEvent.Collider.Id;
				var collidedWith = collisionEvent.CollidedWith.Id;

				if (world.PlayerHasRespawned)
				{
					cc.respawn.Play(0.15f, 0f, 0f);
					world.PlayerHasRespawned = false;
				}
				if (collider == world.Player.Id || collidedWith == world.Player.Id)
				{
					//Player specific events
					if (world.EntityProperties[collidedWith].Satisfies(Properties.BOUNCE_FLAG))
					{
						cc.bounce.Play(0.15f, 0f, 0f);
					}
					if (world.PlayerCubeState.OnGround && !oldPlayerCubeState.OnGround
						|| world.PlayerCubeState.OnWall && !oldPlayerCubeState.OnWall)
					{
						cc.hit.Play();
					}
				}
			}
			if (!world.PlayerCubeState.OnGround && oldPlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				cc.jump.Play(1, pitch, 0);
			}
			else if (!world.PlayerCubeState.OnWall && oldPlayerCubeState.OnWall && !world.PlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				cc.wallJump.Play(1, pitch, 0);
			}

			oldPlayerCubeState = world.PlayerCubeState;
		}
	}
}
