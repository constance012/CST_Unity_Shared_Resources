using System.Collections.Generic;
using CST.Shared.Resources.Editor.Dashboard.Interfaces;
using CST.Shared.Resources.Editor.Dashboard.Tabs;
using CST.Shared.Resources.Editor.Dashboard.Tabs.BuildHandler;
using CST.Shared.Resources.Editor.Dashboard.Tabs.ProjectInfo;
using CST.Shared.Resources.Editor.Dashboard.Tabs.Utilities;
using UnityEditor;
using UnityEngine;

namespace CST.Shared.Resources.Editor.Dashboard
{
	public class CSTDashboard : EditorWindow
	{
		public static CSTDashboard Instance { get; private set; }
		public bool IsInitialized { get; private set; }

		private IDrawable _title;
		private List<BaseDashboardTab> _tabs;
		private int _selectedTabIndex;

		[MenuItem("CST Games/Dashboard %#d")]
		public static void ShowWindow()
		{
			var dashboard = GetWindow<CSTDashboard>("CST Dashboard");

			dashboard.Show();
			dashboard.Focus();
		}

		private void OnEnable()
		{
			Instance = this;

			UnsubscribeEvents();
			SubscribeEvents();
		}

		private void OnDisable()
		{
			UnsubscribeEvents();

			if (_tabs != null && _selectedTabIndex < _tabs.Count)
			{
				_tabs[_selectedTabIndex].OnDisable();
			}
		}

		private void OnGUI()
		{
			EnsureInitialization();

			_title.Draw();

			if (_tabs == null || _tabs.Count == 0)
			{
				EditorGUILayout.HelpBox("No tabs available to draw", MessageType.Warning);
				return;
			}

			DrawToolbar();
			EditorGUILayout.Space(10f);

			_tabs[_selectedTabIndex].Draw();
		}

		#region Event Subscriptions
		private void SubscribeEvents()
		{
			AssemblyReloadEvents.afterAssemblyReload += AssemblyReloadEvents_AfterAssemblyReload;
		}

		private void UnsubscribeEvents()
		{
			AssemblyReloadEvents.afterAssemblyReload -= AssemblyReloadEvents_AfterAssemblyReload;
		}

		private void AssemblyReloadEvents_AfterAssemblyReload()
		{
			IsInitialized = false;
			_tabs = null;
		}
		#endregion

		#region Initialization Methods
		private void EnsureInitialization()
		{
			if (!IsInitialized)
			{
				InitTabs();
				InitGUI();

				Repaint();
				IsInitialized = true;
			}
		}

		private void InitTabs()
		{
			_tabs ??= new List<BaseDashboardTab>()
			{
				new ProjectInfoTab(),
				new BuildHandlerTab(),
				new UtilitiesTab(),
			};

			if (_tabs.Count > 0)
			{
				_tabs[_selectedTabIndex].OnEnable();
			}
		}

		private void InitGUI()
		{
			_title ??= new DashboardTitleDrawer();
			_title.OnEnable();
		}
		#endregion

		#region Drawing Methods.
		private void DrawToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			{
				int newIndex = GUILayout.Toolbar(_selectedTabIndex, GetAllTabNames(), EditorStyles.toolbarButton);

				if (newIndex != _selectedTabIndex)
				{
					_tabs[_selectedTabIndex].OnDisable();

					_selectedTabIndex = newIndex;

					_tabs[_selectedTabIndex].OnEnable();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion

		#region Helper Methods.
		private string[] GetAllTabNames()
		{
			string[] names = new string[_tabs.Count];

			for (int i = 0; i < _tabs.Count; i++)
			{
				names[i] = _tabs[i].TabName;
			}

			return names;
		}
		#endregion
	}
}