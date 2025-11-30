using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class BuildHandlerTab : BaseDashboardTab
	{
		public override string TabName => "Build Handler";
		public override int OrderNumber => 2;

		private ProjectInfoDataObject _projectInfoData;
		private BuildHandler _buildHandler;
		private IDrawable _buildInfoDrawer;

		public override void OnEnable()
		{
			base.OnEnable();
			Initialize();
		}

		#region Drawing Methods.
		public override void Draw()
		{
			if (_buildHandler.NeedToSwitchBuildTarget)
			{
				DrawSwitchBuildTargetWarning();
			}
			else
			{
				DrawBuildInfoSection();
			}
		}

		private void DrawSwitchBuildTargetWarning()
		{
			if (_buildHandler.SelectedBuildTargetGroup == BuildTargetGroup.Unknown)
			{
				EditorGUILayout.HelpBox
				(
					$"The current active build platform is \"{_buildHandler.ActiveBuildTarget}\".\n" +
					$"The Project Info settings are configured for the \"{_projectInfoData.TargetPlatform}\" platform, which belongs to an UNKNOWN build target group.\n" +
					"Switching build target is NOT possible.",
					MessageType.Error
				);

				return;
			}

			EditorGUILayout.HelpBox
			(
				$"The current active build platform is \"{_buildHandler.ActiveBuildTarget}\".\n" +
				$"The Project Info settings are configured for the \"{_projectInfoData.TargetPlatform}\" platform of the \"{_buildHandler.SelectedBuildTargetGroup}\" group.\n" +
				"Switching build target is MANDATORY to match the currently selected platform.",
				MessageType.Warning
			);

			EditorGUILayout.Space(10f);

			if (GUILayout.Button($"SWITCH TO THE SELECTED PLATFORM".ToUpper(),
				GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle),
				GUILayout.MinHeight(50f)))
			{
				_buildHandler.SwitchActiveBuildTarget();
			}
		}

		private void DrawBuildInfoSection()
		{
			_buildInfoDrawer.Draw();
		}
		#endregion

		#region Initialization Methods
		private void Initialize()
		{
			_projectInfoData = ProjectInfoDataObject.LoadOrCreateInstance();
			_buildHandler = new BuildHandler(_projectInfoData);

			_buildInfoDrawer = new BuildInfoDrawer(_projectInfoData);
			_buildInfoDrawer.OnEnable();
		}
		#endregion
	}
}