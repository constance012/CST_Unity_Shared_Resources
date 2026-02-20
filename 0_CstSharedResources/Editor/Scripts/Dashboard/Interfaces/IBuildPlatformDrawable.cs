using UnityEditor;

namespace CSTGames.SharedResources.Editor.Dashboard.Interfaces
{
	public interface IBuildPlatformDrawable : IDrawable
	{
		public BuildTarget BuildTarget { get; }
		public string BuildTargetDisplayName { get; }

		public void SetupBuildParameters(string buildPath);
		public string GetFileExtension();
	}
}