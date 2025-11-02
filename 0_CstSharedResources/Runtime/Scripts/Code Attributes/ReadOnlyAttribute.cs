using System;
using UnityEngine;

namespace CSTGames.SharedResources
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute
	{

	}
}