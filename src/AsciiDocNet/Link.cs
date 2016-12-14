namespace AsciiDocNet
{
	public class Link : IInlineElement, IAttributable
	{
		public Link(string href, string text)
		{
			Href = href;
			Text = text;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Href { get; set; }

		public string Text { get; set; }

		public static bool operator ==(Link left, Link right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Link left, Link right)
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
			return Equals((Link)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ (Href?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Text?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Link other)
		{
			return Equals(Attributes, other.Attributes) && 
				string.Equals(Href, other.Href) && 
				string.Equals(Text, other.Text);
		}
	}
}