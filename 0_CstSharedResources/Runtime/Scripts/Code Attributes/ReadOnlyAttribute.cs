using System;
using UnityEngine;

namespace CST.Shared.Resources
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute
	{

	}
}