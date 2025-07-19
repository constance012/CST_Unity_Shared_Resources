using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace CSTGames.Utility
{
	public static class CstMenuItems
	{
		[MenuItem("CST Utilities/Clear Player Prefs")]
		public static void ClearPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}

		[MenuItem("CST Utilities/Data/Open Persistent Data Folder")]
		public static void OpenPersistentDataFolder()
		{
			Process.Start(Application.persistentDataPath);
		}
	}
}