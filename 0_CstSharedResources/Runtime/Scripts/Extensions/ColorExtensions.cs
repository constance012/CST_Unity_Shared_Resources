using UnityEngine;

namespace CST.Shared.Resources
{
	public static class ColorExtensions
	{
		public static Color ExtractRGB(this Color target)
		{
			return new Color(target.r, target.g, target.b);
		}

		public static Color ExtractRGB(this Color target, float alpha)
		{
			return new Color(target.r, target.g, target.b, alpha);
		}
	}
}