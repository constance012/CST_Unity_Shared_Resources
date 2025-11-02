using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.Utilities
{
	public class UtilitiesTab : BaseDashboardTab
	{
		public override string TabName => "Utilities";
		public override int OrderNumber => 3;

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