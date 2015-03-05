using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Levels.Assets
{
	class Player : Asset
	{
		StandardEffectParams effect;

		public void Add(Vector3 pos)
		{
			world.SpawnPoint = pos;
			world.CameraPosition = pos + new Vector3(0, 2, -1);

			Entity player = new EntityBuilder().WithModel(model)
					.WithPosition(world.SpawnPoint)
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(effect)
					.WithBoundingVolume(obb)
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);
			world.Player = player;
		}
		protected override void loadAssets()
		{
			model = game.Content.Load<Model>("Models/Cube/cube_clean");
			obb = new OBB(new Vector3(0, .5f, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(1, 1, 1));
			effect = new StandardEffectParams
			{
				DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};
		}

		public Player(World world, Game game) :
			base(world, game)
		{
			loadAssets();
		}
	}
}
