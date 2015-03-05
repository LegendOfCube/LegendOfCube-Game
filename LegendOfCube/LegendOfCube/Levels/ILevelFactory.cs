using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	interface ILevelFactory
	{
		World CreateWorld(Game game, GameObjectTemplates gameObjectTemplates);
	}
}
