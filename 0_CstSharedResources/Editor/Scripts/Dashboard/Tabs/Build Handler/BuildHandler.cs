using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class BuildHandler
	{
		public BuildTarget ActiveBuildTarget => _activeBuildTarget;
		public BuildTargetGroup SelectedBuildTargetGroup => _selectedBuildTargetGroup;
		public bool NeedToSwitchBuildTarget => _needToSwitchBuildTarget;

		private ProjectInfoDataObject _projectInfoData;
		private BuildTarget _activeBuildTarget;
		private BuildTargetGroup _selectedBuildTargetGroup;
		private bool _needToSwitchBuildTarget;

		public BuildHandler(ProjectInfoDataObject projectInfoData)
		{
			_activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;

			_projectInfoData = projectInfoData;
			_selectedBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(_projectInfoData.TargetPlatform);

			_needToSwitchBuildTarget = _projectInfoData.TargetPlatform != _activeBuildTarget;
		}

		public void SwitchActiveBuildTarget()
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(_selectedBuildTargetGroup, _projectInfoData.TargetPlatform);
			_needToSwitchBuildTarget = false;
		}
	}
}