using LegendOfCube.Engine.BoundingVolumes;
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
		public ModelData BrickWallArrowsHorizontal { get; private set; }
		public ModelData BrickWallArrowsVertical { get; private set; }
		public ModelData BrickWallWindow { get; private set; }
		public ModelData PlainCube { get; private set; }
		public ModelData DropSign { get; private set; }
		public ModelData CatwalkStart { get; private set; }
		public ModelData CatwalkMiddle { get; private set; }
		public ModelData CatwalkEnd { get; private set; }
		public ModelData Door { get; private set; }
		public ModelData ExitSign { get; private set; }
		public ModelData Pillar { get; private set; }
		public ModelData DeathDuct { get; private set; }
		public ModelData Duct { get; private set; }
		public ModelData Fan { get; private set; }
		public ModelData MovingPartsSign { get; private set; }
		public ModelData HangingPlatform { get; private set; }
		public ModelData Manhole { get; private set; }
		public ModelData Truss { get; private set; }
		public ModelData GroundConcrete { get; private set; }
		public ModelData GroundAsphalt { get; private set; }
		public ModelData GroundWood { get; private set; }
		public ModelData GroundStone { get; private set; }
		public ModelData SignArrowUp { get; private set; }
		public ModelData SignTrampoline { get; private set; }
		public ModelData WindowBars { get; private set; }
		public ModelData Fence { get; private set; }
		public ModelData Barbs { get; private set; }

		// Placeholders
		public ModelData placeholderWall { get; private set; }


		public Model CubeModel { get; private set; }
		public Model PlainCubeModel { get; private set; }
		public Model PlatformModel { get; private set; }
		public Model BrickWallModel { get; private set; }
		public Model BrickWallWindowModel { get; private set; }
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
		public Model ManholeModel { get; private set; }
		public Model TrussModel { get; private set; }
		public Model Ground50x50 { get; private set; }
		public Model Ground100x50 { get; private set; }
		public Model SignModel { get; private set; }
		public Model WindowBarsModel { get; private set; }
		public Model FenceModel { get; private set; }
		public Model BarbsModel { get; private set; }


		public void LoadContent(ContentManager cm)
		{
			CubeModel = cm.Load<Model>("Models/Cube/cube_clean");
			PlainCubeModel = cm.Load<Model>("Models/cube/cube_plain");
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
			ManholeModel = cm.Load<Model>("Models/Manhole/manhole");
			TrussModel = cm.Load<Model>("Models/Roof_Beam/roof_beam");
			SignModel = cm.Load<Model>("Models/Signs/sign");
			Ground50x50 = cm.Load<Model>("Models/Ground/ground_50x50");
			BrickWallWindowModel = cm.Load<Model>("Models/Brick_Wall/brick_wall_window_no_bars");
			WindowBarsModel = cm.Load<Model>("Models/Brick_Wall/window_bars");
			FenceModel = cm.Load<Model>("Models/Fence/fence");
			BarbsModel = cm.Load<Model>("Models/Fence/barbs_no_rotation");

			placeholderWall = new ModelData
			{
				Model = Ground50x50,
				//Obb = OBB.CreateAxisAligned(Vector3.Zero, 100, 50, 1),
				EffectParams = new StandardEffectParams
				{
					DiffuseColor = Color.DarkGray.ToVector4(),
				}
			};

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
					NormalTexture = cm.Load<Texture>("Models/Platform/platform-normal"),
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

			BrickWallArrowsHorizontal = new ModelData
			{
				Model = BrickWallModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Brick_Wall/brick_arrows_h_d"),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
					SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 1.25f, 0.0f), 0.5f, 2.5f, 5.0f)
			};

			BrickWallArrowsVertical = new ModelData
			{
				Model = BrickWallModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Brick_Wall/brick_arrows_v_d"),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
					SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 1.25f, 0.0f), 0.5f, 2.5f, 5.0f)
			};

			BrickWallWindow = new ModelData
			{
				Model = BrickWallWindowModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Brick_Wall/brick_d"),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
					SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 1.25f, 0.0f), 0.5f, 2.5f, 5.0f)
			};

			WindowBars = new ModelData
			{
				Model = WindowBarsModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Catwalk/catwalk-d")
				}
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
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, -39, 0), 4.5f, 77, 4.5f)
			};

			DeathDuct = new ModelData
			{
				Model = DeathDuctModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Duct/duct_d"),
					NormalTexture = cm.Load<Texture>("Models/Duct/duct_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(Vector3.Zero, 10, 10, 10)
			};
			Duct = new ModelData
			{
				Model = Cube10Model,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Duct/duct_d"),
					NormalTexture = cm.Load<Texture>("Models/Duct/duct_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 5, 0), 10, 10, 10)
			};
			Fan = new ModelData
			{
				Model = DeathDuctFanModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Duct/duct_d"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 5, -5), 7, 7, 0.5f)
			};

			MovingPartsSign = new ModelData
			{
				Model = MovingPartsSignModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Sign_Moving/moving_parts_d"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			HangingPlatform = new ModelData
			{
				Model = HangingPlatformModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Hanging_Platform/metal_plate-diffuse"),
					NormalTexture = cm.Load<Texture>("Models/Hanging_Platform/metal_plate-normal"),
					SpecularTexture = cm.Load<Texture>("Models/Hanging_Platform/metal_plate-spec"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -0.25f, 0.0f), 10.0f, 0.5f, 10.0f)
			};

			Manhole = new ModelData
			{
				Model = ManholeModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Manhole/rusted-manhole-d"),
					NormalTexture = cm.Load<Texture>("Models/Manhole/rusted-manhole-n"),
					SpecularColor = Color.Gray.ToVector4()
				}
			};

			Truss = new ModelData
			{
				Model = TrussModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseColor = Color.DarkRed.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Manhole/rusted-manhole-n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 1.4f, 0), 0.5f, 2.8f, 50)
			};

			GroundConcrete = new ModelData
			{
				Model = Ground50x50,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Ground/concrete_d"),
					NormalTexture = cm.Load<Texture>("Models/Ground/concrete_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -1f, 0.0f), 50, 2, 50)
			};
			GroundAsphalt = new ModelData
			{
				Model = Ground50x50,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Ground/asphalt_d"),
					NormalTexture = cm.Load<Texture>("Models/Ground/asphalt_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -1f, 0.0f), 50, 2, 50)
			};
			GroundWood = new ModelData
			{
				Model = Ground50x50,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Ground/planks_d"),
					NormalTexture = cm.Load<Texture>("Models/Ground/planks_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -1f, 0.0f), 50, 2, 50)
			};
			GroundStone = new ModelData
			{
				Model = Ground50x50,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Ground/stone_d"),
					NormalTexture = cm.Load<Texture>("Models/Ground/stone_n"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -1f, 0.0f), 50, 2, 50)
			};

			SignArrowUp = new ModelData
			{
				Model = SignModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Signs/arrowup"),
					SpecularColor = Color.Gray.ToVector4()
				},
			};
			SignTrampoline = new ModelData
			{
				Model = SignModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Signs/trampoline"),
					SpecularColor = Color.Gray.ToVector4()
				},
			};

			Fence = new ModelData
			{
				Model = FenceModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Fence/fence_d"),
					NormalTexture = cm.Load<Texture>("Models/Fence/Metall_Rost_Normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0,5,0), 0.3f, 10, 10)
			};
			Barbs = new ModelData
			{
				Model = BarbsModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Fence/fence_d"),
					NormalTexture = cm.Load<Texture>("Models/Fence/Metall_Rost_Normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 1.949f, 0), 0.3f, 3.898f, 10)
				
			};
		}
	}
}
