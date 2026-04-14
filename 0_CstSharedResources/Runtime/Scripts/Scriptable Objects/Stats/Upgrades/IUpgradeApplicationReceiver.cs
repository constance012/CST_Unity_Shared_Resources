using System;

namespace CSTGames.SharedResources
{
	public interface IUpgradeApplicationReceiver
	{
		void OnUpgradeApplied(Type type, UpgradeBase upgrade);
	}
}