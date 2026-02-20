using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class WindowsPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.StandaloneWindows64;
		public string BuildTargetDisplayName => "Windows";

		private ProjectInfoDataObject _projectInfoData;

		public WindowsPlatformDrawer(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
			// TO-DO: Custom constructor logic.
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Windows Platform Drawer content goes here...", MessageType.Info);
		}

		public void SetupBuildParameters(string buildPath)
		{
			// TO-DO: Implement Windows build logic here.
		}

		public string GetFileExtension()
		{
			return ".exe";
		}
	}
}