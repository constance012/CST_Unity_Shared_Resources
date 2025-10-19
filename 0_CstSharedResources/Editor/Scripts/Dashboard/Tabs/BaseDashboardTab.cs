using CST.Shared.Resources.Editor.Dashboard.Interfaces;
using CST.Shared.Resources.Editor.Dashboard.Utilities;
using UnityEngine;

namespace CST.Shared.Resources.Editor.Dashboard.Tabs
{
	public abstract class BaseDashboardTab : IDrawable
	{
		public abstract string TabName { get; }

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