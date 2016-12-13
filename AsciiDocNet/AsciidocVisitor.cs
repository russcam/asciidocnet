using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AsciiDocNet
{
	/// <summary>
	///     Visits a <see cref="Document" /> and writes out an AsciiDoc to
	///     a file / text writer.
	/// </summary>
	public class AsciiDocVisitor : IDocumentVisitor, IDisposable
	{
		private readonly TextWriter _writer;
		private bool _disposed;

		public AsciiDocVisitor(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("must specify a path", nameof(path));
			}

			_writer = new StreamWriter(path);
		}

		public AsciiDocVisitor(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException(nameof(writer));
			}
			_writer = writer;
		}

		public virtual void Visit(Document document)
		{
			if (document == null)
			{
				return;
			}

			foreach (var attribute in document.Attributes)
			{
				attribute.Accept(this);
			}

			Visit(document.Title);
			Visit(document.Authors);

			Visit((Container)document);
		}

		public virtual void Visit(AuthorInfo author)
		{
			if (!string.IsNullOrEmpty(author.Email))
			{
				_writer.Write("{0}{1}{2} <{3}>",
					!string.IsNullOrEmpty(author.FirstName) ? author.MiddleName.Replace(" ", "_") + " " : string.Empty,
					!string.IsNullOrEmpty(author.MiddleName) ? author.MiddleName.Replace(" ", "_") + " " : string.Empty,
					!string.IsNullOrEmpty(author.LastName) ? author.LastName.Replace(" ", "_") + " " : string.Empty,
					author.Email);
			}
			else
			{
				_writer.Write("{0}{1}{2}",
					!string.IsNullOrEmpty(author.FirstName) ? author.MiddleName.Replace(" ", "_") + " " : string.Empty,
					!string.IsNullOrEmpty(author.MiddleName) ? author.MiddleName.Replace(" ", "_") + " " : string.Empty,
					!string.IsNullOrEmpty(author.LastName) ? author.LastName.Replace(" ", "_") + " " : string.Empty);
			}
		}

		public virtual void Visit(IList<AuthorInfo> authors)
		{
			if (authors == null || !authors.Any())
			{
				return;
			}

			for (int i = 0; i < authors.Count; i++)
			{
				var lastAuthor = i == authors.Count - 1;
				var author = authors[i];
				author.Accept(this);

				if (i > 0 && !lastAuthor)
				{
					_writer.Write(";");
				}

				if (lastAuthor)
				{
					_writer.WriteLine();
					_writer.WriteLine();
				}
			}
		}

		public virtual void Visit(OrderedList list)
		{
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

		public virtual void Visit(Paragraph paragraph)
		{
			if (paragraph == null)
			{
				return;
			}

			Visit(paragraph.Attributes);
			Visit((InlineContainer)paragraph);
			_writer.WriteLine();

			if (paragraph.Count > 0)
			{
				var lastElement = paragraph[paragraph.Count - 1];
				if (!lastElement.ToString().EndsWith("\r\n"))
				{
					_writer.WriteLine();
				}
			}
		}

		public virtual void Visit(Source source)
		{
			Visit(source.Attributes);
			_writer.WriteLine(Patterns.Block.Source);
			_writer.WriteLine(source.Text);
			_writer.WriteLine(Patterns.Block.Source);
			foreach (var callout in source.Callouts)
			{
				Visit(callout);
			}
			_writer.WriteLine();
		}

		public virtual void Visit(Attribute attribute)
		{
			_writer.Write(attribute.Name);
		}

		public virtual void Visit(NamedAttribute attribute)
		{
			_writer.Write("{0}={2}{1}{2}", attribute.Name, attribute.Value, attribute.SingleQuoted ? "'" : "\"");
		}

		public virtual void Visit(AttributeList attributes)
		{
			if (attributes == null)
			{
				return;
			}

			if (attributes.HasAnchor)
			{
				attributes.Anchor.Accept(this);
			}

			if (attributes.Any())
			{
				_writer.Write("[");
				for (int index = 0; index < attributes.Count; index++)
				{
					var lastAttribute = index == attributes.Count - 1;
					var asciiAttribute = attributes[index];
					asciiAttribute.Accept(this);

					if (!lastAttribute)
					{
						_writer.Write(",");
					}
				}
				_writer.WriteLine("]");
			}

			if (attributes.HasTitle)
			{
				attributes.Title.Accept(this);
			}
		}

		public virtual void Visit(AttributeEntry attributeEntry)
		{
			if (!string.IsNullOrEmpty(attributeEntry.Value))
			{
				_writer.WriteLine(":{0}: {1}", attributeEntry.Name, attributeEntry.Value);
			}
			else
			{
				_writer.WriteLine(":{0}:", attributeEntry.Name);
			}
			_writer.WriteLine();
		}

		public virtual void Visit(UnorderedList list)
		{
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

		public virtual void Visit(TextLiteral text)
		{
			_writer.Write(text.Text);
		}

		public virtual void Visit(Strong strong)
		{
			if (strong == null)
			{
				return;
			}
			_writer.Write(strong.DoubleDelimited ? "**" : "*");
			Visit((InlineContainer)strong);
			_writer.Write(strong.DoubleDelimited ? "**" : "*");
		}

		public virtual void Visit(Emphasis emphasis)
		{
			if (emphasis == null)
			{
				return;
			}
			_writer.Write(emphasis.DoubleDelimited ? "__{0}__" : "_{0}_", emphasis.Text);
		}

		public virtual void Visit(Quotation quotation)
		{
			if (quotation == null) return;

			_writer.Write(quotation.DoubleDelimited ? "\"`" : "'`");
			Visit((InlineContainer)quotation);
			_writer.Write(quotation.DoubleDelimited ? "`\"" : "`'");
		}

		public virtual void Visit(Quote quote)
		{
			Visit(quote.Attributes);
			var isBlock = quote.Count > 1;

			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Quote);
			}
			Visit((Container)quote);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Quote);
				_writer.WriteLine();
			}
		}

		public virtual void Visit(SectionTitle sectionTitle)
		{
			if (sectionTitle.IsDiscrete)
			{
				_writer.WriteLine("[discrete]");
			}
			else if (sectionTitle.IsFloating)
			{
				_writer.WriteLine("[float]");
			}
			Visit(sectionTitle.Attributes);
			_writer.Write("{0} ", new string('=', sectionTitle.Level));
			Visit((InlineContainer)sectionTitle);
			_writer.WriteLine();
			_writer.WriteLine();
		}

		public virtual void Visit(UnorderedListItem listItem)
		{
			_writer.Write("{0} ", new string('*', listItem.Level));
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}
		}

		public virtual void Visit(Monospace monospace)
		{
			_writer.Write(monospace.DoubleDelimited ? "``" : "`");
			Visit((InlineContainer)monospace);
			_writer.Write(monospace.DoubleDelimited ? "``" : "`");
		}

		public virtual void Visit(UnsetAttributeEntry attributeEntry)
		{
			_writer.WriteLine(":{0}!:", attributeEntry.Name);
			_writer.WriteLine();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Visit(Open open)
		{
			Visit(open.Attributes);
			_writer.WriteLine(Patterns.Block.Open);
			Visit((Container)open);
			_writer.WriteLine(Patterns.Block.Open);
			_writer.WriteLine();
		}

		public virtual void Visit(Callout callout)
		{
			_writer.WriteLine("<{0}> {1}", callout.Number, callout.Text);
		}

		public virtual void Visit(Literal literal)
		{
			Visit(literal.Attributes);
			_writer.WriteLine(Patterns.Block.Literal);
			_writer.WriteLine(literal.Text);
			_writer.WriteLine(Patterns.Block.Literal);
			_writer.WriteLine();
		}

		public virtual void Visit(Title title)
		{
			_writer.WriteLine(".{0}", title.Text);
		}

		public virtual void Visit(CheckListItem listItem)
		{
			_writer.Write("{0} [{1}] ", new string('-', listItem.Level), listItem.Checked? "x" : " ");
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}
		}

		public virtual void Visit(OrderedListItem listItem)
		{
			if (listItem.Number.HasValue)
			{
				switch (listItem.Numbering)
				{
					case NumberStyle.LowerAlpha:
						_writer.Write("{0}. ", Patterns.LowerAlphabet[listItem.Number.Value - 1]);
						break;
					case NumberStyle.UpperAlpha:
						_writer.Write("{0}. ", Patterns.UpperAlphabet[listItem.Number.Value - 1]);
						break;
					case NumberStyle.LowerRoman:
						_writer.Write("{0}. ", RomanNumerals.ToNumeral(listItem.Number.Value).ToLowerInvariant());
						break;
					case NumberStyle.UpperRoman:
						_writer.Write("{0}. ", RomanNumerals.ToNumeral(listItem.Number.Value));
						break;
					case NumberStyle.Decimal:
					case NumberStyle.LowerGreek:
					case NumberStyle.Arabic:
					default:
						_writer.Write("{0}. ", listItem.Number.Value);
						break;
				}
			}
			else
			{
				_writer.Write("{0} ", new string('.', listItem.Level));
			}

			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}
		}

		public virtual void Visit(LabeledListItem listItem)
		{
			Visit(listItem.Attributes);
			_writer.WriteLine("{0}{1}", listItem.Label, new string(':', listItem.Level + 2));
			_writer.WriteLine();
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}		
		}

		public virtual void Visit(LabeledList list)
		{
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

		public virtual void Visit(Link link)
		{
			if (link == null)
			{
				return;
			}

			_writer.Write("{0}[{1}]", link.Href, link.Text);
		}

		public virtual void Visit(Media media)
		{
			media.Accept(this);
		}

		public virtual void Visit(Container elements)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				var element = elements[i];
				element.Accept(this);
			}
		}

		public virtual void Visit(InlineContainer elements)
		{
			for (int index = 0; index < elements.Count; index++)
			{
				var element = elements[index];
				element.Accept(this);
			}
		}

		public virtual void Visit(Image image)
		{
			VisitMedia(image, "image");
		}

		public virtual void Visit(Video video)
		{
			VisitMedia(video, "video");
		}

		public virtual void Visit(Audio audio)
		{
			VisitMedia(audio, "audio");
		}

		public virtual void Visit(Include include)
		{
			if (include == null)
			{
				return;
			}

			var attributes = new StringBuilder();
			if (include.LevelOffset.HasValue)
			{
				attributes.Append($"leveloffset={include.LevelOffset}");
			}
			if (!string.IsNullOrEmpty(include.Lines))
			{
				attributes.Append($"lines=\"{include.Lines}\"");
			}
			if (!string.IsNullOrEmpty(include.Tags))
			{
				attributes.Append($"tags=\"{include.Tags}\"");
			}
			if (include.Indent.HasValue)
			{
				attributes.Append($"indent={include.Indent}");
			}

			_writer.WriteLine("include::{0}[{1}]", include.Path, attributes);
			_writer.WriteLine();
		}

		public virtual void Visit(Listing listing)
		{
			Visit(listing.Attributes);

			_writer.WriteLine(Patterns.Block.Listing);
			_writer.WriteLine(listing.Text);
			_writer.WriteLine(Patterns.Block.Listing);
			foreach (var callout in listing.Callouts)
			{
				Visit(callout);
			}

			_writer.WriteLine();
		}

		public virtual void Visit(Example example)
		{
			Visit(example.Attributes);
			var isBlock = example.Count > 1;

			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Example);
			}
			Visit((Container)example);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Example);
				_writer.WriteLine();
			}
		}

		public virtual void Visit(Comment comment)
		{
			if (comment.Style == CommentStyle.MultiLine)
			{
				_writer.WriteLine(Patterns.Block.Comment);
				_writer.WriteLine(comment.Text);
				_writer.WriteLine(Patterns.Block.Comment);
				_writer.WriteLine();
			}
			else
			{
				_writer.WriteLine("//{0}", comment.Text);
			}
		}

		public virtual void Visit(AttributeReference reference)
		{
			_writer.Write("{{{0}}}", reference.Text);
		}

		public virtual void Visit(Fenced fenced)
		{
			Visit(fenced.Attributes);
			_writer.WriteLine(Patterns.Block.Fenced);
			_writer.WriteLine(fenced.Text);
			_writer.WriteLine(Patterns.Block.Fenced);
			_writer.WriteLine();
		}

		public virtual void Visit(Pass pass)
		{
			Visit(pass.Attributes);
			_writer.WriteLine(Patterns.Block.Pass);
			_writer.WriteLine(pass.Text);
			_writer.WriteLine(Patterns.Block.Pass);
			_writer.WriteLine();
		}

		public virtual void Visit(Sidebar sidebar)
		{
			Visit(sidebar.Attributes);
			_writer.WriteLine(Patterns.Block.Sidebar);
			Visit((Container)sidebar);
			_writer.WriteLine(Patterns.Block.Sidebar);
			_writer.WriteLine();
		}

		public virtual void Visit(Table table)
		{
			throw new NotImplementedException("TODO Visit Table");
		}

		public virtual void Visit(DocumentTitle title)
		{
			if (title == null)
			{
				return;
			}

			Visit(title.Attributes);
			if (!string.IsNullOrEmpty(title.Subtitle))
			{
				_writer.WriteLine("= {0}: {1}", title.Title, title.Subtitle);
			}
			else
			{
				_writer.WriteLine("= {0}", title.Title);
			}

			_writer.WriteLine();
		}

		public virtual void Visit(InternalAnchor anchor)
		{
			if (!string.IsNullOrEmpty(anchor.XRefLabel))
			{
				_writer.Write("<<{0},{1}>>", anchor.Id, anchor.XRefLabel);
			}
			else
			{
				_writer.Write("<<{0}>>", anchor.Id);
			}
		}

		public virtual void Visit(InlineAnchor anchor)
		{
			if (!string.IsNullOrEmpty(anchor.XRefLabel))
			{
				_writer.Write("[[{0},{1}]]", anchor.Id, anchor.XRefLabel);
			}
			else
			{
				_writer.Write("[[{0}]]", anchor.Id);
			}
		}

		public virtual void Visit(Admonition admonition)
		{
			// TODO: Handle admonition block
			_writer.Write("{0}: ", admonition.Style.ToString().ToUpperInvariant());
			Visit((Container)admonition);
		}

		public virtual void Visit(Anchor anchor)
		{
			if (!string.IsNullOrEmpty(anchor.XRefLabel))
			{
				_writer.WriteLine("[[{0},{1}]]", anchor.Id, anchor.XRefLabel);
			}
			else
			{
				_writer.WriteLine("[[{0}]]", anchor.Id);
			}
		}

		public virtual void Visit(Stem stem)
		{
			if (stem == null)
			{
				return;
			}

			Visit(stem.Attributes);
			_writer.WriteLine(Patterns.Block.Stem);
			_writer.WriteLine(stem.Text);
			_writer.WriteLine(Patterns.Block.Stem);
			_writer.WriteLine();
		}

		public virtual void Visit(Verse verse)
		{
			if (verse == null)
			{
				return;
			}

			Visit(verse.Attributes);
			var isBlock = verse.Count > 1;

			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Verse);
			}
			Visit((Container)verse);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Verse);
				_writer.WriteLine();
			}
		}

		public virtual void Visit(Mark mark)
		{
			if (mark == null)
			{
				return;
			}

			var roleAttribute = mark.Attributes["role"] as RoleAttribute;
			if (roleAttribute != null)
			{
				_writer.Write("[{0}]", roleAttribute.Value);
			}

			_writer.Write(mark.DoubleDelimited ? "##" : "#");
			Visit((InlineContainer)mark);
			_writer.Write(mark.DoubleDelimited ? "##" : "#");
		}

		public virtual void Visit(Subscript subscript)
		{
			_writer.Write("~{0}~", subscript.Text);
		}

		public virtual void Visit(Superscript superscript)
		{
			_writer.Write("^{0}^", superscript.Text);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_writer?.Dispose();
				}

				_disposed = true;
			}
		}

		private void VisitMedia(Media media, string name)
		{
			if (media == null)
			{
				return;
			}

			Visit(media.Attributes);
			var attributes = new StringBuilder();
			if (!string.IsNullOrEmpty(media.AlternateText))
			{
				attributes.Append(media.AlternateText);

				if (media.Width.HasValue)
				{
					attributes.Append("," + media.Width);
				}
				if (media.Height.HasValue)
				{
					attributes.Append("," + media.Height);
				}
			}
			if (!string.IsNullOrEmpty(media.Title))
			{
				attributes.Append($"title=\"{media.Title}\"");
			}
			if (!string.IsNullOrEmpty(media.Align))
			{
				attributes.Append($"align=\"{media.Align}\"");
			}
			if (!string.IsNullOrEmpty(media.Link))
			{
				attributes.Append($"link=\"{media.Link}\"");
			}
			if (!string.IsNullOrEmpty(media.Float))
			{
				attributes.Append($"float=\"{media.Float}\"");
			}
			if (!string.IsNullOrEmpty(media.Role))
			{
				attributes.Append($"role=\"{media.Role}\"");
			}

			_writer.WriteLine("{0}::{1}[{2}]", name, media.Path, attributes);
			_writer.WriteLine();
		}
	}
}