using System;
using System.IO;
using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class BuildHandler
	{
		public BuildTarget ActiveBuildTarget => _activeBuildTarget;
		public BuildTargetGroup SelectedBuildTargetGroup => _selectedBuildTargetGroup;
		public bool NeedToSwitchBuildTarget => _needToSwitchBuildTarget;

		private ProjectInfoDataObject _projectInfoData;
		private readonly BuildTarget _activeBuildTarget;
		private readonly BuildTargetGroup _selectedBuildTargetGroup;
		private DevelopBuildOption _developBuildOptions;
		private BuildCompressionOption _compressionOption;
		private bool _needToSwitchBuildTarget;
		private bool _isCleanBuild;
		private IBuildPlatformDrawable _platformDrawer;

		public BuildHandler(ProjectInfoDataObject projectInfoData, IBuildPlatformDrawable platformDrawer)
		{
			_activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;

			_projectInfoData = projectInfoData;
			_platformDrawer = platformDrawer;
			_selectedBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(_projectInfoData.TargetPlatform);

			_needToSwitchBuildTarget = _projectInfoData.TargetPlatform != _activeBuildTarget;
		}

#region Main Draw Method
		public void DrawMainBuildSection()
		{
			GUILayout.Space(10f);
			GUIDrawHelper.ToggleWithLabel("Clean Build: ", ref _isCleanBuild, GUILayout.MinWidth(300f));

			GUIDrawHelper.EnumPopupWithLabel("Build Compression Format: ", ref _compressionOption, GUILayout.MinWidth(300f));
			
			GUIDrawHelper.EnumFlagsFieldWithLabel("Development Build Options: ", ref _developBuildOptions, GUILayout.MinWidth(300f));

			GUILayout.Space(10f);
			if (GUILayout.Button("BUILD PLAYER", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(50)))
			{
				ProcessBuildPlayer();
			}
		}
#endregion

#region Build Handling Methods
		public void SwitchActiveBuildTarget()
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(_selectedBuildTargetGroup, _projectInfoData.TargetPlatform);
			_needToSwitchBuildTarget = false;
		}

		public void ProcessBuildPlayer()
		{
			string parentBuildPath = BuildUtils.PromptSelectBuildDirectory();

			if (string.IsNullOrEmpty(parentBuildPath))
			{
				Debug.LogWarning("Build cancelled: No build directory was selected.");
				return;
			}

			string targetBuildPath = Path.Combine(parentBuildPath, _platformDrawer.BuildTargetDisplayName);
			
			BuildUtils.CleanBuildDirectory(targetBuildPath);

			string fileNamePath = ConstructBuildLocationPath(targetBuildPath);

			_platformDrawer.SetupBuildParameters(targetBuildPath);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			GC.Collect();

			Resources.UnloadUnusedAssets();
			EditorUtility.UnloadUnusedAssetsImmediate();

			BuildPlayerOptions buildPlayerOptions = new()
			{
				scenes = BuildUtils.GetIncludedScenes(),
				locationPathName = fileNamePath,
				target = _platformDrawer.BuildTarget,
				options = SetupBuildOptions()
			};

			var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

			if (buildReport.summary.result != BuildResult.Succeeded)
			{
				throw new Exception($"[CST Dashboard] FAILED to build player with a total of {buildReport.summary.totalErrors} errors.");
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			if (File.Exists(fileNamePath))
			{
				EditorUtility.RevealInFinder(fileNamePath);
			}
			else
			{
				EditorUtility.RevealInFinder(targetBuildPath);
			}

			Debug.Log($"[CST Dashboard] Build SUCCEEDED!\n\n" +
				$"Total time: {buildReport.summary.totalTime}.\n" +
				$"Output path: \"{fileNamePath}\"");
		}
#endregion

#region Private Helper Methods
		private BuildOptions SetupBuildOptions()
		{
			BuildOptions buildOptions = BuildOptions.None;

			if (_compressionOption != BuildCompressionOption.None)
			{
				buildOptions |= _compressionOption == BuildCompressionOption.Lz4 ? BuildOptions.CompressWithLz4 : BuildOptions.CompressWithLz4HC;
			}

			if (_isCleanBuild)
			{
				buildOptions |= BuildOptions.CleanBuildCache;
			}

			if (_developBuildOptions.HasFlag(DevelopBuildOption.AutoConnectProfiler))
			{
				buildOptions |= BuildOptions.ConnectWithProfiler;
			}

			if (_developBuildOptions.HasFlag(DevelopBuildOption.DeepProfilingSupport))
			{
				buildOptions |= BuildOptions.EnableDeepProfilingSupport;
			}

			if (_developBuildOptions.HasFlag(DevelopBuildOption.ScriptDebugging))
			{
				buildOptions |= BuildOptions.AllowDebugging;
			}

			return buildOptions;
		}

		private string ConstructBuildLocationPath(string buildPath)
		{
			string fileExtension = _platformDrawer.GetFileExtension();

			string fileName = string.IsNullOrEmpty(fileExtension) ? buildPath :
				Path.Combine(buildPath, $"{Application.productName}_{_projectInfoData.Version}_{_projectInfoData.BuildNumber}{fileExtension}");
			
			return fileName;
		}
#endregion
	}

	[Flags]
	public enum DevelopBuildOption
	{
		None = 0,
		AutoConnectProfiler = 1,
		DeepProfilingSupport = 2,
		ScriptDebugging = 4,
	}

	public enum BuildCompressionOption
	{
		None,
		Lz4,
		Lz4HC,
	}
}