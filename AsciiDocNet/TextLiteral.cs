namespace AsciiDocNet
{
	public class TextLiteral : IInlineElement, IAttributable, IText
	{
		public TextLiteral(string text)
		{
			Text = text;
		}

		public TextLiteral()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Text { get; set; }

		public static bool operator ==(TextLiteral left, TextLiteral right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(TextLiteral left, TextLiteral right)
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
			return Equals((TextLiteral)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Attributes.GetHashCode() * 397) ^ (Text?.GetHashCode() ?? 0);
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(TextLiteral other)
		{
			return Equals(Attributes, other.Attributes) && 
				string.Equals(Text, other.Text);
		}

		public override string ToString() => Text;
	}
}