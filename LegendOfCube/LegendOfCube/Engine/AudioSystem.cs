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

		private readonly SoundEffect respawn;
		private readonly SoundEffect oldJump;
		private readonly SoundEffect jump;
		private readonly SoundEffect wallJump;
		private readonly SoundEffect bounce;
		private readonly SoundEffect hit;

		private PlayerCubeState oldPlayerCubeState;

		private float pitch;

		public AudioSystem(ContentManager cm)
		{
			respawn = cm.Load<SoundEffect>("SoundEffects/bwiip");
			oldJump = cm.Load<SoundEffect>("SoundEffects/waom");
			wallJump = cm.Load<SoundEffect>("SoundEffects/waom2");
			jump = cm.Load<SoundEffect>("SoundEffects/waom3");
			bounce = cm.Load<SoundEffect>("SoundEffects/boing");
			hit = cm.Load<SoundEffect>("SoundEffects/hit");

			oldPlayerCubeState = new PlayerCubeState();

			pitch = 0;
		}

		public void Update(World world)
		{
			foreach (var collisionEvent in world.EventBuffer.CollisionEvents)
			{
				var collider = collisionEvent.Collider.Id;
				var collidedWith = collisionEvent.CollidedWith.Id;
				if (collider == world.Player.Id || collidedWith == world.Player.Id)
				{
					//Player specific events
					if (world.EntityProperties[collidedWith].Satisfies(Properties.BOUNCE_FLAG))
					{
						bounce.Play(0.15f, 0f, 0f);
					}
					if (world.PlayerCubeState.OnGround && !oldPlayerCubeState.OnGround
						|| world.PlayerCubeState.OnWall && !oldPlayerCubeState.OnWall)
					{
						hit.Play();
					}
				}
			}
			if (world.PlayerHasRespawned)
			{
				respawn.Play(0.15f, 0f, 0f);
				world.PlayerHasRespawned = false;
			}
			if (!world.PlayerCubeState.OnGround && oldPlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				jump.Play(1, pitch, 0);
			}
			else if (!world.PlayerCubeState.OnWall && oldPlayerCubeState.OnWall && !world.PlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				wallJump.Play(1, pitch, 0);
			}

			oldPlayerCubeState = world.PlayerCubeState;
		}
	}
}
