using System;

namespace CSTGames.SharedResources.Editor.Dashboard.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class HaveScriptableDataObjectAttribute : Attribute
	{
		public Type ScriptableDataType { get; }
		public string AssetName { get; }

		/// <summary>
		/// Attribute to indicate that a class requires its own ScriptableObject data asset.
		/// </summary>
		/// <param name="scriptableDataType"></param>
		/// <param name="assetName"></param>
		public HaveScriptableDataObjectAttribute(Type scriptableDataType, string assetName)
		{
			ScriptableDataType = scriptableDataType;
			AssetName = assetName;
		}
	}
}