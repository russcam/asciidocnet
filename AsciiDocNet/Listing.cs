using System.Collections.Generic;
using System.Linq;

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

		public static bool operator ==(Listing left, Listing right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Listing left, Listing right)
		{
			return !Equals(left, right);
		}

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
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
			return Equals((Listing)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Attributes != null ? Attributes.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Callouts != null ? Callouts.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
				return hashCode;
			}
		}

		protected bool Equals(Listing other)
		{
			return Equals(Attributes, other.Attributes) &&
			       Callouts.SequenceEqual(other.Callouts) &&
			       string.Equals(Text, other.Text);
		}
	}
}