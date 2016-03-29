namespace AsciidocNet
{
	// TODO: Should derive from AsciiSectionTitle and take a collection inline elements for title
	/// <summary>
	///     A document title
	/// </summary>
	/// <example>
	///     = This is a title
	/// </example>
	public class DocumentTitle : IAttributable
	{
		public DocumentTitle(string title) : this(title, null)
		{
		}

		public DocumentTitle(string title, string subtitle)
		{
			Title = title;
			Subtitle = subtitle;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Subtitle { get; set; }

		public string Title { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}