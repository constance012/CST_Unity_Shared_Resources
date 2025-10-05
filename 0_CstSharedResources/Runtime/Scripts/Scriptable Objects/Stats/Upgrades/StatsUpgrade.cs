using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace CST.Shared.Resources
{
	[CreateAssetMenu(menuName = "Upgrades/Stats Upgrade", fileName = "New Stats Upgrade")]
	public class StatsUpgrade : GenericUpgradeBase<Stats>
	{
		[Header("Detail"), Space]
		public SerializedDictionary<StatType, float> affectedStats = new SerializedDictionary<StatType, float>();

		public override void DoUpgrade()
		{
			if (!IsApplied)
			{
				Debug.Log($"Applying \"{this.displayName}\" upgrade...");
				unitsToApply.ForEach(unit => unit.AddUpgrade(this));
				IsApplied = true;
			}
		}

		public override void RemoveUpgrade()
		{
			if (IsApplied)
			{
				Debug.Log($"Removing \"{this.displayName}\" upgrade...");
				unitsToApply.ForEach(unit => unit.RemoveUpgrade(this));
				IsApplied = false;
			}
		}
	}
}