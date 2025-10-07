using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

namespace CST.Shared.Resources
{
	[CreateAssetMenu(menuName = "Unit Stats/Stats", fileName = "New Blank Stats")]
	public class Stats : ScriptableObject
	{
		[Header("Stats"), Space]
		[Tooltip("Dynamic stats are GLOBAL stats shared between objects, which CAN be modified by upgrades.")]
		public SerializedDictionary<StatType, float> dynamicStats = new SerializedDictionary<StatType, float>();

		[Tooltip("Static stats are GLOBAL stats shared between objects, which CAN NOT be modified by upgrades.")]
		public SerializedDictionary<StatType, float> staticStats = new SerializedDictionary<StatType, float>();

		// Private fields.
		private readonly HashSet<StatsUpgrade> _appliedUpgrades = new HashSet<StatsUpgrade>();
		private readonly HashSet<StatType> _toStringIgnoreStats = new HashSet<StatType>()
	{
		StatType.InvincibilityTime,
		StatType.ProjectileSpeed,
		StatType.ProjectileLifeTime,
		StatType.ProjectileTrackingRigidity,
	};

		public void AddUpgrade(StatsUpgrade upgrade)
		{
			if (!_appliedUpgrades.Contains(upgrade))
				_appliedUpgrades.Add(upgrade);
		}

		public void RemoveUpgrade(StatsUpgrade upgrade)
		{
			_appliedUpgrades.Remove(upgrade);
		}

		public void ClearUpgrades()
		{
			_appliedUpgrades.Clear();
		}

		public float GetStaticStat(StatType statName)
		{
			if (staticStats.TryGetValue(statName, out float value))
				return value;
			else
			{
				string statString = statName.ToString().AddWhitespaceBeforeCapital();
				Debug.LogWarning($"No STATIC stat value found for \"{statString}\" on {this.name}");
				return -1f;
			}
		}

		public float GetDynamicStat(StatType statName)
		{
			if (dynamicStats.TryGetValue(statName, out float baseValue))
				return GetUpgradedValue(statName, baseValue);
			else
			{
				string statString = statName.ToString().AddWhitespaceBeforeCapital();
				Debug.LogWarning($"No DYNAMIC stat value found for \"{statString}\" on {this.name}");
				return -1f;
			}
		}

		public void ModifyStat(StatType statName, float delta)
		{
			if (dynamicStats.TryGetValue(statName, out float _))
			{
				dynamicStats[statName] += delta;
			}
			else
			{
				Debug.LogError($"No DYNAMIC stat value found for {statName} on {this.name}");
			}
		}

		private float GetUpgradedValue(StatType stat, float baseValue)
		{
			foreach (StatsUpgrade upgrade in _appliedUpgrades)
			{
				if (!upgrade.affectedStats.TryGetValue(stat, out float upgradeValue))
					continue;

				if (upgrade.type == UpgradeValueType.Percentage)
					baseValue *= 1f + upgradeValue;
				else
					baseValue += upgradeValue;
			}

			return baseValue;
		}

		public override string ToString()
		{
			string result = "";
			foreach (KeyValuePair<StatType, float> stat in dynamicStats)
			{
				if (!_toStringIgnoreStats.Contains(stat.Key))
					result += $"{stat.Key.ToString().AddWhitespaceBeforeCapital()}: {stat.Value}\n";
			}
			result += "\n";
			foreach (KeyValuePair<StatType, float> stat in staticStats)
			{
				if (!_toStringIgnoreStats.Contains(stat.Key))
					result += $"{stat.Key.ToString().AddWhitespaceBeforeCapital()}: {stat.Value}\n";
			}
			return result.TrimEnd('\r', '\n');
		}
	}
}