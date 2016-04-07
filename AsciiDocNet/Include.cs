using System;

namespace AsciiDocNet
{
	public class Include : IElement
	{
		public Include(string path)
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

		public int? Indent { get; set; }

		public int? LevelOffset { get; set; }

		public string Lines { get; set; }

		public Container Parent { get; set; }

		public string Path { get; set; }

		public string Tags { get; set; }

		public static bool operator ==(Include left, Include right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Include left, Include right)
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
			return Equals((Include)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Indent.GetHashCode();
				hashCode = (hashCode * 397) ^ LevelOffset.GetHashCode();
				hashCode = (hashCode * 397) ^ (Lines?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Parent?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Path?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Tags?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		protected bool Equals(Include other)
		{
			return Indent == other.Indent &&
			       LevelOffset == other.LevelOffset &&
			       string.Equals(Lines, other.Lines) &&
			       string.Equals(Path, other.Path) &&
			       string.Equals(Tags, other.Tags);
		}
	}
}