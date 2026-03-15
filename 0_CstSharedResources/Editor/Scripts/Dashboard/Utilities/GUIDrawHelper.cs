using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Utilities
{
	public static class GUIDrawHelper
	{
		public static void EnumPopupWithLabel<TEnum>(string label, ref TEnum enumValue, params GUILayoutOption[] options)
			where TEnum : System.Enum
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label, EditorStyles.label);
				enumValue = (TEnum)EditorGUILayout.EnumPopup(enumValue, options);
			}
			GUILayout.EndHorizontal();
		}

		public static void EnumFlagsFieldWithLabel<TEnum>(string label, ref TEnum enumValue, params GUILayoutOption[] options)
			where TEnum : System.Enum
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label, EditorStyles.label);
				enumValue = (TEnum)EditorGUILayout.EnumFlagsField(enumValue, options);
			}
			GUILayout.EndHorizontal();
		}

		public static void ToggleWithLabel(string label, ref bool toggleValue, params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label, EditorStyles.label);
				toggleValue = EditorGUILayout.Toggle(toggleValue, options);
			}
			GUILayout.EndHorizontal();
		}
	}
}