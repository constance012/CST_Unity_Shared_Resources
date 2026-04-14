using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs
{
	public abstract class BaseDashboardTab : IDrawable, IDashboardTab
	{
		public abstract string TabName { get; }
		public abstract int OrderNumber { get; }

		protected GUIStyle _subHeaderStyle;
		protected GUIStyle _boxStyle;

		public BaseDashboardTab()
		{
			_subHeaderStyle = GUIStyleGetter.Get(GUIStyleType.SubHeaderStyle);
			_boxStyle = GUIStyleGetter.Get(GUIStyleType.BoxStyle);
		}

		public virtual void OnEnable() { }

		public virtual void OnDisable() { }

		public abstract void Draw();
	}
}