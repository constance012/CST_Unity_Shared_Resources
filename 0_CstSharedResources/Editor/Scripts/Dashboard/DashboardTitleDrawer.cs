using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard
{
	public class DashboardTitleDrawer : IDrawable
	{
		private GUIStyle _titleStyle;

		public DashboardTitleDrawer()
		{
			InitStyle();
		}

		public void Draw()
		{
			EditorGUILayout.LabelField("CST DASHBOARD", _titleStyle, GUILayout.ExpandWidth(true), GUILayout.Height(40));
		}

		private void InitStyle()
		{
			_titleStyle = new(EditorStyles.boldLabel);
			
			_titleStyle.fontSize = 20;
			_titleStyle.fontStyle = FontStyle.BoldAndItalic;
			_titleStyle.normal.background = GUIStyleUtils.CreateRadialGradientTexture(new Color(0.702f, 0.557f, 0.388f, 1.000f), new Color(0.286f, 0.286f, 0.286f, 1.000f), 256);
			_titleStyle.normal.textColor = new Color(0.859f, 0.859f, 0.859f, 1.000f);
			_titleStyle.alignment = TextAnchor.MiddleCenter;
			_titleStyle.padding = new RectOffset(10, 10, 10, 10);
			_titleStyle.margin = new RectOffset(0, 0, 0, 10);
		}
	}
}