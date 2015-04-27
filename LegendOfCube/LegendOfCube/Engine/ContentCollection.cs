using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace LegendOfCube.Engine
{
	public class ContentCollection
	{
		public SoundEffect respawn { get; private set; }
		public SoundEffect oldJump { get; private set; }
		public SoundEffect jump { get; private set; }
		public SoundEffect wallJump { get; private set; }
		public SoundEffect whoopJump { get; private set; }
		public SoundEffect whoopJump2 { get; private set; }
		public SoundEffect bounce { get; private set; }
		public SoundEffect hit { get; private set; }
		public SoundEffect select { get; private set; }
		public SoundEffect select2 { get; private set; }
		public Song music { get; private set; }
		public Song level1amb { get; private set; }
		public Song level1full { get; private set; }

		public ModelData PlayerCube2 { get; private set; }
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
		public ModelData PipeWalk { get; private set; }
		public ModelData Pipe { get; private set; }
		public ModelData PipeTurn { get; private set; }
		public ModelData Railing { get; private set; }
		public ModelData GrassSmall { get; private set; }
		public ModelData GrassRound { get; private set; }
		public ModelData GrassLong { get; private set; }
		public ModelData ContainerBlue { get; private set; }
		public ModelData ContainerRed { get; private set; }
		public ModelData ContainerGreen { get; private set; }
		public ModelData Cart1 { get; private set; }
		public ModelData Cart2 { get; private set; }
		public ModelData TrainDoor { get; private set; }
		public ModelData TrainDoorClosed { get; private set; }
		public ModelData Rails { get; private set; }
		public ModelData Locomotive { get; private set; }
		public ModelData WoodPile { get; private set; }
		public ModelData WoodenPlatform { get; private set; }

		// Placeholders
		public ModelData placeholderWall { get; private set; }

		public Model CubeModel2 { get; private set; }
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
		public Model PipeWalkModel { get; private set; }
		public Model PipeModel { get; private set; }
		public Model PipeTurnModel { get; private set; }
		public Model RailingModel { get; private set; }
		public Model GrassSmallModel { get; private set; }
		public Model GrassRoundModel { get; private set; }
		public Model GrassLongModel { get; private set; }
		public Model ContainerModel { get; private set; }
		public Model Cart1Model { get; private set; }
		public Model Cart2Model { get; private set; }
		public Model TrainDoorModel { get; private set; }
		public Model TrainDoorClosedModel { get; private set; }
		public Model RailsModel { get; private set; }
		public Model LocomotiveModel { get; private set; }
		public Model WoodPileModel { get; private set; }
		public Model WoodenPlatformModel { get; private set; }

		public void LoadContent(ContentManager cm)
		{
			CubeModel2 = cm.Load<Model>("Models/Cube/newcube_ep");
			respawn = cm.Load<SoundEffect>("SoundEffects/bwiip");
			oldJump = cm.Load<SoundEffect>("SoundEffects/waom");
			wallJump = cm.Load<SoundEffect>("SoundEffects/waom2");
			jump = cm.Load<SoundEffect>("SoundEffects/waom3");
			whoopJump = cm.Load<SoundEffect>("SoundEffects/whoop");
			whoopJump2 = cm.Load<SoundEffect>("SoundEffects/whoopShort");
			bounce = cm.Load<SoundEffect>("SoundEffects/boing");
			hit = cm.Load<SoundEffect>("SoundEffects/hit");
			select = cm.Load<SoundEffect>("SoundEffects/select");
			select2 = cm.Load<SoundEffect>("SoundEffects/select2");
			music = cm.Load<Song>("SoundEffects/LoC_music");
			level1amb = cm.Load<Song>("SoundEffects/LoC_level1_amb");
			level1full = cm.Load<Song>("SoundEffects/LoC_full");

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
			PipeWalkModel = cm.Load<Model>("Models/Pipe/pipe_walk");
			PipeModel = cm.Load<Model>("Models/Pipe/pipe");
			PipeTurnModel = cm.Load<Model>("Models/Pipe/pipe_turn");
			RailingModel = cm.Load<Model>("Models/Railing/railing");
			GrassSmallModel = cm.Load<Model>("Models/Vegetation/small_grass");
			//GrassRoundModel = cm.Load<Model>("Models/Vegetation/grass_round_optimized");
			GrassLongModel = cm.Load<Model>("Models/Vegetation/grass_long_optimized");
			ContainerModel = cm.Load<Model>("Models/Container/container_mapped");
			Cart1Model = cm.Load<Model>("Models/Train/cart1");
			Cart2Model = cm.Load<Model>("Models/Train/cart2");
			TrainDoorModel = cm.Load<Model>("Models/Train/dooropen_fix");
			TrainDoorClosedModel = cm.Load<Model>("Models/Train/doorclosed");
			RailsModel = cm.Load<Model>("Models/Train/rails");
			LocomotiveModel = cm.Load<Model>("Models/Train/locomotive");
			WoodPileModel = cm.Load<Model>("Models/Wood_Stack/wood_pile");
			WoodenPlatformModel = cm.Load<Model>("Models/Wooden_Platform/wood_platform");


			placeholderWall = new ModelData
			{
				Model = Ground50x50,
				Obb = OBB.CreateAxisAligned(Vector3.Zero, 50, 2, 50),
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Ground/groundconcrete_d"),
					//DiffuseColor = Color.DarkGray.ToVector4(),
				}
			};

			PlayerCube2 = new ModelData
			{
				Model = CubeModel2,
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 0.5f, 0.0f), 1, 1, 1),
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Cube/testdiff"),
					EmissiveTexture = cm.Load<Texture>("Models/Cube/testemissive"),
					SpecularTexture = cm.Load<Texture>("Models/Cube/cubespec"),
					//SpecularColor = Color.Gray.ToVector4(),
					//EmissiveColor = Color.Black.ToVector4()
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
					//DiffuseColor = Color.White.ToVector4(),
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
					//DiffuseColor = Color.White.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/new_normal_disp"),
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
					//DiffuseColor = Color.White.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/new_normal_disp"),
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
					//DiffuseColor = Color.White.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/new_normal_disp"),
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
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/new_normal_disp"),
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
					//DiffuseColor = Color.Gray.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Platform/pipe_normal2"),
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
					//DiffuseColor = Color.LightGray.ToVector4(),
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
					//DiffuseColor = Color.White.ToVector4(),
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
					//DiffuseColor = Color.White.ToVector4(),
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
					//DiffuseColor = Color.Red.ToVector4(),
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
					//DiffuseColor = Color.Black.ToVector4(),
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
				Obb = OBB.CreateAxisAligned(new Vector3(0,5,0), 0.4f, 10, 10)
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

			PipeWalk = new ModelData
			{
				Model = PipeWalkModel,
				EffectParams = new StandardEffectParams
				{
					//DiffuseColor = Color.WhiteSmoke.ToVector4(),
					DiffuseTexture = cm.Load<Texture>("Models/Pipe/pipewalk_diffuse"),
					NormalTexture = cm.Load<Texture>("Models/Pipe/pipewalk_normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 0.35f, 0), 30, 6, 5.8f)

			};
			Pipe = new ModelData
			{
				Model = PipeModel,
				EffectParams = new StandardEffectParams
				{
					//DiffuseColor = Color.WhiteSmoke.ToVector4(),
					DiffuseTexture = cm.Load<Texture>("Models/Pipe/pipewalk_diffuse"),
					NormalTexture = cm.Load<Texture>("Models/Pipe/pipewalk_normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 0.35f, 0), 30, 6, 5.8f)

			};
			PipeTurn = new ModelData
			{
				Model = PipeTurnModel,
				EffectParams = new StandardEffectParams
				{
					//DiffuseColor = Color.WhiteSmoke.ToVector4(),
					DiffuseTexture = cm.Load<Texture>("Models/Pipe/pipe_d"),
					NormalTexture = cm.Load<Texture>("Models/Pipe/pipewalk_normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(-22.5f, 0.35f, 0), 15, 5, 5.8f)

			};

			Railing = new ModelData
			{
				Model = RailingModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Railing/blue_metal"),
					//DiffuseColor = Color.Blue.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 0.3f, 5), 0.15f, 0.55f, 10)

			};

			GrassSmall = new ModelData
			{
				Model = GrassSmallModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Vegetation/grass_d"),
					//DiffuseColor = Color.Blue.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Vegetation/grass_n"),
					SpecularTexture = cm.Load<Texture>("Models/Vegetation/grass_s")
				}
			};
			GrassRound = new ModelData
			{
				Model = GrassRoundModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Vegetation/grass_d"),
					//DiffuseColor = Color.Blue.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Vegetation/grass_n"),
					SpecularTexture = cm.Load<Texture>("Models/Vegetation/grass_s")
				}
			};
			GrassLong = new ModelData
			{
				Model = GrassLongModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Vegetation/grass_d"),
					//DiffuseColor = Color.Blue.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Vegetation/grass_n"),
					SpecularTexture = cm.Load<Texture>("Models/Vegetation/grass_s")
				}
			};

			ContainerRed = new ModelData
			{
				Model = ContainerModel,
				EffectParams = new StandardEffectParams
				{
					//DiffuseTexture = cm.Load<Texture>("Models/Railing/blue_metal"),
					DiffuseColor = Color.DarkRed.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
				//Obb = OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)
			};
			ContainerBlue = new ModelData
			{
				Model = ContainerModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Railing/blue_metal"),
					//DiffuseColor = Color.DarkRed.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
				//Obb = OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)
			};
			ContainerGreen = new ModelData
			{
				Model = ContainerModel,
				EffectParams = new StandardEffectParams
				{
					//DiffuseTexture = cm.Load<Texture>("Models/Railing/blue_metal"),
					DiffuseColor = Color.DarkGreen.ToVector4(),
					NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
				//Obb = OBB.CreateAxisAligned(new Vector3(0, 10.16f, 0), 45.08f, 20.32f, 16.544f)
			};

			Cart1 = new ModelData
			{
				Model = Cart1Model,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Hanging_Platform/metal_plate-diffuse"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			Cart2 = new ModelData
			{
				Model = Cart2Model,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Hanging_Platform/metal_plate-diffuse"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			TrainDoor = new ModelData
			{
				Model = TrainDoorModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Train/traindoor_d"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			TrainDoorClosed = new ModelData
			{
				Model = TrainDoorClosedModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Train/traindoor_d"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			Rails = new ModelData
			{
				Model = RailsModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Platform/metal_rust_tex_01"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			Locomotive = new ModelData
			{
				Model = LocomotiveModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Train/tgm3_1010a"),
					//DiffuseColor = Color.DarkOrange.ToVector4(),
					//NormalTexture = cm.Load<Texture>("Models/Railing/blue_metal_normal")
				},
			};
			
			WoodPile = new ModelData
			{
				Model = WoodPileModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Wood_Stack/wood-pile-d"),
					NormalTexture = cm.Load<Texture>("Models/Wood_Stack/wood-pile-n"),
					SpecularTexture = cm.Load<Texture>("Models/Wood_Stack/wood-pile-s")
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0, 0.5f, 0), 1.5f, 1.3f, 3.3f)
			};
			
			WoodenPlatform = new ModelData
			{
				Model = WoodenPlatformModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Wooden_Platform/defuse"),
					NormalTexture = cm.Load<Texture>("Models/Wooden_Platform/normals"),
					SpecularTexture = cm.Load<Texture>("Models/Wooden_Platform/spec"),
				},
			};

		}
	}
}
