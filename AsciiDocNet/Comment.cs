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

		public Container Parent { get; set; }

		public CommentStyle Style { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Comment left, Comment right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Comment left, Comment right)
		{
			return !Equals(left, right);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((Comment)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Style;
				hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Comment other)
		{
			return Equals(Attributes, other.Attributes) && 
				Style == other.Style && 
				string.Equals(Text, other.Text);
		}
	}
}