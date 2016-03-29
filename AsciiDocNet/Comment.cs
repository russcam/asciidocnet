using System;
using System.Collections.Generic;

namespace AsciiDocNet
{
	public class Comment : IElement, IText, IAttributable
	{
		public Comment(string text)
		{
			Text = text;
			Style = CommentStyle.SingleLine;
		}

		public Comment(IEnumerable<string> text)
		{
			Text = string.Join(Environment.NewLine, text);
			Style = CommentStyle.MultiLine;
		}

		public Comment()
		{
			Style = CommentStyle.MultiLine;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public CommentStyle Style { get; set; }

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public Container Parent { get; set; }
	}
}