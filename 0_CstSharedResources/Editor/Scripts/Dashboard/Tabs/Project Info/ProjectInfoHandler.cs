using UnityEditor;
using UnityEditor.Build;

namespace CST.Shared.Resources.Editor.Dashboard.Tabs.ProjectInfo
{
	public class ProjectInfoHandler
	{
		public void ApplyProjectInfo(ProjectInfoDataObject dataObject)
		{
			var activeBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(activeBuildTargetGroup);

			PlayerSettings.SetApplicationIdentifier(namedBuildTarget, dataObject.ApplicationIdentifier);
			PlayerSettings.productName = dataObject.ProductName;
			PlayerSettings.companyName = dataObject.CompanyName;

			PlayerSettings.bundleVersion = dataObject.Version;
			ApplyBuildNumber(activeBuildTargetGroup, dataObject.BuildNumber);

			AssetDatabase.SaveAssets();
		}

		private void ApplyBuildNumber(BuildTargetGroup buildTarget, int buildNumber)
		{
			switch (buildTarget)
			{
				case BuildTargetGroup.Android:
					PlayerSettings.Android.bundleVersionCode = buildNumber;
					break;

				case BuildTargetGroup.iOS:
					PlayerSettings.iOS.buildNumber = buildNumber.ToString();
					break;
			}
		}
	}
}