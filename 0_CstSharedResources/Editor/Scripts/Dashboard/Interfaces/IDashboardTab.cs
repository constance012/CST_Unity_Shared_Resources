namespace CST.Shared.Resources.Editor.Dashboard.Interfaces
{
	public interface IDashboardTab
	{
		string TabName { get; }

		/// <summary>
		/// Called when the tab is first created or activated.
		/// </summary>
		void OnEnable();

		/// <summary>
		/// Called to render the tab's GUI.
		/// </summary>
		void OnGUI();

		/// <summary>
		/// Called when the tab is deactivated or destroyed.
		/// </summary>
		void OnDisable();
	}
}