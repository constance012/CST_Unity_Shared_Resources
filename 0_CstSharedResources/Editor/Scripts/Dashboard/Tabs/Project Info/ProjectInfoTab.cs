using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo
{
	public class ProjectInfoTab : BaseDashboardTab
	{
		public override string TabName => "Project Info";
		public override int OrderNumber => 1;

		private ProjectInfoDataObject _dataObject;
		private SerializedObject _dataSerializedObject;
		private UnityEditor.Editor _dataObjectEditor;

		private ProjectInfoHandler _handler;

		public override void OnEnable()
		{
			base.OnEnable();
			LoadDataObject();
		}

		public override void Draw()
		{
			EditorGUILayout.BeginVertical(GUIStyleGetter.Get(GUIStyleType.BoxStyle));
			EditorGUILayout.Space(20f);

			if (_dataObjectEditor != null)
			{
				DrawDataObjectSection();
			}
			else
			{
				EditorGUILayout.HelpBox("Project Info tab content goes here.", MessageType.Error);
			}

			EditorGUILayout.EndVertical();
		}
		
		private void DrawDataObjectSection()
		{
			EditorGUI.BeginChangeCheck();

			_dataObjectEditor.OnInspectorGUI();
			EditorGUILayout.Space(20f);

			if (GUILayout.Button("APPLY CHANGES", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(50f)))
			{
				if (EditorGUI.EndChangeCheck())
				{
					SaveDataObject();
				}
				
				_handler.ApplyProjectInfo(_dataObject);
			}
		}

		private void LoadDataObject()
		{
			_dataObject = ProjectInfoDataObject.LoadOrCreateInstance();

			_dataSerializedObject = new SerializedObject(_dataObject);
			_dataObjectEditor = UnityEditor.Editor.CreateEditor(_dataObject);

			_handler = new ProjectInfoHandler();
		}

		private void SaveDataObject()
		{
			_dataSerializedObject.ApplyModifiedProperties();

			EditorUtility.SetDirty(_dataObject);
			AssetDatabase.SaveAssetIfDirty(_dataObject);
			
			_dataSerializedObject.Update();
		}
	}
}