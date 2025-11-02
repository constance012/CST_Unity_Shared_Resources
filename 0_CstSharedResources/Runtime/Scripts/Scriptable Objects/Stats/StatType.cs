namespace CSTGames.SharedResources
{
	public enum StatType
	{
		// Dynamic stats.

		/// <summary>
		/// Maximum health of the character.
		/// </summary>
		MaxHealth = 0,

		/// <summary>
		/// How much damage does the character deal per hit?
		/// </summary>
		Damage = 1,

		/// <summary>
		/// How many attacks can the character do per second?
		/// </summary>
		AttackSpeed = 2,

		/// <summary>
		/// How fast does the character move? In unit/s
		/// </summary>
		MoveSpeed = 3,
		
		/// <summary>
		/// How quick does the character change its moving direction? In deg/s
		/// </summary>
		TurnAngleDegree = 10,

		/// <summary>
		/// For how long is the character invincible after taking damage? In seconds.
		/// </summary>
		InvincibilityTime = 4,

		// Static stats.
		/// <summary>
		/// How strong is the knockback force applied to other objects when this character hits them?
		/// </summary>
		KnockBackStrength = 5,

		/// <summary>
		/// How strong does the character resist knockback forces? In scale of 0-1.
		/// </summary>
		KnockBackResistant = 6,

		/// <summary>
		/// How fast do projectiles fired by this character move? In unit/s
		/// </summary>
		ProjectileSpeed = 7,

		/// <summary>
		/// How quickly do projectiles adjust their direction towards the target? Higher values means more rigid tracking.
		/// </summary>
		ProjectileTrackingRigidity = 8,

		/// <summary>
		/// How long do projectiles fired by this character last before disappearing? In seconds.
		/// </summary>
		ProjectileLifeTime = 9
	}
}