namespace AsciidocNet
{
	/// <summary>
	/// An inline element
	/// </summary>
	public interface IInlineElement
	{
		TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;
	}
}