using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public static class BuildUtils
	{
		public static string[] GetIncludedScenes()
		{
			return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
		}

		public static void DeleteBuildDirectory(string buildPath)
		{
			if (Directory.Exists(buildPath))
			{
				try
				{
					Directory.Delete(buildPath, true);
				}
				catch (System.Exception e)
				{
					Debug.LogError($"Failed to delete build directory at \"{buildPath}\".\n\n" +
						$"Reason: {e.Message}.\n");
				}
			}
			else
			{
				Debug.LogWarning($"Build directory at \"{buildPath}\" does not exist. No need to delete it.");
			}
		}
	}
}