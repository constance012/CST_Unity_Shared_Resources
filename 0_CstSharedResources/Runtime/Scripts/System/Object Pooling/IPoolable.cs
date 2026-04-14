namespace CSTGames.SharedResources
{
	public interface IPoolable
	{
		void Allocate();
		void Deallocate();
	}
}