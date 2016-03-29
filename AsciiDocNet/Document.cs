using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsciiDocNet
{
	public class Document : Container
	{
		public Document()
		{
			Attributes.Add(new AttributeEntry("docdir", Directory.GetCurrentDirectory()));
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

		public AuthorInfo Author => Authors?.FirstOrDefault();

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
	}
}