using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class UnknownPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.NoTarget;
		public string BuildTargetDisplayName => "Unknown";

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Unknown Platform selected, no specific build options available.\n" +
				"Try installing the necessary platform support module in the Unity Hub and restart the Editor.\n" +
				"Or select a different platform in the Project Info settings.", MessageType.Warning);
		}

		public void SetupBuildParameters(string buildPath)
		{
			
		}

		public string GetFileExtension()
		{
			return string.Empty;
		}
	}
}