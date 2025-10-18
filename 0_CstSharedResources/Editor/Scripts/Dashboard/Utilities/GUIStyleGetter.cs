using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CST.Shared.Resources.Editor.Dashboard.Utilities
{
	public enum GUIStyleType
	{
		GreenButtonStyle,
		YellowButtonStyle,
		CyanButtonStyle,
		RedButtonStyle,
		SelectedButtonStyle,
		NormalButtonStyle,
		BoxButtonStyle,
		BoxContentStyle,
		SubHeaderStyle,
		BoxStyle,
		CyanBoxStyle,
		SubHeaderTexture,
		BoxTexture,
		RowBoxStyle,
		RowRedBoxStyle,
		RowGreenBoxStyle,
		RowBlueBoxStyle,
		RowYellowBoxStyle,
		RowHighLightBoxStyle,
		RedTextStyle,
		GreenTextStyle,
		CyanTextStyle,
		GrayTextStyle,
		LightGrayTextStyle,
		BlueTextStyle,
		WhiteTextStyle,
		YellowTextStyle,
		WhiteTextCenterStyle,
	}

	public static class GUIStyleGetter
	{
		private static Dictionary<GUIStyleType, GUIStyle> _styles = null;

		public static GUIStyle Get(GUIStyleType type)
		{
			if (_styles == null)
			{
				InitStyle();
			}

			return _styles[type];
		}

		private static void InitStyle()
		{
			_styles = new Dictionary<GUIStyleType, GUIStyle>();

			var greenButtonStyle = new GUIStyle(GUI.skin.button);
			greenButtonStyle.normal.textColor = Color.green;
			greenButtonStyle.fontStyle = FontStyle.Bold;
			greenButtonStyle.alignment = TextAnchor.MiddleCenter;

			var yellowButtonStyle = new GUIStyle(GUI.skin.button);
			yellowButtonStyle.normal.textColor = new Color(1.0f, 0.92f, 0.016f);
			yellowButtonStyle.fontStyle = FontStyle.Bold;
			yellowButtonStyle.alignment = TextAnchor.MiddleCenter;

			var cyanButtonStyle = new GUIStyle(GUI.skin.button);
			cyanButtonStyle.normal.textColor = new Color(0.0f, 1.0f, 1.0f);
			cyanButtonStyle.fontStyle = FontStyle.Bold;
			cyanButtonStyle.alignment = TextAnchor.MiddleCenter;

			var redButtonStyle = new GUIStyle(GUI.skin.button);
			redButtonStyle.normal.textColor = new Color(1f, .4f, .4f);
			redButtonStyle.fontStyle = FontStyle.Bold;
			redButtonStyle.alignment = TextAnchor.MiddleCenter;

			var selectedButtonStyle = CreateButtonStyle(true);
			var normalButtonStyle = CreateButtonStyle(false);

			var boxButtonStyle = new GUIStyle(GUI.skin.box);
			boxButtonStyle.padding = new RectOffset(4, 4, 4, 4);
			boxButtonStyle.margin = new RectOffset(2, 2, 2, 2);
			boxButtonStyle.alignment = TextAnchor.MiddleCenter;
			boxButtonStyle.normal.textColor = Color.white;
			boxButtonStyle.fontSize = 10;
			boxButtonStyle.normal.background = GUIStyleUtils.CreateTexture(2, 2, new Color(0.2f, 0.3f, 0.4f, 1.0f));
			boxButtonStyle.border = new RectOffset(2, 2, 2, 2);

			var boxContentStyle = new GUIStyle(GUI.skin.label);
			boxContentStyle.padding = new RectOffset(4, 4, 4, 4);
			boxContentStyle.margin = new RectOffset(2, 2, 2, 2);
			boxContentStyle.alignment = TextAnchor.MiddleLeft;
			boxContentStyle.fontSize = 10;
			boxContentStyle.normal.textColor = Color.white;
			boxContentStyle.normal.background = GUIStyleUtils.CreateTexture(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.8f));

			var subHeaderTexture = GUIStyleUtils.CreateRadialGradientTexture(
				new Color(0.25f, 0.25f, 0.25f),
				new Color(0.15f, 0.15f, 0.15f),
				30);

			var subHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
			subHeaderStyle.fontSize = 14;
			subHeaderStyle.normal.background = subHeaderTexture;
			subHeaderStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
			subHeaderStyle.alignment = TextAnchor.MiddleLeft;
			subHeaderStyle.padding = new RectOffset(3, 0, 0, 0);
			subHeaderStyle.margin = new RectOffset(0, 0, 0, 0);

			var boxStyle = new GUIStyle(EditorStyles.helpBox);
			boxStyle.normal.background = GUIStyleUtils.CreateRadialGradientTexture(
				new Color(0.28f, 0.28f, 0.28f),
				new Color(0.20f, 0.20f, 0.20f),
				50);
			boxStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			boxStyle.padding = new RectOffset(2, 2, 2, 2);
			boxStyle.margin = new RectOffset(2, 2, 2, 2);

			var cyanBoxStyle = new GUIStyle(GUI.skin.box);
			cyanBoxStyle.normal.background = GUIStyleUtils.CreateTexture(1, 1, new Color(0f, 0.3f, 0.3f));
			cyanBoxStyle.padding = new RectOffset(2, 2, 2, 2);
			cyanBoxStyle.margin = new RectOffset(2, 2, 2, 2);

			var headerTextBoxStyle = new GUIStyle(GUI.skin.box);
			headerTextBoxStyle.fontStyle = FontStyle.Bold;
			headerTextBoxStyle.normal.textColor = Color.white;
			headerTextBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowBoxStyle = new GUIStyle(GUI.skin.box);
			rowBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowRedBoxStyle = new GUIStyle(GUI.skin.box);
			rowRedBoxStyle.normal.background = GUIStyleUtils.CreateHorizontalGradientTexture(new Color(0.7f, 0.3f, 0.3f), Color.gray, 100);
			rowRedBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowGreenBoxStyle = new GUIStyle(GUI.skin.box);
			rowGreenBoxStyle.normal.background = GUIStyleUtils.CreateHorizontalGradientTexture(new Color(0.3f, 0.6f, 0.3f), Color.gray, 100);
			rowGreenBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowYellowBoxStyle = new GUIStyle(GUI.skin.box);
			rowYellowBoxStyle.normal.background = GUIStyleUtils.CreateHorizontalGradientTexture(new Color(0.7f, 0.7f, 0.3f), Color.gray, 100);
			rowYellowBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowBlueBoxStyle = new GUIStyle(GUI.skin.box);
			rowBlueBoxStyle.normal.background = GUIStyleUtils.CreateHorizontalGradientTexture(new Color(0.3f, 0.3f, 0.6f), Color.gray, 100);
			rowBlueBoxStyle.alignment = TextAnchor.MiddleCenter;

			var rowHighLightBoxStyle = new GUIStyle(GUI.skin.box);
			rowHighLightBoxStyle.normal.background = GUIStyleUtils.CreateCircularGradientTexture(new Color(0.5f, 0.45f, 0.25f), Color.gray, 100);
			rowHighLightBoxStyle.alignment = TextAnchor.MiddleCenter;

			var redTextStyle = new GUIStyle(GUI.skin.label);
			redTextStyle.normal.textColor = Color.red;
			redTextStyle.alignment = TextAnchor.MiddleLeft;

			var greenTextStyle = new GUIStyle(GUI.skin.label);
			greenTextStyle.normal.textColor = Color.green;
			greenTextStyle.alignment = TextAnchor.MiddleLeft;

			var cyanTextStyle = new GUIStyle(GUI.skin.label);
			cyanTextStyle.normal.textColor = Color.cyan;
			cyanTextStyle.alignment = TextAnchor.MiddleLeft;

			var grayTextStyle = new GUIStyle(GUI.skin.label);
			grayTextStyle.normal.textColor = Color.gray;
			grayTextStyle.alignment = TextAnchor.MiddleLeft;

			var lightGrayTextStyle = new GUIStyle(GUI.skin.label);
			lightGrayTextStyle.normal.textColor = new Color(.8f, .8f, .8f);
			lightGrayTextStyle.alignment = TextAnchor.MiddleLeft;

			var blueTextStyle = new GUIStyle(GUI.skin.label);
			blueTextStyle.normal.textColor = new Color(0.0f, 0.3f, 0.6f);
			blueTextStyle.alignment = TextAnchor.MiddleLeft;

			var whiteTextStyle = new GUIStyle(GUI.skin.label);
			whiteTextStyle.normal.textColor = Color.white;
			whiteTextStyle.alignment = TextAnchor.MiddleLeft;

			var yellowTextStyle = new GUIStyle(GUI.skin.label);
			yellowTextStyle.normal.textColor = Color.yellow;
			yellowTextStyle.alignment = TextAnchor.MiddleLeft;

			var whiteTextCenterStyle = new GUIStyle(GUI.skin.label);
			whiteTextCenterStyle.normal.textColor = Color.white;
			whiteTextCenterStyle.alignment = TextAnchor.MiddleCenter;

			_styles.Add(GUIStyleType.GreenButtonStyle, greenButtonStyle);
			_styles.Add(GUIStyleType.YellowButtonStyle, yellowButtonStyle);
			_styles.Add(GUIStyleType.CyanButtonStyle, cyanButtonStyle);
			_styles.Add(GUIStyleType.RedButtonStyle, redButtonStyle);
			_styles.Add(GUIStyleType.SelectedButtonStyle, selectedButtonStyle);
			_styles.Add(GUIStyleType.NormalButtonStyle, normalButtonStyle);
			_styles.Add(GUIStyleType.BoxButtonStyle, boxButtonStyle);
			_styles.Add(GUIStyleType.BoxContentStyle, boxContentStyle);
			_styles.Add(GUIStyleType.SubHeaderStyle, subHeaderStyle);
			_styles.Add(GUIStyleType.BoxStyle, boxStyle);
			_styles.Add(GUIStyleType.CyanBoxStyle, cyanBoxStyle);
			_styles.Add(GUIStyleType.RowBoxStyle, rowBoxStyle);
			_styles.Add(GUIStyleType.RowRedBoxStyle, rowRedBoxStyle);
			_styles.Add(GUIStyleType.RowGreenBoxStyle, rowGreenBoxStyle);
			_styles.Add(GUIStyleType.RowBlueBoxStyle, rowBlueBoxStyle);
			_styles.Add(GUIStyleType.RowYellowBoxStyle, rowYellowBoxStyle);
			_styles.Add(GUIStyleType.RowHighLightBoxStyle, rowHighLightBoxStyle);
			_styles.Add(GUIStyleType.RedTextStyle, redTextStyle);
			_styles.Add(GUIStyleType.GreenTextStyle, greenTextStyle);
			_styles.Add(GUIStyleType.CyanTextStyle, cyanTextStyle);
			_styles.Add(GUIStyleType.GrayTextStyle, grayTextStyle);
			_styles.Add(GUIStyleType.LightGrayTextStyle, lightGrayTextStyle);
			_styles.Add(GUIStyleType.BlueTextStyle, blueTextStyle);
			_styles.Add(GUIStyleType.WhiteTextStyle, whiteTextStyle);
			_styles.Add(GUIStyleType.YellowTextStyle, yellowTextStyle);
			_styles.Add(GUIStyleType.WhiteTextCenterStyle, whiteTextCenterStyle);
		}

		private static GUIStyle CreateButtonStyle(bool isSelected)
		{
			GUIStyle style = new GUIStyle(GUI.skin.button);

			style.fontSize = 12;
			style.fontStyle = FontStyle.Bold;
			style.margin = new RectOffset(3, 3, 3, 3);
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;

			style.hover.background = GUIStyleUtils.CreateBorderTexture(new Color(0f, 1f, 1f), new Color(0.2f, 0.2f, 0.4f));
			style.hover.scaledBackgrounds = new Texture2D[] { style.hover.background };
			style.hover.textColor = Color.yellow;

			style.active.background = GUIStyleUtils.CreateBorderTexture(new Color(1f, 0.2f, 0.2f), new Color(0.3f, 0.0f, 0.0f));
			style.active.scaledBackgrounds = new Texture2D[] { style.active.background };
			style.active.textColor = Color.red;

			if (isSelected)
			{
				style.normal.background = GUIStyleUtils.CreateBorderTexture(new Color(1f, 1f, 0f), new Color(0.1f, 0.1f, 0.3f));
				style.normal.scaledBackgrounds = new Texture2D[] { style.normal.background };
				style.normal.textColor = Color.white;
				style.border = new RectOffset(2, 2, 2, 2);
			}

			return style;
		}
	}
}
