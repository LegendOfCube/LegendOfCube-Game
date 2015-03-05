using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class GameObjectTemplates
	{
		public GameObjectTemplate PlayerCube { get; private set; }
		public GameObjectTemplate PlayerCubePlain { get; private set; }
		public GameObjectTemplate RustPlatform { get; private set; }
		public GameObjectTemplate BrickWall { get; private set; }

		public void LoadContent(ContentManager cm)
		{
			var cubeModel = cm.Load<Model>("Models/Cube/cube_clean");
			var plainCubeModel = cm.Load<Model>("Models/cube/cube_plain");
			var platformModel = cm.Load<Model>("Models/Platform/platform");
			var brickWallModel = cm.Load<Model>("Models/Brick_Wall/brick_wall");

			PlayerCube = new GameObjectTemplate
			{
				Model = cubeModel,
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, 0.5f, 0.0f), 1, 1, 1),
				EffectParams = new StandardEffectParams
				{
					DiffuseColor = new Vector4(new Vector3(0.3f), 1.0f),
					EmissiveTexture = cm.Load<Texture>("Models/Cube/cube_emissive"),
					SpecularColor = Color.Gray.ToVector4(),
					EmissiveColor = Color.White.ToVector4()
				}
			};

			PlayerCubePlain = new GameObjectTemplate
			{
				Model = plainCubeModel,
				Obb = OBB.CreateAxisAligned(new Vector3(0.5f), 1, 1, 1),
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

			RustPlatform = new GameObjectTemplate
			{
				Model = platformModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Platform/rusted metal-d"),
					NormalTexture = cm.Load<Texture>("Models/Platform/rust_normal_sharp"),
					SpecularColor = Color.Gray.ToVector4()
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -0.25f, 0.0f), 10.0f, 0.5f, 10.0f)
			};
			
			BrickWall = new GameObjectTemplate
			{
				Model = brickWallModel,
				EffectParams = new StandardEffectParams
				{
					DiffuseTexture = cm.Load<Texture>("Models/Brick_Wall/brick_d"),
					NormalTexture = cm.Load<Texture>("Models/Brick_Wall/brick_n_sharp"),
					SpecularColor = new Vector4(new Vector3(0.1f), 1.0f)
				},
				Obb = OBB.CreateAxisAligned(new Vector3(0.0f, -0.25f, 0.0f), 10.0f, 0.5f, 10.0f)
			};
		}
	}
}
