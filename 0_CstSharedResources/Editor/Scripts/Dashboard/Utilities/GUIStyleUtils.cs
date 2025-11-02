using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Utilities
{
	public static class GUIStyleUtils
	{
		public static Texture2D CreateTexture(int width, int height, Color32 color)
		{
			Color32[] pix = new Color32[width * height];
			for (int i = 0; i < pix.Length; i++)
				pix[i] = color;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels32(pix);
			result.Apply();
			return result;
		}

		public static Texture2D CreateBorderTexture(Color borderColor, Color backgroundColor)
		{
			int width = 128;
			int height = 64;
			int borderThickness = 1;

			Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (x < borderThickness || x >= width - borderThickness || y < borderThickness || y >= height - borderThickness)
					{
						texture.SetPixel(x, y, borderColor);
					}
					else
					{
						texture.SetPixel(x, y, backgroundColor);
					}
				}
			}

			texture.Apply();
			return texture;
		}

		public static Texture2D CreateVerticalGradientTexture(Color top, Color bottom, int height)
		{
			Texture2D texture = new Texture2D(1, height);
			for (int i = 0; i < height; i++)
			{
				texture.SetPixel(0, i, Color.Lerp(bottom, top, (float)i / height));
			}
			texture.Apply();
			return texture;
		}

		public static Texture2D CreateHorizontalGradientTexture(Color left, Color right, int width)
		{
			Texture2D texture = new Texture2D(width, 1);
			for (int i = 0; i < width; i++)
			{
				texture.SetPixel(i, 0, Color.Lerp(left, right, (float)i / width));
			}
			texture.Apply();
			return texture;
		}

		public static Texture2D CreateRadialGradientTexture(Color center, Color edge, int size)
		{
			Texture2D texture = new Texture2D(size, size);
			Vector2 centerPos = new Vector2(size / 2f, size / 2f);
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float dist = Vector2.Distance(centerPos, new Vector2(x, y)) / (size / 2f);
					texture.SetPixel(x, y, Color.Lerp(center, edge, Mathf.Clamp01(dist)));
				}
			}
			texture.Apply();
			return texture;
		}

		public static Texture2D CreateDiagonalGradientTexture(Color topLeft, Color bottomRight, int size)
		{
			Texture2D texture = new Texture2D(size, size);
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float ratio = (float)(x + y) / (2 * size);
					texture.SetPixel(x, y, Color.Lerp(topLeft, bottomRight, ratio));
				}
			}
			texture.Apply();
			return texture;
		}
		public static Texture2D CreateCircularGradientTexture(Color center, Color edge, int size)
		{
			Texture2D texture = new Texture2D(size, size);
			Vector2 centerPos = new Vector2(size / 2f, size / 2f);
			float maxDist = size / 2f;
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float dist = Vector2.Distance(centerPos, new Vector2(x, y));
					texture.SetPixel(x, y, Color.Lerp(center, edge, dist / maxDist));
				}
			}
			texture.Apply();
			return texture;
		}
		public static Texture2D CreateVerticalSymmetricalGradientTexture(Color top, Color middle, Color bottom, int height)
		{
			Texture2D texture = new Texture2D(1, height);
			for (int i = 0; i < height / 2; i++)
			{
				float t = (float)i / (height / 2);
				texture.SetPixel(0, i, Color.Lerp(middle, top, t));
				texture.SetPixel(0, height - 1 - i, Color.Lerp(middle, bottom, t));
			}
			texture.Apply();
			return texture;
		}
	}
}
