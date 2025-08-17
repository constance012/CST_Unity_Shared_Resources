using System;

namespace CST.Shared.Resources
{
	public interface IUpgradeApplicationReceiver
	{
		void OnUpgradeApplied(Type type, UpgradeBase upgrade);
	}
}