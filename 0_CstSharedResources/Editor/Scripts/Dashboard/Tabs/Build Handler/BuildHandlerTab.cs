using UnityEditor;

namespace CST.Shared.Resources.Editor.Dashboard.Tabs
{
	public class BuildHandlerTab : BaseDashboardTab
	{
		public override string TabName => "Build Handler";

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public override void Draw()
		{
			EditorGUILayout.HelpBox("Build Handler tab content goes here.", MessageType.Info);
		}
	}
}