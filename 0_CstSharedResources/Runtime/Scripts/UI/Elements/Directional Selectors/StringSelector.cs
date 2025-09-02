namespace CST.Shared.Resources
{
	public sealed class StringSelector : DirectionalSelector<string>
	{
		protected override void SetDisplayText()
		{
			selectedText.text = _selected;
		}
	}
}