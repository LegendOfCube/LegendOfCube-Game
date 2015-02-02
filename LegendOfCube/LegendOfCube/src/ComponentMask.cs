using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	/// <summary>
	/// Specifies a combination of Components.
	/// </summary>
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
		public const UInt64 AFFECTED_BY_GRAVITY = 1 << 3;
		public const UInt64 RECEIVE_INPUT = 1 << 4;
		public const UInt64 MODEL = 1 << 5;
		public const UInt64 TRANSFORM = 1 << 6;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private UInt64 Mask;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public ComponentMask(UInt64 mask)
		{
			Mask = mask;
		}

		// Public Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/// <summary>
		/// Checks whether this ComponentMask contains all the Components in the param Mask.
		/// </summary>
		/// <param name="requirements">The mask to check against</param>
		/// <returns>True if all requirements are filled</returns>
		public bool Satisfies(ComponentMask requirements)
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

		public bool Equals(ComponentMask other)
		{
			return Mask == other.Mask;
		}

		public override int GetHashCode()
		{
			return Mask.GetHashCode();
		}
	}
}
