using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	/** Specifies a combination of Components. */
	public struct ComponentMask
	{
		// Mask Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Notes: 'const' implies 'static' in C#.
		// '1 << x' means that the bit at the x:th position is set.

		public const UInt64 NO_COMPONENTS = 0;
		public const UInt64 POSITION = 1 << 0;
		public const UInt64 VELOCITY = 1 << 1;
		public const UInt64 ACCELERATION = 1 << 2;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public readonly UInt64 Mask;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public ComponentMask(UInt64 mask)
		{
			Mask = mask;
		}

		// Public Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/** Checks whether this ComponentMask contains all the Components in the param Mask. */
		public bool satisfies(ComponentMask requirements)
		{
			return ((this.Mask & requirements.Mask) == requirements.Mask);
		}

		// Operators
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public override bool Equals(object obj)
		{
			if (!(obj is ComponentMask)) return false;
			return this == ((ComponentMask)obj);
		}

		public static bool operator ==(ComponentMask lhs, ComponentMask rhs)
		{
			return lhs.Mask == rhs.Mask;
		}

		public static bool operator !=(ComponentMask lhs, ComponentMask rhs)
		{
			return lhs.Mask != rhs.Mask;
		}
	}
}
