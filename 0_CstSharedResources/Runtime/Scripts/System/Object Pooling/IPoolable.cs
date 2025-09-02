namespace CST.Shared.Resources
{
	public interface IPoolable
	{
		void Allocate();
		void Deallocate();
	}
}