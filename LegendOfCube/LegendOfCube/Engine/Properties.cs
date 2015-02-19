using System;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Specifies a combination of Components.
	/// </summary>
	public struct Properties
	{
		// Mask Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		// Notes: 'const' implies 'static' in C#.
		// '1 << x' means that the bit at the x:th position is set.

		public const UInt64 NO_PROPERTIES = 0;
		public const UInt64 TRANSFORM = 1 << 0;
		public const UInt64 VELOCITY = 1 << 1;
		public const UInt64 ACCELERATION = 1 << 2;
		public const UInt64 GRAVITY_FLAG = 1 << 3;
		public const UInt64 INPUT_FLAG = 1 << 4;
		public const UInt64 MODEL = 1 << 5;
		public const UInt64 FULL_LIGHT_EFFECT = 1 << 6;
		public const UInt64 MODEL_SPACE_BV = 1 << 7;
		public const UInt64 DEATH_ZONE_FLAG = 1 << 8;

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private UInt64 mask;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public Properties(UInt64 mask)
		{
			this.mask = mask;
		}

		// Public Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		/// <summary>
		/// Checks whether these Properties contains all the Properties in the param mask.
		/// </summary>
		/// <param name="requirements">The mask to check against</param>
		/// <returns>True if all requirements are fulfilled</returns>
		public bool Satisfies(Properties requirements)
		{
			return Satisfies(requirements.mask);
		}

		public bool Satisfies(UInt64 requirements)
		{
			return ((this.mask & requirements) == requirements);
		}

		public void Add(UInt64 add)
		{
			mask |= add;
		}

		public void Subtact(UInt64 subtract)
		{
			mask &= (~subtract);
		}

		public void Add(Properties add)
		{
			mask |= add.mask;
		}

		public void Subtact(Properties subtract)
		{
			mask &= (~subtract.mask);
		}

		public bool Equals(Properties other)
		{
			return mask == other.mask;
		}

		public override bool Equals(object obj)
		{
			return obj is Properties && Equals((Properties)obj);
		}

		public override int GetHashCode()
		{
			return mask.GetHashCode();
		}

		// Operators
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static bool operator ==(Properties c1, Properties c2)
		{
			return c1.mask == c2.mask;
		}

		public static bool operator !=(Properties c1, Properties c2)
		{
			return c1.mask != c2.mask;
		}
	}
}
