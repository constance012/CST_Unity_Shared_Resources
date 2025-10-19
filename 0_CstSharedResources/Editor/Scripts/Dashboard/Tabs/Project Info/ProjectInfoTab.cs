using UnityEditor;

namespace CST.Shared.Resources.Editor.Dashboard.Tabs
{
	public class ProjectInfoTab : BaseDashboardTab
	{
		public override string TabName => "Project Info";

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public override void Draw()
		{
			EditorGUILayout.HelpBox("Project Info tab content goes here.", MessageType.Info);
		}
	}
}