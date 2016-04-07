using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class SectionTitle : IContainerInlineElement, IElement, IAttributable
	{
		private int _level;

		public SectionTitle(IList<IInlineElement> elements, int level)
		{
			if (elements == null)
			{
				throw new ArgumentNullException(nameof(elements));
			}
			if (!elements.Any())
			{
				throw new ArgumentException("must have at least one element", nameof(elements));
			}
			ValidateLevel(level);
			Elements = elements;
			_level = level;
		}

		public SectionTitle(IInlineElement element, int level)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			ValidateLevel(level);
			Elements.Add(element);
			_level = level;
		}

		public SectionTitle(string text, int level) : this(new TextLiteral(text), level)
		{
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public InlineElementType ContainElementType { get; } = InlineElementType.All;

		public IList<IInlineElement> Elements { get; } = new List<IInlineElement>();

		public int Level
		{
			get { return _level; }
			set
			{
				ValidateLevel(value);
				_level = value;
			}
		}

		public Container Parent { get; set; }

		public static bool operator ==(SectionTitle left, SectionTitle right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(SectionTitle left, SectionTitle right)
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
			return Equals((SectionTitle)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = _level;
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Elements.GetHashCode();
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(SectionTitle other)
		{
			return _level == other._level &&
			       Attributes.Equals(other.Attributes) &&
			       Elements.SequenceEqual(other.Elements);
		}

		private static void ValidateLevel(int level)
		{
			if (level < 1 || level > 6)
			{
				throw new ArgumentOutOfRangeException(nameof(level), "must be between 1 and 6");
			}
		}
	}
}