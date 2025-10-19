using UnityEditor;

namespace CST.Shared.Resources.Editor.Dashboard.Tabs
{
	public class UtilitiesTab : BaseDashboardTab
	{
		public override string TabName => "Utilities";

		public override void OnEnable()
		{
			base.OnEnable();
		}

		public override void Draw()
		{
			EditorGUILayout.HelpBox("Utilities tab content goes here.", MessageType.Info);
		}
	}
}