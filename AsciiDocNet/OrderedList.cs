using System.Collections.Generic;

namespace AsciiDocNet
{
	public class OrderedList : IElement
	{
		public IList<OrderedListItem> Items { get; } = new List<OrderedListItem>();

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public Container Parent { get; set; }
	}
}