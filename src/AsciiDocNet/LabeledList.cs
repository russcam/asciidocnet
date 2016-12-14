using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class LabeledList : IElement
	{
		public IList<LabeledListItem> Items { get; } = new List<LabeledListItem>();

		public Container Parent { get; set; }

		public static bool operator ==(LabeledList left, LabeledList right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LabeledList left, LabeledList right)
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
			return Equals((LabeledList)obj);
		}

		public override int GetHashCode()
		{
			return Items.GetHashCode();
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(LabeledList other)
		{
			return Items.SequenceEqual(other.Items);
		}
	}
}