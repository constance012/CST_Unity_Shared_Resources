using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class WebPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.WebGL;
		public string BuildTargetDisplayName => "WebGL";

		private ProjectInfoDataObject _projectInfoData;

		public WebPlatformDrawer(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
			// TO-DO: Custom constructor logic.
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Web Platform Drawer content goes here...", MessageType.Info);
		}

		public void SetupBuildParameters(string buildPath)
		{
			// TO-DO: Implement WebGL build logic here.
		}

		public string GetFileExtension()
		{
			return string.Empty;
		}
	}
}