using System.Collections.Generic;

namespace AsciidocNet
{
	public class LabeledList : IElement
	{
		public IList<LabeledListItem> Items { get; } = new List<LabeledListItem>();

		public Container Parent { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}