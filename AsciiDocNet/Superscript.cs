namespace AsciiDocNet
{
	public class Superscript : IInlineElement, IText, IAttributable
	{
		public Superscript(string text)
		{
			Text = text;
		}

		public Superscript()
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Text { get; set; }

		public static bool operator ==(Superscript left, Superscript right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Superscript left, Superscript right)
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
			return Equals((Superscript)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Text?.GetHashCode() ?? 0) * 397) ^ Attributes.GetHashCode();
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Superscript other)
		{
			return string.Equals(Text, other.Text) && Equals(Attributes, other.Attributes);
		}
	}
}