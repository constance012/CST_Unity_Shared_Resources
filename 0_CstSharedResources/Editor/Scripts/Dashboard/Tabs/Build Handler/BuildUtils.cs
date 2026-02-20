using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public static class BuildUtils
	{
		public const string PREVIOUS_BUILD_PATH_KEY = "BuildHandler_PreviousBuildPath";

		public static string[] GetIncludedScenes()
		{
			return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
		}

		public static string PromptSelectBuildDirectory()
		{
			string previousBuildPath = EditorPrefs.GetString(PREVIOUS_BUILD_PATH_KEY, string.Empty);

			string buildPath = EditorUtility.SaveFolderPanel("Select Build Output Folder", previousBuildPath, string.Empty);
			EditorPrefs.SetString(PREVIOUS_BUILD_PATH_KEY, buildPath);

			return buildPath;
		}

		public static void CleanBuildDirectory(string buildPath)
		{
			if (Directory.Exists(buildPath))
			{
				try
				{
					Directory.Delete(buildPath, true);
				}
				catch (System.Exception e)
				{
					Debug.LogError($"Failed to clean build directory at \"{buildPath}\".\n\n" +
						$"Reason: {e.Message}.\n");
				}
			}
			else
			{
				Debug.LogWarning($"Build directory at \"{buildPath}\" does not exist. No need to clean it.");
			}

			Directory.CreateDirectory(buildPath);
		}
	}
}