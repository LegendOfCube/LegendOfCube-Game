﻿using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class ContentCollection
	{
		public ModelData PlayerCube { get; private set; }
		public ModelData PlayerCubePlain { get; private set; }
		public ModelData RustPlatform { get; private set; }
		public ModelData BrickWall { get; private set; }
		public ModelData PlainCube { get; private set; }
		public ModelData DropSign { get; private set; }
		public ModelData CatwalkStart { get; private set; }
		public ModelData CatwalkMiddle { get; private set; }
		public ModelData CatwalkEnd { get; private set; }
		public ModelData Door { get; private set; }
		public ModelData ExitSign { get; private set; }
		public ModelData Pillar { get; private set; }

		public Model CubeModel { get; private set; }
		public Model PlainCubeModel { get; private set; }
		public Model PlatformModel { get; private set; }
		public Model BrickWallModel { get; private set; }
		public Model DropSignModel { get; private set; }
		public Model CatwalkStartModel { get; private set; }
		public Model CatwalkMiddleModel { get; private set; }
		public Model CatwalkEndModel { get; private set; }
		public Model DoorModel { get; private set; }
		public Model ExitSignModel { get; private set; }
		public Model PillarModel { get; private set; }
		public Model DeathDuctModel { get; private set; }
		public Model DeathDuctFanModel { get; private set; }
		public Model Cube10Model { get; private set; }
		public Model MovingPartsSignModel { get; private set; }
		public Model HangingPlatformModel { get; private set; }


		public void LoadContent(ContentManager cm)
		{
			CubeModel = cm.Load<Model>("Models/Cube/cube_clean");
			PlatformModel = cm.Load<Model>("Models/Platform/platform");
			BrickWallModel = cm.Load<Model>("Models/Brick_Wall/brick_wall");
			DropSignModel = cm.Load<Model>("Models/Sign_Drop/danger_drop");
			CatwalkStartModel = cm.Load<Model>("Models/Catwalk/catwalk_start_fix_2");
			CatwalkMiddleModel = cm.Load<Model>("Models/Catwalk/catwalk_middle_fix_2");
			CatwalkEndModel = cm.Load<Model>("Models/Catwalk/catwalk_end_fix_2");
			DoorModel = cm.Load<Model>("Models/Door/door");
			ExitSignModel = cm.Load<Model>("Models/Sign_Exit/exit_sign");
			PillarModel = cm.Load<Model>("Models/Platform/pillar");
			DeathDuctModel = cm.Load<Model>("Models/Duct/deathcube");
			DeathDuctFanModel = cm.Load<Model>("Models/Duct/deathcube_fan");
			Cube10Model = cm.Load<Model>("Models/Duct/cube10");
			MovingPartsSignModel = cm.Load<Model>("Models/Sign_Moving/moving_parts");
			HangingPlatformModel = cm.Load<Model>("Models/Hanging_Platform/hanging_platform");

			PlayerCube = new ModelData
			{
				Model = CubeModel,
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 0.5f, 0.0f), 1, 1, 1),
				EffectParams = new StandardEffectParams
				{
					DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
					EmissiveTexture = cm.Load<Texture>("Models/Cube/cube_emissive"),
					SpecularColor = Color.Gray.ToVector4(),
					EmissiveColor = Color.White.ToVector4()
				}
			};

			PlayerCubePlain = new ModelData
			{
				Model = PlainCubeModel,
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 0.5f, 0.0f), 1, 1, 1),
				EffectParams = new StandardEffectParams
				{
					DiffuseColor = new Vector4(new Vector3(0.45f), 1.0f),
					SpecularTexture = cm.Load<Texture>("Models/cube/cube_specular"),
					EmissiveTexture = cm.Load<Texture>("Models/cube/cube_emissive_plain"),
					NormalTexture = cm.Load<Texture>("Models/cube/cube_normal"),
					SpecularColor = Color.White.ToVector4(),
					EmissiveColor = Color.White.ToVector4()
				}
			};

			PlainCube = new ModelData
			{
				Model = PlainCubeModel,
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 0.5f, 0.0f), 1, 1, 1)
			};

			RustPlatform = new ModelData
			{
				Model = PlatformModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Platform/rusted metal-d"),
					NormalTexture = cm.Load<Texture>("Models/Platform/rust_normal_sharp"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -0.25f, 0.0f), 10.0f, 0.5f, 10.0f)
			};

			BrickWall = new ModelData
			{
				Model = BrickWallModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Brick_Wall/brick_d"),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
					SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 1.25f, 0.0f), 0.5f, 2.5f, 5.0f)
			};

			DropSign = new ModelData
			{
				Model = DropSignModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Sign_Drop/danger_drop_d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			CatwalkStart = new ModelData
			{
				Model = CatwalkStartModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Catwalk/catwalk-d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			CatwalkMiddle = new ModelData
			{
				Model = CatwalkMiddleModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Catwalk/catwalk-d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};
			CatwalkEnd = new ModelData
			{
				Model = CatwalkEndModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Catwalk/catwalk-d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			Door = new ModelData
			{
				Model = DoorModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Door/door_d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			ExitSign = new ModelData
			{
				Model = ExitSignModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Sign_Exit/exit_d_e"),
					EmissiveTexture = cm.Load<Texture>("Models/Sign_Exit/exit_d_e"),
					SpecularColor = Color.Gray.ToVector4(),
					EmissiveColor = Color.White.ToVector4()
				}
			};

			Pillar = new ModelData
			{
				Model = PillarModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Platform/metal_rust_tex_01"),
					NormalTexture = cm.Load<Texture>("Models/Platform/pipe_normal"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};




		}
	}
}
