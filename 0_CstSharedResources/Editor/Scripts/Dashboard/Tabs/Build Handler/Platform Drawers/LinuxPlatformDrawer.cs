#if UNITY_STANDALONE_LINUX
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
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox("Linux Platform does not require any specific build settings.", MessageType.Info);
		}

		public void SetupBuildParameters(string buildPath)
		{
			
		}

		public string GetFileExtension()
		{
			return ".x86_64";
		}
	}
}
#endif