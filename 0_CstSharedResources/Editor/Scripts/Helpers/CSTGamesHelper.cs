using UnityEditor;
using UnityEngine;

namespace CST.Shared.Resources.Editor
{	
	public static class CSTGamesHelper
	{
		[MenuItem("CST Games/Player Prefs/Delete All Keys")]
		public static void DeleteAllKeys()
		{
			if (EditorUtility.DisplayDialog("Delete All Player Prefs Keys", "Are you sure? These changes are irreversible by any means.",
				"Yes, sir!", "Maybe not."))
			{
				PlayerPrefs.DeleteAll();
			}
		}

		[MenuItem("CST Games/Data/Open Persistent Data Folder")]
		public static void OpenPersistentDataFolder()
		{
			EditorUtility.RevealInFinder(Application.persistentDataPath);
		}
	}
}