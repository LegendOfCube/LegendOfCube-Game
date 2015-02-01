using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
    /** Type used to specify an entity in the World. */
    public struct Entity
    {
        public readonly UInt32 ID;
        public Entity(UInt32 id)
        {
            ID = id;
        }
    }
}
