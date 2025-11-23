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

		public static IEnumerable<TInterface> FindAllInstancesOfInterface<TInterface>()
		{
			var derivedTypes = FindAllTypesDerivedFromInterface<TInterface>();
			var instances = new HashSet<TInterface>();

			foreach (var type in derivedTypes)
			{
				try
				{
					var tabInstance = (TInterface)Activator.CreateInstance(type);
					instances.Add(tabInstance);
				}
				catch (Exception e)
				{
					Debug.LogError($"[Type-Utils] FAILED to create instance of type \"{type.Name}\" implementing the \"{typeof(TInterface).Name}\" interface.\n\n" +
						$"Reason: {e.Message}.\n" +
						$"Stack Trace: {e.StackTrace}");
				}
			}

			return instances;
		}
	}
}