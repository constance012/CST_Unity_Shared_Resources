using System.Text;
using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class BuildInfoDrawer : IDrawable
	{
		private ProjectInfoDataObject _projectInfoData;
		private StringBuilder _stringBuilder;
		private string _buildInfoString;

		public BuildInfoDrawer(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;

			ConstructBuildInfoString();
		}

		public void Draw()
		{
			EditorGUILayout.HelpBox(_buildInfoString, MessageType.Info);
		}

		private void ConstructBuildInfoString()
		{
			_stringBuilder = new StringBuilder();

			_stringBuilder.AppendLine($"          {_projectInfoData.ProductName.ToUpper()}          ");
			_stringBuilder.AppendLine();
			_stringBuilder.AppendLine("========== GENERAL INFORMATION ==========");
			_stringBuilder.AppendLine($"Application Identifier: {_projectInfoData.ApplicationIdentifier}");
			_stringBuilder.AppendLine($"Product Name: {_projectInfoData.ProductName}");
			_stringBuilder.AppendLine($"Company Name: {_projectInfoData.CompanyName}");
			_stringBuilder.AppendLine();
			_stringBuilder.AppendLine("========== VERSIONS ==========");
			_stringBuilder.AppendLine($"Version: {_projectInfoData.Version}");
			_stringBuilder.AppendLine($"Build Number: {_projectInfoData.BuildNumber}");

			_buildInfoString = _stringBuilder.ToString();
		}
	}
}