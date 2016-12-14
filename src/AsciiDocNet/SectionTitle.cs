using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
	public class SectionTitle : InlineContainer, IElement, IAttributable
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
			Elements = elements.ToList();
			_level = level;
		}

		public SectionTitle(IInlineElement element, int level)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			ValidateLevel(level);
			Add(element);
			_level = level;
		}

		public SectionTitle(string text, int level) : this(new TextLiteral(text), level)
		{
		}

		public bool IsDiscrete
		{
			get { return Attributes.IsDiscrete; }
			set { Attributes.IsDiscrete = value; }
		}

		public bool IsFloating
		{
			get { return Attributes.IsFloating; }
			set { Attributes.IsFloating = value; }
		}

		public AttributeList Attributes { get; } = new AttributeList();

		public override InlineElementType ContainElementType { get; } = InlineElementType.All;

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

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
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

		public static bool operator ==(SectionTitle left, SectionTitle right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(SectionTitle left, SectionTitle right)
		{
			return !Equals(left, right);
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