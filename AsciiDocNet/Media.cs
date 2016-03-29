using System;

namespace AsciidocNet
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

		public string Path { get; set; }

		public string Role { get; set; }

		public string Title { get; set; }

		public int? Width { get; private set; }

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public void SetWidthAndHeight(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public Container Parent { get; set; }
	}
}