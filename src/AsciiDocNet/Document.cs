using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsciiDocNet
{
	public class Document : Container
	{
		public Document() : this(null)
		{
		}

		public Document(string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				var directory = new FileInfo(source).Directory.FullName;
				Attributes.Add(new AttributeEntry("docdir", directory));
			}
			else
			{
				Attributes.Add(new AttributeEntry("docdir", Directory.GetCurrentDirectory()));
			}
		}

		public IList<AttributeEntry> Attributes { get; } = new List<AttributeEntry>();

		public AuthorInfo Author => Authors.FirstOrDefault();

		public IList<AuthorInfo> Authors { get; } = new List<AuthorInfo>();

		public DocType DocType { get; set; } = DocType.Article;

		public DocumentTitle Title { get; set; }

		public static Document Load(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return new Document();
			}

			using (var asciiReader = new DocumentReader(path))
			{
				var parser = new Parser();
				return parser.Process(asciiReader);
			}
		}

		public static Document Load(Stream stream)
		{
			if (stream == null || stream.Length == 0)
			{
				return new Document();
			}

			using (var reader = new StreamReader(stream))
			{
				using (var asciiReader = new DocumentReader(reader))
				{
					var parser = new Parser();
					return parser.Process(asciiReader);
				}
			}
		}

		public static bool operator ==(Document left, Document right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Document left, Document right)
		{
			return !Equals(left, right);
		}

		public static Document Parse(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return new Document();
			}

			using (var reader = new StringReader(text))
			{
				using (var asciiReader = new DocumentReader(reader))
				{
					var parser = new Parser();
					return parser.Process(asciiReader);
				}
			}
		}

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
			return obj.GetType() == this.GetType() && Equals((Document)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
				hashCode = (hashCode * 397) ^ Authors.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)DocType;
				hashCode = (hashCode * 397) ^ (Title?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

		protected bool Equals(Document other)
		{
			return base.Equals(other) &&
			       Equals(Attributes, other.Attributes) &&
			       Equals(Authors, other.Authors) &&
			       DocType == other.DocType &&
			       Equals(Title, other.Title);
		}
	}
}