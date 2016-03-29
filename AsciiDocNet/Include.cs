using System;

namespace AsciidocNet
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

		public TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}