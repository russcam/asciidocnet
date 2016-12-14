using System;

namespace AsciiDocNet
{
	public abstract class Media : IElement, IAttributable
	{
		protected Media(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("must specify a path", nameof(path));
			}

			Path = path;
		}

		public string Align { get; set; }

		public string AlternateText { get; set; }

		public AttributeList Attributes { get; } = new AttributeList();

		public string Float { get; set; }

		public int? Height { get; private set; }

		public string Link { get; set; }

		public Container Parent { get; set; }

		public string Path { get; set; }

		public string Role { get; set; }

		public string Title { get; set; }

		public int? Width { get; private set; }

		public static bool operator ==(Media left, Media right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Media left, Media right)
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
			return Equals((Media)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Align?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (AlternateText?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Attributes?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Float?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ Height.GetHashCode();
				hashCode = (hashCode * 397) ^ (Link?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Path?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Role?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ Width.GetHashCode();
				return hashCode;
			}
		}

		public void SetWidthAndHeight(int width, int height)
		{
			Width = width;
			Height = height;
		}

		protected bool Equals(Media other)
		{
			return string.Equals(Align, other.Align) &&
			       string.Equals(AlternateText, other.AlternateText) &&
			       Equals(Attributes, other.Attributes) &&
			       string.Equals(Float, other.Float) &&
			       Height == other.Height &&
			       Width == other.Width &&
			       string.Equals(Link, other.Link) &&
			       string.Equals(Path, other.Path) &&
			       string.Equals(Role, other.Role) &&
			       string.Equals(Title, other.Title);
		}
	}
}