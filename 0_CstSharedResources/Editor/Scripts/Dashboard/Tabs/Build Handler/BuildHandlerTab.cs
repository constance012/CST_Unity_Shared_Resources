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
		private IBuildPlatformDrawable _platformDrawer;

		public override void OnEnable()
		{
			base.OnEnable();
			Initialize();
			ConstructBuildPlatformDrawer();
		}

		#region Drawing Methods.
		public override void Draw()
		{
			if (EditorApplication.isCompiling || EditorApplication.isUpdating)
			{
				DrawCompilingOrImportingWarning();
				return;
			}
			
			if (_buildHandler.NeedToSwitchBuildTarget)
			{
				DrawSwitchBuildTargetWarning();
			}
			else
			{
				DrawHelperButtons();
				DrawBuildSummarizationSection();
				DrawPlatformSpecificSection();
				DrawMainBuildSection();
			}
		}

		private void DrawCompilingOrImportingWarning()
		{
			EditorGUILayout.HelpBox
			(
				"The Unity Editor is currently compiling scripts or importing assets.\n" +
				"Please wait until the process is finished.",
				MessageType.Warning
			);
			EditorGUILayout.Space(10f);
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

		private void DrawHelperButtons()
		{
			EditorGUILayout.LabelField("Helper Buttons", _subHeaderStyle);

			GUILayout.BeginVertical(_boxStyle);
			{
				if (GUILayout.Button("Open Player Settings", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(30f)))
				{
					SettingsService.OpenProjectSettings("Project/Player");
				}
				if (GUILayout.Button("Open Build Profile", GUIStyleGetter.Get(GUIStyleType.CyanButtonStyle), GUILayout.MinHeight(30f)))
				{
					EditorApplication.ExecuteMenuItem("File/Build Profiles");
				}
				if (GUILayout.Button("Open Previous Build Folder", GUIStyleGetter.Get(GUIStyleType.NormalButtonStyle), GUILayout.MinHeight(30f)))
				{
					string buildFolderPath = PlayerPrefsRuntimeHelper.GetValue(GlobalDefines.PLAYER_PREFS_PREVIOUS_BUILD_PATH_KEY, string.Empty);
					EditorUtility.RevealInFinder(buildFolderPath);
				}
			}
			GUILayout.EndVertical();
			
			EditorGUILayout.Space(10f);
		}

		private void DrawBuildSummarizationSection()
		{
			EditorGUILayout.LabelField("Build Summarization", _subHeaderStyle);
			_buildInfoDrawer.Draw();
			EditorGUILayout.Space(10f);
		}

		private void DrawPlatformSpecificSection()
		{
			EditorGUILayout.LabelField($"{_platformDrawer.BuildTargetDisplayName} Build Options", _subHeaderStyle);
			
			GUILayout.BeginVertical(_boxStyle);
			{
				_platformDrawer.Draw();
			}
			GUILayout.EndVertical();

			EditorGUILayout.Space(10f);
		}

		private void DrawMainBuildSection()
		{
			EditorGUILayout.LabelField($"Main Build Controls", _subHeaderStyle);

			GUILayout.BeginVertical(_boxStyle);
			{
				_buildHandler.DrawMainBuildSection();
			}
			GUILayout.EndVertical();
		}
		#endregion

		#region Initialization Methods
		private void Initialize()
		{
			_projectInfoData = ProjectInfoDataObject.LoadOrCreateInstance();

			_buildInfoDrawer = new BuildInfoDrawer(_projectInfoData);
			_buildInfoDrawer.OnEnable();

			_buildHandler ??= new BuildHandler();
		}

		private void ConstructBuildPlatformDrawer()
		{
			switch (_projectInfoData.TargetPlatform)
			{
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
#if UNITY_STANDALONE_WIN
					_platformDrawer ??= new WindowsPlatformDrawer();
#endif
					break;
				
				case BuildTarget.StandaloneLinux64:
				case BuildTarget.EmbeddedLinux:
				case BuildTarget.LinuxHeadlessSimulation:
#if UNITY_STANDALONE_LINUX
					_platformDrawer ??= new LinuxPlatformDrawer();
#endif
					break;

				case BuildTarget.StandaloneOSX:
#if UNITY_STANDALONE_OSX
					_platformDrawer ??= new MacPlatformDrawer();
#endif
					break;

				case BuildTarget.WebGL:
#if UNITY_WEBGL
					_platformDrawer ??= new WebPlatformDrawer();
#endif
					break;
				
				default:
					_platformDrawer ??= new UnknownPlatformDrawer();
					break;
			}

			_platformDrawer.Initialize(_projectInfoData);
			_buildHandler.Initialize(_projectInfoData, _platformDrawer);
		}
		#endregion
	}
}