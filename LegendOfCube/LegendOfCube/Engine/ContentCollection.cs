using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class ContentCollection
	{

		public SoundEffect respawn { get; private set; }
		public SoundEffect oldJump { get; private set; }
		public SoundEffect jump { get; private set; }
		public SoundEffect wallJump { get; private set; }
		public SoundEffect bounce { get; private set; }
		public SoundEffect hit { get; private set; }
		public SoundEffect select { get; private set; }
		public SoundEffect select2 { get; private set; }

		public ModelData PlayerCube { get; private set; }
		public ModelData PlayerCubePlain { get; private set; }
		public ModelData RustPlatform { get; private set; }
		public ModelData BrickWall { get; private set; }
		public ModelData PlainCube { get; private set; }

		public Model CubeModel { get; private set; }
		public Model PlainCubeModel { get; private set; }
		public Model PlatformModel { get; private set; }
		public Model BrickWallModel { get; private set; }

		public void LoadContent(ContentManager cm)
		{
			respawn = cm.Load<SoundEffect>("SoundEffects/bwiip");
			oldJump = cm.Load<SoundEffect>("SoundEffects/waom");
			wallJump = cm.Load<SoundEffect>("SoundEffects/waom2");
			jump = cm.Load<SoundEffect>("SoundEffects/waom3");
			bounce = cm.Load<SoundEffect>("SoundEffects/boing");
			hit = cm.Load<SoundEffect>("SoundEffects/hit");
			select = cm.Load<SoundEffect>("SoundEffects/select");
			select2 = cm.Load<SoundEffect>("SoundEffects/select2");

			CubeModel = cm.Load<Model>("Models/Cube/cube_clean");
			PlainCubeModel = cm.Load<Model>("Models/cube/cube_plain");
			PlatformModel = cm.Load<Model>("Models/Platform/platform");
			BrickWallModel = cm.Load<Model>("Models/Brick_Wall/brick_wall");

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
		}
	}
}
