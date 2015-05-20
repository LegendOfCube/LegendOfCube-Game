using System;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework.Media;

namespace LegendOfCube.Engine
{
	class AudioSystem
	{
		private PlayerCubeState oldPlayerCubeState;

		private float pitch;
		private readonly ContentCollection cc;

		public AudioSystem(ContentCollection contentCollection)
		{
			oldPlayerCubeState = new PlayerCubeState();
			pitch = 0;
			cc = contentCollection;
		}

		public void Update(World world)
		{
			foreach (var collisionEvent in world.EventBuffer.CollisionEvents)
			{
				var collider = collisionEvent.Collider.Id;
				var collidedWith = collisionEvent.CollidedWith.Id;

				if (world.PlayerRespawAudioCue)
				{
					cc.respawn.Play(1, 0f, 0f);
					world.PlayerRespawAudioCue = false;
				}
				if (collider == world.Player.Id || collidedWith == world.Player.Id)
				{
					//Player specific events
					if (world.EntityProperties[collidedWith].Satisfies(Properties.BOUNCE_FLAG))
					{
						cc.bounce.Play(0.05f, 0f, 0f);
					}
					if (world.PlayerCubeState.OnGround && !oldPlayerCubeState.OnGround
						|| world.PlayerCubeState.OnWall && !oldPlayerCubeState.OnWall)
					{
						cc.hit.Play(MathUtils.MapRangeToRange(Math.Abs(collisionEvent.ColliderVelocity.Y), 0, 50, 0, 1), 0f, 0f);
					}
				}
			}
			if (!world.PlayerCubeState.OnGround && oldPlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping() && world.InputData[world.Player.Id].NewJump())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				cc.oldJump.Play(0.7f, 0, 0);
			}
			else if (!world.PlayerCubeState.OnWall && oldPlayerCubeState.OnWall && !world.PlayerCubeState.OnGround && world.InputData[world.Player.Id].IsJumping() && world.InputData[world.Player.Id].NewJump())
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				cc.wallJump.Play(0.6f, 0, 0);
			}

			oldPlayerCubeState = world.PlayerCubeState;
		}

		public void OnStart(World world)
		{
			if (world.Ambience != null)
			{
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(world.Ambience);
				MediaPlayer.Volume = 0.2f;
			}
		}

		public void OnResume(World world)
		{
			if (world.Ambience != null)
			{
				MediaPlayer.Resume();
			}
		}

		public void OnPause(World world)
		{
			if (world.Ambience != null)
			{
				MediaPlayer.Pause();
			}
		}
	}
}
