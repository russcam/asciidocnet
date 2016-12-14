namespace AsciiDocNet
{
	// TODO: text cannot be null?
	public class AttributeReference : IInlineElement, IText
	{
		public AttributeReference(string text)
		{
			Text = text;
		}

		public string Text { get; set; }

		public static bool operator ==(AttributeReference left, AttributeReference right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AttributeReference left, AttributeReference right)
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
			return Equals((AttributeReference)obj);
		}

		public override int GetHashCode()
		{
			return Text?.GetHashCode() ?? 0;
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(AttributeReference other)
		{
			return string.Equals(Text, other.Text);
		}
	}
}