using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.Utilities
{
	public class UtilitiesTab : BaseDashboardTab
	{
		public override string TabName => "Utilities";
		public override int OrderNumber => 3;

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public override void Draw()
		{
			DrawNavigationSection();

			EditorGUILayout.Space(20f);

			DrawCachingSection();
		}

		private void DrawNavigationSection()
		{
			EditorGUILayout.LabelField("Navigation", _subHeaderStyle);
			EditorGUILayout.BeginVertical(_boxStyle);
			{
				if (GUILayout.Button("Open \"Persistent Data\" Folder", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(30f)))
				{
					CSTDashboardUtils.RevealFolder(Application.persistentDataPath);
				}
				if (GUILayout.Button("Open \"Streaming Assets\" Folder", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(30f)))
				{
					CSTDashboardUtils.RevealFolder(Application.streamingAssetsPath);
				}
			}
			EditorGUILayout.EndVertical();
		}
		
		private void DrawCachingSection()
		{
			EditorGUILayout.LabelField("Caching", _subHeaderStyle);
			EditorGUILayout.BeginVertical(_boxStyle);
			{
				if (GUILayout.Button("Delete ALL Player Prefs Keys", GUIStyleGetter.Get(GUIStyleType.RedButtonStyle), GUILayout.MinHeight(30f)))
				{
					CSTDashboardUtils.DeleteAllPlayerPrefsKeys();
				}
				if (GUILayout.Button("Clear Asset Bundle Caches", GUIStyleGetter.Get(GUIStyleType.RedButtonStyle), GUILayout.MinHeight(30f)))
				{
					CSTDashboardUtils.ClearAssetBundleCaches();
				}
			}
			EditorGUILayout.EndVertical();
		}
	}
}