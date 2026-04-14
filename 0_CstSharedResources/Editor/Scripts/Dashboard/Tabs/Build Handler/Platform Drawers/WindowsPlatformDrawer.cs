#if UNITY_STANDALONE_WIN
using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class WindowsPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.StandaloneWindows64;
		public string BuildTargetDisplayName => "Windows";

		private ProjectInfoDataObject _projectInfoData;
		private WindowsBuildArchitecture _buildArchitecture = WindowsBuildArchitecture.Intel64Bit;
		private bool _shouldCopyPDBFiles = false;
		private bool _shouldCreatVisualStudioSolution = false;

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;

			_buildArchitecture = _projectInfoData.TargetPlatform == BuildTarget.StandaloneWindows ? WindowsBuildArchitecture.Intel32Bit : WindowsBuildArchitecture.Intel64Bit;
		}

		public void Draw()
		{
			GUIDrawHelper.EnumPopupWithLabel("Build Architecture: ", ref _buildArchitecture, GUILayout.MinWidth(300f));

			GUILayout.Space(10f);
			GUIDrawHelper.ToggleWithLabel("Copy PDB Files: ", ref _shouldCopyPDBFiles, GUILayout.MinWidth(300f));
			GUIDrawHelper.ToggleWithLabel("Create Visual Studio Solution: ", ref _shouldCreatVisualStudioSolution, GUILayout.MinWidth(300f));
		}

		public void SetupBuildParameters(string buildPath)
		{
			UnityEditor.WindowsStandalone.UserBuildSettings.architecture = GetOsArchitecture();
			UnityEditor.WindowsStandalone.UserBuildSettings.copyPDBFiles = _shouldCopyPDBFiles;
			UnityEditor.WindowsStandalone.UserBuildSettings.createSolution = _shouldCreatVisualStudioSolution;
		}

		public string GetFileExtension()
		{
			return ".exe";
		}

		private OSArchitecture GetOsArchitecture()
		{
			return _buildArchitecture switch
			{
				WindowsBuildArchitecture.Intel64Bit => OSArchitecture.x64,
				WindowsBuildArchitecture.Intel32Bit => OSArchitecture.x86,
				WindowsBuildArchitecture.Arm64Bit => OSArchitecture.ARM64,
				_ => OSArchitecture.x64
			};
		}
	}

	public enum WindowsBuildArchitecture
	{
		Intel64Bit = 0,
		Intel32Bit = 1,
		Arm64Bit = 2
	}
}
#endif