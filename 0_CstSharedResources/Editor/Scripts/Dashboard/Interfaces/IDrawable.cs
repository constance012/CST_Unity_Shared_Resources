namespace CSTGames.SharedResources.Editor.Dashboard.Interfaces
{
	public interface IDrawable
	{
		/// <summary>
		/// Called when the tab is first created or activated.
		/// </summary>
		void OnEnable() { }

		/// <summary>
		/// Called to render the tab's GUI.
		/// </summary>
		void Draw();

		/// <summary>
		/// Called when the tab is deactivated or destroyed.
		/// </summary>
		void OnDisable() { }
	}
}