using CSTGames.SharedResources.Editor.Dashboard.Interfaces;
using CSTGames.SharedResources.Editor.Dashboard.Tabs.ProjectInfo;
using CSTGames.SharedResources.Editor.Dashboard.Utilities;
using UnityEditor;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Tabs.BuildHandler
{
	public class WebPlatformDrawer : IBuildPlatformDrawable
	{
		public BuildTarget BuildTarget => BuildTarget.WebGL;
		public string BuildTargetDisplayName => "WebGL";

		private ProjectInfoDataObject _projectInfoData;
		private WebGLCompressionFormat _resourcesCompressionFormat = WebGLCompressionFormat.Disabled;
		private WebGLTextureSubtarget _textureCompressionFormat = WebGLTextureSubtarget.ASTC;
		private WebGLClientBrowserType _clientBrowserType;
		private UnityEditor.WebGL.WasmCodeOptimization _codeOptimizationType;

		public void Initialize(ProjectInfoDataObject projectInfoData)
		{
			_projectInfoData = projectInfoData;
		}

		public void Draw()
		{
			GUIDrawHelper.EnumPopupWithLabel("Resources Compression Format: ", ref _resourcesCompressionFormat, GUILayout.MinWidth(300f));
			GUIDrawHelper.EnumPopupWithLabel("Texture Compression Format: ", ref _textureCompressionFormat, GUILayout.MinWidth(300f));

			GUILayout.Space(10f);
			GUIDrawHelper.EnumPopupWithLabel("Client Browser Type: ", ref _clientBrowserType, GUILayout.MinWidth(300f));

			GUILayout.Space(10f);
			GUIDrawHelper.EnumPopupWithLabel("Code Optimization Type: ", ref _codeOptimizationType, GUILayout.MinWidth(300f));
		}

		public void SetupBuildParameters(string buildPath)
		{
			PlayerSettings.WebGL.compressionFormat = _resourcesCompressionFormat;

			EditorUserBuildSettings.webGLBuildSubtarget = _textureCompressionFormat;
			EditorUserBuildSettings.webGLClientBrowserType = _clientBrowserType;

			UnityEditor.WebGL.UserBuildSettings.codeOptimization = _codeOptimizationType;
		}

		public string GetFileExtension()
		{
			return string.Empty;
		}
	}
}