namespace CST.Shared.Resources
{
	public interface IHealable
	{
		bool CanBeHealed { get; }
		void Heal(int amount);
	}
}