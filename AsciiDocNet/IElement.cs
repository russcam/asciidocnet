namespace AsciidocNet
{
	public interface IElement
	{
		Container Parent { get; set; }

		TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor;
	}
}