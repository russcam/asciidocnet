namespace AsciiDocNet
{
	public class Subscript : IInlineElement, IText, IAttributable
	{
		protected bool Equals(Subscript other)
		{
			return string.Equals(Text, other.Text) && Equals(Attributes, other.Attributes);
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
			return Equals((Subscript)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Text?.GetHashCode() ?? 0) * 397) ^ Attributes.GetHashCode();
			}
		}

		public static bool operator ==(Subscript left, Subscript right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Subscript left, Subscript right)
		{
			return !Equals(left, right);
		}

		public Subscript(string text)
		{
			Text = text;
		}

		public Subscript()
		{
		}

		public string Text { get; set; }

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public AttributeList Attributes { get; } = new AttributeList();
	}
}