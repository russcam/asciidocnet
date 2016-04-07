using System;

namespace AsciiDocNet
{
	public class Literal : IElement, IAttributable, IText
	{
		public Literal(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			if (text.Length == 0)
			{
				throw new ArgumentException("must be at least one character", nameof(text));
			}

			Text = text;
		}

		public Literal()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public Container Parent { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Literal left, Literal right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Literal left, Literal right)
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
			return Equals((Literal)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Attributes?.GetHashCode() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Literal other)
		{
			return Equals(Attributes, other.Attributes) && string.Equals(Text, other.Text);
		}
	}
}