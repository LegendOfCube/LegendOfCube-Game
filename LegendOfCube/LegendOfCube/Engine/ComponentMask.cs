using System;

namespace LegendOfCube.Engine
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

		private UInt64 mask;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public ComponentMask(UInt64 mask)
		{
			this.mask = mask;
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
			return ((this.mask & requirements.mask) == requirements.mask);
		}

		public void Add(UInt64 add)
		{
			mask |= add;
		}

		public void Subtact(UInt64 subtract)
		{
			mask &= (~subtract);
		}

		public void Add(ComponentMask add)
		{
			mask |= add.mask;
		}

		public void Subtact(ComponentMask subtract)
		{
			mask &= (~subtract.mask);
		}

		public bool Equals(ComponentMask other)
		{
			return mask == other.mask;
		}

		public override bool Equals(object obj)
		{
			return obj is ComponentMask && Equals((ComponentMask)obj);
		}

		public override int GetHashCode()
		{
			return mask.GetHashCode();
		}

		// Operators
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static bool operator ==(ComponentMask c1, ComponentMask c2)
		{
			return c1.mask == c2.mask;
		}

		public static bool operator !=(ComponentMask c1, ComponentMask c2)
		{
			return c1.mask != c2.mask;
		}
	}
}
