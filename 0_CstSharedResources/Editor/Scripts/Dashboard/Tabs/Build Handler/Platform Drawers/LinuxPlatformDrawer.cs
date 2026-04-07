using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class LinuxPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.StandaloneLinux64;
		public string BuildTargetDisplayName => "Linux";

		private ProjectInfoDataObject _projectInfoData;

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
			// TO-DO: Custom initialization logic.
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Linux Platform Drawer content goes here...", MessageType.Info);
		}

		public void SetupBuildParameters(string buildPath)
		{
			// TO-DO: Implement Linux build logic here.
		}

		public string GetFileExtension()
		{
			return ".x86_64";
		}
	}
}