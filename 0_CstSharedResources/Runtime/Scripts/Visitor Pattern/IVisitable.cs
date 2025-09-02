namespace CST.Shared.Resources
{
	public interface IVisitable
	{
		void Accept(IVisitor visitor);
	}
}