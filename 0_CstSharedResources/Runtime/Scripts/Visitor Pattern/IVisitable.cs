namespace CSTGames.SharedResources
{
	public interface IVisitable
	{
		void Accept(IVisitor visitor);
	}
}