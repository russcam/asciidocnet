namespace AsciiDocNet
{
	// TODO: Should derive from AsciiSectionTitle and take a collection inline elements for title
	/// <summary>
	///     A document title
	/// </summary>
	/// <example>
	///     = This is a title
	/// </example>
	public class DocumentTitle : IAttributable
	{
		public DocumentTitle(string title) : this(title, null)
		{
		}

		public DocumentTitle(string title, string subtitle)
		{
			Title = title;
			Subtitle = subtitle;
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public string Subtitle { get; set; }

		public string Title { get; set; }

		public static bool operator ==(DocumentTitle left, DocumentTitle right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DocumentTitle left, DocumentTitle right)
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
			return Equals((DocumentTitle)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ (Subtitle?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(DocumentTitle other)
		{
			return Equals(Attributes, other.Attributes) &&
			       string.Equals(Subtitle, other.Subtitle) &&
			       string.Equals(Title, other.Title);
		}
	}
}