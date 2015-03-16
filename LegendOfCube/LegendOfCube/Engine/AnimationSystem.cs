using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.CubeMath;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	class AnimationSystem
	{
		public void OnUpdate(World world, float delta)
		{
			SetCubeColor(world, world.Player);
		}

		// Modify cube emissiveness based on state
		private void SetCubeColor(World world, Entity e)
		{
			// Color cube sides if on wall
			var playerEffect = world.StandardEffectParams[e.Id];
			var newEffect = playerEffect.ShallowCopy();

			var cubeState = world.PlayerCubeState;

			Color newColor;
			if (cubeState.OnWall) newColor = new Color(255, 108, 0);
			else if (cubeState.OnGround) newColor = new Color(0, 247, 255);
			else newColor = new Color(255, 246, 0);

			float speed = world.Velocities[e.Id].Length();
			float brightness = MathUtils.ClampLerp(speed, 0.6f, 1.0f, 0.0f, world.MaxSpeed[e.Id]);

			newEffect.EmissiveColor = (newColor * brightness).ToVector4();

			world.StandardEffectParams[e.Id] = newEffect;
		}
	}
}
