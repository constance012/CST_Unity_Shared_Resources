using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class MacPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.StandaloneOSX;
		public string BuildTargetDisplayName => "MacOS";

		private ProjectInfoDataObject _projectInfoData;
		private MacBuildArchitecture _buildArchitecture = MacBuildArchitecture.BothIntelAndApple;
		private bool _shouldCreateXcodeProject = false;

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
		}

		public void Draw()
		{
			GUIDrawHelper.EnumPopupWithLabel("Build Architecture: ", ref _buildArchitecture, GUILayout.MinWidth(300f));

			GUILayout.Space(10f);
			GUIDrawHelper.ToggleWithLabel("Create Xcode Project: ", ref _shouldCreateXcodeProject, GUILayout.MinWidth(300f));
		}

		public void SetupBuildParameters(string buildPath)
		{
			UnityEditor.OSXStandalone.UserBuildSettings.architecture = GetOsArchitecture();
			UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject = _shouldCreateXcodeProject;
		}

		public string GetFileExtension()
		{
			return ".app";
		}

		private OSArchitecture GetOsArchitecture()
		{
			return _buildArchitecture switch
			{
				MacBuildArchitecture.Intel64Bit => OSArchitecture.x64,
				MacBuildArchitecture.AppleSilicon => OSArchitecture.ARM64,
				MacBuildArchitecture.BothIntelAndApple => OSArchitecture.x64ARM64,
				_ => OSArchitecture.x64ARM64
			};
		}
	}

	public enum MacBuildArchitecture
	{
		Intel64Bit = 0,
		AppleSilicon = 1,
		BothIntelAndApple = 2
	}
}