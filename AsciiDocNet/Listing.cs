using System.Collections.Generic;

namespace AsciiDocNet
{
	public class Listing : IElement, IText, IAttributable
	{
		public Listing()
		{
		}

		public Listing(string text)
		{
			Text = text;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public IList<Callout> Callouts { get; } = new List<Callout>();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}