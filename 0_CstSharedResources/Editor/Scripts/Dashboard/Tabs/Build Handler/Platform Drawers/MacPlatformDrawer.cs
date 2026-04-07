using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class MacPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.StandaloneOSX;
		public string BuildTargetDisplayName => "MacOS";

		private ProjectInfoDataObject _projectInfoData;

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
			// TO-DO: Custom initialization logic.
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Mac Platform Drawer content goes here...", MessageType.Info);
		}

		public void SetupBuildParameters(string buildPath)
		{
			// TO-DO: Implement Mac build logic here.
		}

		public string GetFileExtension()
		{
			return ".app";
		}
	}
}