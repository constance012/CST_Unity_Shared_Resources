using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSTGames.SharedResources.Editor.Dashboard.Attributes;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo
{
	public class ProjectInfoTab : BaseDashboardTab
	{
		public override string TabName => "Project Info";
		public override int OrderNumber => 1;

		private const string UNCATEGORIZED_GROUP_NAME = "Uncategorized";

		private ProjectInfoDataObject _dataObject;
		private SerializedObject _dataSerializedObject;
		private Dictionary<string, List<SerializedProperty>> _categorizedProperties;
		private List<SerializedProperty> _uncategorizedProperties;

		private ProjectInfoHandler _handler;
		private Vector2 _scrollPosition;

		public override void OnEnable()
		{
			base.OnEnable();
			LoadDataObject();
			InitializePropertyGroups();
		}

		#region Drawing Methods.
		public override void Draw()
		{
			if (GUILayout.Button("APPLY CHANGES", GUIStyleGetter.Get(GUIStyleType.YellowButtonStyle), GUILayout.MinHeight(50f)))
			{
				EditorUtility.SetDirty(_dataObject);
				AssetDatabase.SaveAssetIfDirty(_dataObject);

				_handler.ApplyProjectInfo(_dataObject);
			}

			EditorGUILayout.Space(10f);

			if (_categorizedProperties != null)
			{
				_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
				{
					DrawDataObjectSection();
				}
				EditorGUILayout.EndScrollView();
			}
			else
			{
				EditorGUILayout.HelpBox($"Unable to parse properties from the associated {typeof(ProjectInfoDataObject).Name} scriptable object.\n" +
					"Check the Console for more details.", MessageType.Error);
			}
		}

		private void DrawDataObjectSection()
		{
			_dataSerializedObject.Update();

			foreach (var propertyGroup in _categorizedProperties)
			{
				EditorGUILayout.LabelField(propertyGroup.Key, _subHeaderStyle);
				EditorGUILayout.BeginVertical(_boxStyle);
				{
					foreach (var property in propertyGroup.Value)
					{
						EditorGUILayout.PropertyField(property, new GUIContent(property.displayName));
					}
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space(10f);
			}

			if (_uncategorizedProperties.Count > 0)
			{
				EditorGUILayout.LabelField(UNCATEGORIZED_GROUP_NAME, _subHeaderStyle);
				EditorGUILayout.BeginVertical(_boxStyle);
				{
					foreach (var property in _uncategorizedProperties)
					{
						EditorGUILayout.PropertyField(property, new GUIContent(property.displayName));
					}
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space(10f);
			}

			_dataSerializedObject.ApplyModifiedProperties();
		}
		#endregion

		#region Initialization Methods
		private void InitializePropertyGroups()
		{
			var groupSortOrders = new Dictionary<string, int>();

			var fields = _dataSerializedObject.targetObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			string previousHeader = string.Empty;

			foreach (var field in fields)
			{
				var property = _dataSerializedObject.FindProperty(field.Name);
				var headerGroupAttribute = field.GetCustomAttribute<HeaderGroupAttribute>();

				string currentHeader = headerGroupAttribute?.HeaderName;
				int? orderNumber = headerGroupAttribute?.Order;

				if (string.IsNullOrEmpty(previousHeader) && string.IsNullOrEmpty(currentHeader))
				{
					_uncategorizedProperties.Add(property);
					continue;
				}

				if (!string.IsNullOrEmpty(currentHeader) && currentHeader != previousHeader)
				{
					previousHeader = currentHeader;
				}

				TryAddToPropertyGroup(previousHeader, property);

				if (orderNumber.HasValue)
				{
					groupSortOrders[previousHeader] = orderNumber.Value;
				}
			}

			_categorizedProperties = _categorizedProperties
				.OrderBy(group => groupSortOrders[group.Key])
				.ToDictionary(group => group.Key, group => group.Value);
		}
		
		private void TryAddToPropertyGroup(string headerName, SerializedProperty property)
		{
			if (!_categorizedProperties.TryGetValue(headerName, out var propertyGroup))
			{
				propertyGroup = new List<SerializedProperty>();
				_categorizedProperties[headerName] = propertyGroup;
			}

			propertyGroup.Add(property);
		}

		private void LoadDataObject()
		{
			_dataObject = ProjectInfoDataObject.LoadOrCreateInstance();

			_dataSerializedObject = new SerializedObject(_dataObject);

			_categorizedProperties = new Dictionary<string, List<SerializedProperty>>();
			_uncategorizedProperties = new List<SerializedProperty>();

			_handler = new ProjectInfoHandler();
		}
		#endregion
	}
}