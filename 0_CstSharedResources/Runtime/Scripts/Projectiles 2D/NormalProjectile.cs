using UnityEngine;

namespace CST.Shared.Resources
{
	public sealed class NormalProjectile : ProjectileBase
	{
		public override void ProcessCollision(Collider2D other)
		{
			DamageTarget(other, 1f);
			Deallocate();
		}
	}
}
