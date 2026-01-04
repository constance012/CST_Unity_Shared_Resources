using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.Utilities
{
	public static class CSTDashboardUtils
	{
		[MenuItem("CST Games/Caching/Delete all PlayerPrefs keys")]
		public static void DeleteAllPlayerPrefsKeys()
		{
			if (EditorUtility.DisplayDialog("Delete all Player Prefs keys", "Are you sure? These changes are irreversible by any means.",
				"Yes, sir!", "Maybe not."))
			{
				PlayerPrefs.DeleteAll();
			}
		}

		[MenuItem("CST Games/Caching/Clear asset bundles caches")]
		public static bool ClearAssetBundleCaches()
		{
			bool success = Caching.ClearCache();

			if (success)
			{
				EditorUtility.DisplayDialog("Caches CLEARED", "Successfully purged Asset Bundle caches.", "OK");
			}
			else
			{
				EditorUtility.DisplayDialog("Cache clearing FAILED", "Some of the caches are still in use. Try exiting play mode and try again.", "OK");
			}

			return success;
		}

		[MenuItem("CST Games/Navigation/Open Persistent Data folder")]
		private static void OpenPersistentDataFolder()
		{
			RevealFolder(Application.persistentDataPath);
		}

		[MenuItem("CST Games/Navigation/Open Streaming Assets folder")]
		private static void OpenStreamingAssetsFolder()
		{
			RevealFolder(Application.streamingAssetsPath);
		}

		public static void RevealFolder(string path)
		{
			EditorUtility.RevealInFinder(path);
		}

		public static void ClearEditorConsole()
		{
			var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
			var clearMethod = logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
			clearMethod?.Invoke(null, null);
		}
	}
}