using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CSTGames.SharedResources.Editor.Dashboard.Utilities
{
	public static class TypeUtils
	{
		private static readonly Dictionary<Type, IEnumerable<Type>> _derivedTypeCaches = new();

		public static IEnumerable<Type> FindAllTypesDerivedFromInterface<TInterface>()
		{
			var interfaceType = typeof(TInterface);

			if (_derivedTypeCaches.TryGetValue(interfaceType, out var derivedTypes))
			{
				Debug.Log($"[TypeUtils] All derived types from interface {interfaceType.Name} are already found. Returning from caches.");
				return derivedTypes;
			}

			Debug.Log($"[TypeUtils] Finding all derived types from interface {interfaceType.Name}...");

			derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

			_derivedTypeCaches[interfaceType] = derivedTypes;

			return derivedTypes;
		}
	}
}