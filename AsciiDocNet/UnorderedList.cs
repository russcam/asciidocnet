using System.Collections.Generic;

namespace AsciidocNet
{
	public class UnorderedList : IElement
	{
		public IList<UnorderedListItem> Items { get; } = new List<UnorderedListItem>();

		public Container Parent { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}