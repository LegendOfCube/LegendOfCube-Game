using Microsoft.Xna.Framework;

namespace LegendOfCube.Engine
{
	interface ILevelFactory
	{
		World CreateWorld(Game game, GameObjectTemplateCollection gameObjectTemplateCollection);
	}
}
