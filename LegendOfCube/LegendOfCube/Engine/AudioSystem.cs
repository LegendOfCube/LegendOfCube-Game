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
		public AudioSystem(ContentManager cm)
		{
			respawn = cm.Load<SoundEffect>("SoundEffects/bwiip");
		}

		public void Update(World world)
		{
			foreach (var collisionEvent in world.EventBuffer.CollisionEvents)
			{
				if (collisionEvent.Collider.Id == world.Player.Id || collisionEvent.CollidedWith.Id == world.Player.Id)
				{
					if (world.PlayerHasRespawned)
					{
						respawn.Play(0.5f ,0.0f, 0.0f);
						world.PlayerHasRespawned = false;
					} 
				}
			}
		}
	}
}
