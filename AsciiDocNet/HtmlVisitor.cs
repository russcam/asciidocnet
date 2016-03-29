using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsciidocNet
{
	public class HtmlVisitor : IDocumentVisitor, IDisposable
	{
		// keep the collection of attribute entries as we see them in the document
		private readonly Dictionary<string, AttributeEntry> _attributeEntries = new Dictionary<string, AttributeEntry>();

		private readonly TextWriter _writer;
		private bool _disposed;

		private int _figureNumber = 1;

		private bool _isBlock = true;

		public HtmlVisitor(TextWriter writer)
		{
			_writer = writer;
		}

		public virtual void Visit(Admonition admonition)
		{
		}

		public virtual void Visit(Anchor anchor)
		{
		}

		public virtual void Visit(Attribute attribute)
		{
		}

		public virtual void Visit(AttributeEntry attributeEntry)
		{
			_attributeEntries[attributeEntry.Name] = attributeEntry;
		}

		public virtual void Visit(AttributeList attributes)
		{
		}

		public virtual void Visit(AttributeReference reference)
		{
			AttributeEntry attributeEntry;
			if (!_attributeEntries.TryGetValue(reference.Text, out attributeEntry))
			{
				// TODO: Log that attribute is not found
				_writer.Write("{{{0}}}", reference.Text);
			}
			else
			{
				if (attributeEntry is UnsetAttributeEntry)
				{
					// TODO: do nothing for the moment. Should probably introduce IsSet property on AttributeEntry as opposed to having a specific type for Unset...
				}

				_writer.Write(attributeEntry.Value ?? string.Empty);
			}
		}

		public virtual void Visit(Audio audio)
		{
			throw new NotImplementedException("TODO: Html audio");
		}

		public virtual void Visit(AuthorInfo author)
		{
			throw new NotImplementedException("TODO: Html author");
		}

		public virtual void Visit(IList<AuthorInfo> authors)
		{
			throw new NotImplementedException("TODO: Html authors");
		}

		public virtual void Visit(Strong strong)
		{
			if (strong == null)
			{
				return;
			}

			_writer.Write("<strong>");
			Visit(strong.Elements);
			_writer.Write("</strong>");
		}

		public virtual void Visit(CheckListItem listItem)
		{
		}

		public virtual void Visit(Document document)
		{
			_writer.WriteLine("<!doctype html>");
			_writer.WriteLine("<html>");

			_writer.WriteLine("<head>");

			foreach (var attribute in document.Attributes)
			{
				Visit(attribute);
			}

			Visit(document.Title);

			_writer.WriteLine("</head>");

			_writer.WriteLine("<body class=\"article\">");

			Visit((Container)document);

			_writer.WriteLine("</body>");
			_writer.WriteLine("</html>");
		}

		public virtual void Visit(Mark mark)
		{
			string classes = null;
			foreach (var role in mark.Attributes.OfType<RoleAttribute>())
			{
				classes = string.Join(" ", role.Values);
			}

			if (classes != null)
			{
				_writer.Write("<span class=\"{0}\">", classes);
				Visit(mark.Elements);
				_writer.Write("</span>");
			}
			else
			{
				_writer.Write("<mark>");
				Visit(mark.Elements);
				_writer.Write("</mark>");
			}
		}

		public virtual void Visit(Container elements)
		{
			foreach (var element in elements)
			{
				element.Accept(this);
			}
		}

		public virtual void Visit(IList<IInlineElement> inlineElements)
		{
			foreach (var element in inlineElements)
			{
				element.Accept(this);
			}
		}

		public virtual void Visit(Image image)
		{
			_writer.WriteLine("<div class=\"imageblock\">");
			_writer.WriteLine("<div class=\"content\">");

			if (!string.IsNullOrEmpty(image.Link))
			{
				_writer.WriteLine("<a class=\"image\" href=\"{0}\">", image.Link);
			}

			_writer.WriteLine("<img src=\"{0}\"{1}{2}{3} />",
				image.Path,
				!string.IsNullOrEmpty(image.AlternateText) ? " alt=\"" + image.AlternateText + "\"" : string.Empty,
				image.Width.HasValue ? " width=\"" + image.Width.Value + "\"" : string.Empty,
				image.Height.HasValue ? " height=\"" + image.Height.Value + "\"" : string.Empty);

			if (!string.IsNullOrEmpty(image.Link))
			{
				_writer.WriteLine("</a>");
			}

			_writer.WriteLine("</div>");

			if (!string.IsNullOrEmpty(image.Title))
			{
				_writer.WriteLine("<div class=\"title\">Figure {0}: {1}</div>", _figureNumber, image.Title);
				++_figureNumber;
			}

			_writer.WriteLine("</div>");
		}

		public virtual void Visit(Include include)
		{
			throw new NotImplementedException("TODO: write out include");
		}

		public virtual void Visit(Emphasis emphasis)
		{
			_writer.Write("<em>{0}</em>", emphasis.Text);
		}

		public virtual void Visit(LabeledListItem listItem)
		{
		}

		public virtual void Visit(LabeledList list)
		{
		}

		public virtual void Visit(Link link)
		{
			_writer.Write("<a href=\"{0}\">{1}</a>", link.Href, link.Text);
		}

		public virtual void Visit(Listing listing)
		{
		}

		public virtual void Visit(Callout callout)
		{
		}

		public virtual void Visit(UnorderedListItem listItem)
		{

			if (listItem.Count == 1)
			{
				_writer.Write("<li>");
				_isBlock = false;
				Visit((Container)listItem);
				_isBlock = true;
				_writer.WriteLine("</li>");
			}
			else
			{
				_writer.WriteLine("<li>");
				Visit((Container)listItem);
				_writer.WriteLine("</li>");
			}
		}

		public virtual void Visit(UnorderedList list)
		{
			_writer.WriteLine("<ul>");
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
			_writer.WriteLine("</ul>");
		}

		public virtual void Visit(TextLiteral text)
		{
			_writer.Write(text.Text);
		}

		public virtual void Visit(Literal literal)
		{
		}

		public virtual void Visit(Media media)
		{
			media.Accept(this);
		}

		public virtual void Visit(Monospace monospace)
		{
			if (monospace == null)
			{
				return;
			}

			_writer.Write("<code>");
			Visit(monospace.Elements);
			_writer.Write("</code>");
		}

		public virtual void Visit(NamedAttribute attribute)
		{
		}

		public virtual void Visit(Open open)
		{
			_writer.WriteLine("<div class=\"openblock\">");
			_writer.WriteLine("<div class=\"content\">");
			Visit((Container)open);
			_writer.WriteLine("</div>");
			_writer.WriteLine("</div>");
		}

		public virtual void Visit(OrderedListItem listItem)
		{
		}

		public virtual void Visit(OrderedList list)
		{

		}

		public virtual void Visit(Paragraph paragraph)
		{
			if (_isBlock)
			{
				_writer.WriteLine("<div class=\"paragraph\">");
				_writer.Write("<p>");

				Visit(paragraph.Elements);

				_writer.WriteLine("</p>");
				_writer.WriteLine("</div>");
			}
			else
			{
				_writer.Write("<p>");
				Visit(paragraph.Elements);
				_writer.Write("</p>");
			}
		}

		public virtual void Visit(Quotation quotation)
		{
			if (quotation == null)
			{
				return;
			}

			_writer.Write(quotation.DoubleDelimited ? "“" : "‘");
			Visit(quotation.Elements);
			_writer.Write(quotation.DoubleDelimited ? "”" : "’");
		}

		public virtual void Visit(Quote quote)
		{
		}

		public virtual void Visit(SectionTitle sectionTitle)
		{
		}

		public virtual void Visit(Source source)
		{
			_writer.WriteLine("<div class=\"listingblock\">");
			_writer.WriteLine("<div class=\"content\">");

			_writer.WriteLine("<pre class=\"highlight\"><code class=\"language-{0} hljs\" data-lang=\"{0}\">{1}</code></pre>",
				source.Attributes[1].Name,
				HtmlEncode(source.Text));

			_writer.WriteLine("</div>");
			_writer.WriteLine("</div>");
		}

		public virtual void Visit(Title title)
		{
		}

		public virtual void Visit(UnsetAttributeEntry attributeEntry)
		{
		}

		public virtual void Visit(Video video)
		{
		}

		public virtual void Visit(Example example)
		{
		}

		public virtual void Visit(Comment comment)
		{
		}

		public virtual void Visit(Fenced fenced)
		{
		}

		public virtual void Visit(Pass pass)
		{
			if (pass == null) return;

			var subsAttribute = pass.Attributes["subs"] as NamedAttribute;

			if (subsAttribute != null)
			{
				
			}

			_writer.Write(pass.Text);
		}

		public virtual void Visit(Sidebar sidebar)
		{
		}

		public virtual void Visit(Table table)
		{
		}

		public virtual void Visit(DocumentTitle title)
		{
			if (title != null)
			{
				_writer.WriteLine("<title>{0}</title>", title.Title);
			}
			else
			{
				_writer.WriteLine("<title>AsciidocNet</title>");
			}
		}

		public virtual void Visit(InternalAnchor anchor)
		{
		}

		public virtual void Visit(InlineAnchor anchor)
		{
		}

		public virtual void Visit(Stem stem)
		{
		}

		public virtual void Visit(Verse verse)
		{
		}

		public virtual void Visit(Subscript subscript)
		{
			_writer.Write("<sub>{0}</sub>", subscript.Text);
		}

		public virtual void Visit(Superscript superscript)
		{
			_writer.Write("<sup>{0}</sup>", superscript.Text);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
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

		private static string HtmlEncode(string text)
		{
			return Substitutor.SpecialCharacters.Aggregate(text, (current, specialCharacter) => current.Replace(specialCharacter.Key, specialCharacter.Value));
		}
	}
}