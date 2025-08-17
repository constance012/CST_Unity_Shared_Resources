using UnityEngine;

namespace CST.Shared.Resources
{
	public interface IVisitor
	{
		void Visit<T>(T visitable) where T : Component, IVisitable;
	}
}