using System;

namespace CSTGames.SharedResources.Editor.Dashboard.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class HeaderGroupAttribute : Attribute
	{
		public string HeaderName { get; }
		public int Order { get; }

		/// <summary>
		/// Attribute to indicate the beginning of a new header group.
		/// </summary>
		/// <param name="headerName"> The name of the group. </param>
		/// <param name="order"> The sorting order used to sort against other groups. Default is -1, which is unsorted. </param>
		public HeaderGroupAttribute(string headerName, int order = -1)
		{
			HeaderName = headerName;
			Order = order;
		}
	}
}