using System;

namespace AsciiDocNet
{
	// TODO: Id must be a valid value. Check in Id setter
	public class Anchor
	{
		public Anchor(string id) : this(id, null)
		{
		}

		public Anchor(string id, string xRefLabel)
		{
			Id = id;
			XRefLabel = xRefLabel;
		}

		public string Id { get; set; }

		public string XRefLabel { get; set; }

		public static bool operator ==(Anchor left, Anchor right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Anchor left, Anchor right)
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
			return Equals((Anchor)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Id?.GetHashCode() ?? 0) * 397) ^ (XRefLabel?.GetHashCode() ?? 0);
			}
		}

		protected bool Equals(Anchor other)
		{
			return string.Equals(Id, other.Id) && 
				string.Equals(XRefLabel, other.XRefLabel);
		}
	}
}