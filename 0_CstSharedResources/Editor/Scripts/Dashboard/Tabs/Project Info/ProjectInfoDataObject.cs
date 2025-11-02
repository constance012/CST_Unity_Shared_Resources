using System.IO;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo
{
	[CreateAssetMenu(fileName = "ProjectInfoDataObject", menuName = "CST Dashboard/Data Objects/Project Info")]
	public class ProjectInfoDataObject : ScriptableObject
	{
		[Header("General Info"), Space]
		public BuildTarget TargetPlatform = BuildTarget.StandaloneWindows64;

		[Space]
		public string ApplicationIdentifier;
		public string ProductName;
		public string CompanyName;

		[Space]
		public string Version;
		public int BuildNumber;

		public static ProjectInfoDataObject LoadOrCreateInstance()
		{
			if (!Directory.Exists(Constants.DATA_PATH))
			{
				Directory.CreateDirectory(Constants.DATA_PATH);
			}

			var dataObjectPath = Path.Combine(Constants.DATA_PATH, $"{nameof(ProjectInfoDataObject)}.asset");
			var loadedDataObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(dataObjectPath);

			if (loadedDataObject == null)
			{
				loadedDataObject = CreateInstance(typeof(ProjectInfoDataObject));
				AssetDatabase.CreateAsset(loadedDataObject, dataObjectPath);
				AssetDatabase.SaveAssets();
			}

			return loadedDataObject as ProjectInfoDataObject;
		}
	}
}