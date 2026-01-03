using System;

namespace SpoonEditor.Utils
{
	public static class MathU
	{
		public const float epsilon = 0.00001f;

		public static bool IsTheSameAs(this float value, float other)
		{
			return Math.Abs(value - other) < epsilon;
		}
		public static bool IsTheSameAs(this float? value, float? other)
		{
			if(!value.HasValue || !other.HasValue)
				return false;
			
			return Math.Abs(value.Value - other.Value) < epsilon;
		}
	}
}
