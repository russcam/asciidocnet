using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AsciiDocNet
{
    /// <summary>
    /// Visits a <see cref="Document" /> and writes out an AsciiDoc to
    /// a file / text writer.
    /// </summary>
    /// <seealso cref="AsciiDocNet.IDocumentVisitor" />
    /// <seealso cref="System.IDisposable" />
    public class AsciiDocVisitor : IDocumentVisitor, IDisposable
	{
		private readonly TextWriter _writer;
		private bool _disposed;
		private bool _insideListItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsciiDocVisitor"/> class.
        /// </summary>
        /// <param name="path">The path to the file where the AsciiDoc will be written.</param>
        /// <exception cref="System.ArgumentNullException">path must not be null</exception>
        /// <exception cref="System.ArgumentException">must specify a path</exception>
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

			_writer = new StreamWriter(new FileStream(path, FileMode.Create));
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="AsciiDocVisitor"/> class.
        /// </summary>
        /// <param name="writer">The writer to which the AsciiDoc will be written</param>
        /// <exception cref="System.ArgumentNullException">writer must not be null</exception>
        public AsciiDocVisitor(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException(nameof(writer));
			}
			_writer = writer;
		}

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document.</param>
        public virtual void VisitDocument(Document document)
		{
            if (document == null) return;

            foreach (var attribute in document.Attributes)
			{
				attribute.Accept(this);
			}

			VisitDocumentTitle(document.Title);
			VisitAuthorInfos(document.Authors);
			VisitContainer(document);
		}

        /// <summary>
        /// Visits the author
        /// </summary>
        /// <param name="author">The author.</param>
        public virtual void VisitAuthorInfo(AuthorInfo author)
		{
            if (author == null) return;
            _writer.Write("{0}{1}{2}",
                !string.IsNullOrEmpty(author.FirstName) ? author.FirstName.Replace(" ", "_") + " " : string.Empty,
                !string.IsNullOrEmpty(author.MiddleName) ? author.MiddleName.Replace(" ", "_") + " " : string.Empty,
                !string.IsNullOrEmpty(author.LastName) ? author.LastName.Replace(" ", "_") + " " : string.Empty);

            if (!string.IsNullOrEmpty(author.Email))
			{
				_writer.Write(" <{0}>", author.Email);
			}
		}

        /// <summary>
        /// Visits the authors.
        /// </summary>
        /// <param name="authors">The authors.</param>
        public virtual void VisitAuthorInfos(IList<AuthorInfo> authors)
		{
		    if (authors == null || !authors.Any()) return;

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

        /// <summary>
        /// Visits the ordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitOrderedList(OrderedList list)
		{
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

        /// <summary>
        /// Visits the paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        public virtual void VisitParagraph(Paragraph paragraph)
		{
		    if (paragraph == null) return;

		    VisitAttributeList(paragraph.Attributes);
			VisitInlineContainer(paragraph);
		
			_writer.WriteLine();
			
			if (paragraph.Count > 0)
			{
				var lastElement = paragraph[paragraph.Count - 1];
				if (!lastElement.ToString().EndsWith("\r\n") && !_insideListItem)
				{
					_writer.WriteLine();
				}
			}
		}

        /// <summary>
        /// Visits the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public virtual void VisitSource(Source source)
		{
            if (source == null) return;
			VisitAttributeList(source.Attributes);
			_writer.WriteLine(Patterns.Block.Source);
			_writer.WriteLine(source.Text);
			_writer.WriteLine(Patterns.Block.Source);
			foreach (var callout in source.Callouts)
			{
				VisitCallout(callout);
			}
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public virtual void VisitAttribute(Attribute attribute)
		{
            if (attribute == null) return;
		    _writer.Write(attribute.Name);
		}

        /// <summary>
        /// Visits the named attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public virtual void VisitNamedAttribute(NamedAttribute attribute)
        {
            if (attribute == null) return;
            _writer.Write("{0}={2}{1}{2}", attribute.Name, attribute.Value, attribute.SingleQuoted ? "'" : "\"");
        }

        /// <summary>
        /// Visits the attribute list.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public virtual void VisitAttributeList(AttributeList attributes)
		{
            if (attributes == null) return;
            if (attributes.HasAnchor)
                attributes.Anchor.Accept(this);

            if (attributes.IsDiscrete)
                _writer.WriteLine("[discrete]");
            else if (attributes.IsFloating)
                _writer.WriteLine("[float]");

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

        /// <summary>
        /// Visits the attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        public virtual void VisitAttributeEntry(AttributeEntry attributeEntry)
		{
		    if (attributeEntry == null) return;
		    if (!string.IsNullOrEmpty(attributeEntry.Value))
		        _writer.WriteLine(":{0}: {1}", attributeEntry.Name, attributeEntry.Value);
		    else
		        _writer.WriteLine(":{0}:", attributeEntry.Name);
		    _writer.WriteLine();
		}

        /// <summary>
        /// Visits the unordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitUnorderedList(UnorderedList list)
		{
            if (list == null) return;
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

        /// <summary>
        /// Visits the text literal.
        /// </summary>
        /// <param name="text">The text.</param>
        public virtual void VisitTextLiteral(TextLiteral text)
		{
            if (text == null) return;
		    _writer.Write(text.Text);
		}

        /// <summary>
        /// Visits the strong.
        /// </summary>
        /// <param name="strong">The strong.</param>
        public virtual void VisitStrong(Strong strong)
		{
            if (strong == null) return;
            _writer.Write(strong.DoubleDelimited ? "**" : "*");
			VisitInlineContainer(strong);
			_writer.Write(strong.DoubleDelimited ? "**" : "*");
		}

        /// <summary>
        /// Visits the emphasis.
        /// </summary>
        /// <param name="emphasis">The emphasis.</param>
        public virtual void VisitEmphasis(Emphasis emphasis)
		{
		    if (emphasis == null) return;
		    _writer.Write(emphasis.DoubleDelimited ? "__{0}__" : "_{0}_", emphasis.Text);
		}

        /// <summary>
        /// Visits the quotation mark.
        /// </summary>
        /// <param name="quotation">The QuotationMark.</param>
        public virtual void VisitQuotationMark(QuotationMark quotation)
		{
			if (quotation == null) return;
			_writer.Write(quotation.DoubleDelimited ? "\"`" : "'`");
			VisitInlineContainer(quotation);
			_writer.Write(quotation.DoubleDelimited ? "`\"" : "`'");
		}

        /// <summary>
        /// Visits the quote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        public virtual void VisitQuote(Quote quote)
		{
            if (quote == null) return;
			VisitAttributeList(quote.Attributes);
			var isBlock = quote.Count > 1;
		    if (isBlock)
		        _writer.WriteLine(Patterns.Block.Quote);
		    VisitContainer(quote);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Quote);
				_writer.WriteLine();
			}
		}

        /// <summary>
        /// Visits the section title.
        /// </summary>
        /// <param name="sectionTitle">The section title.</param>
        public virtual void VisitSectionTitle(SectionTitle sectionTitle)
		{
            if (sectionTitle == null) return;
			VisitAttributeList(sectionTitle.Attributes);
			_writer.Write("{0} ", new string('=', sectionTitle.Level));
			VisitInlineContainer(sectionTitle);
			_writer.WriteLine();
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the unordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitUnorderedListItem(UnorderedListItem listItem)
		{
            if (listItem == null) return;
			_writer.Write("{0} ", new string('*', listItem.Level));
			if (listItem.Count > 1)
			{
				_insideListItem = true;
			}
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				if (lastElement)
				{
					_insideListItem = false;
				}			
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}		
		}

        /// <summary>
        /// Visits the monospace.
        /// </summary>
        /// <param name="monospace">The monospace.</param>
        public virtual void VisitMonospace(Monospace monospace)
		{
            if (monospace == null) return;
			_writer.Write(monospace.DoubleDelimited ? "``" : "`");
			VisitInlineContainer(monospace);
			_writer.Write(monospace.DoubleDelimited ? "``" : "`");
		}

        /// <summary>
        /// Visits the unset attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        public virtual void VisitUnsetAttributeEntry(UnsetAttributeEntry attributeEntry)
		{
            if (attributeEntry == null) return;
			_writer.WriteLine(":{0}!:", attributeEntry.Name);
			_writer.WriteLine();
		}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Visits the open.
        /// </summary>
        /// <param name="open">The open.</param>
        public virtual void VisitOpen(Open open)
		{
            if (open == null) return;
			VisitAttributeList(open.Attributes);
			_writer.WriteLine(Patterns.Block.Open);
			VisitContainer(open);
			_writer.WriteLine(Patterns.Block.Open);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the callout.
        /// </summary>
        /// <param name="callout">The callout.</param>
        public virtual void VisitCallout(Callout callout)
		{
            if (callout == null) return;
			_writer.WriteLine("<{0}> {1}", callout.Number, callout.Text);
		}

        /// <summary>
        /// Visits the literal.
        /// </summary>
        /// <param name="literal">The literal.</param>
        public virtual void VisitLiteral(Literal literal)
		{
            if (literal == null) return;
			VisitAttributeList(literal.Attributes);
			_writer.WriteLine(Patterns.Block.Literal);
			_writer.WriteLine(literal.Text);
			_writer.WriteLine(Patterns.Block.Literal);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the title.
        /// </summary>
        /// <param name="title">The title.</param>
        public virtual void VisitTitle(Title title)
		{
            if (title == null) return;
			_writer.WriteLine(".{0}", title.Text);
		}

        /// <summary>
        /// Visits the check list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitCheckListItem(CheckListItem listItem)
		{
            if (listItem == null) return;
			_writer.Write("{0} [{1}] ", new string('-', listItem.Level), listItem.Checked? "x" : " ");
			if (listItem.Count > 1)
			{
				_insideListItem = true;
			}
			
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				if (lastElement)
				{
					_insideListItem = false;
				}
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}		
		}

        /// <summary>
        /// Visits the ordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitOrderedListItem(OrderedListItem listItem)
		{
            if (listItem == null) return;
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

			if (listItem.Count > 1)
			{
				_insideListItem = true;
			}
			
			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;
				if (lastElement)
				{
					_insideListItem = false;
				}
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}
		}

        /// <summary>
        /// Visits the labeled list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitLabeledListItem(LabeledListItem listItem)
		{
            if (listItem == null) return;
			VisitAttributeList(listItem.Attributes);
			_writer.WriteLine("{0}{1}", listItem.Label, new string(':', listItem.Level + 2));
			_writer.WriteLine();
			if (listItem.Count > 1)
			{
				_insideListItem = true;
			}

			for (int index = 0; index < listItem.Count; index++)
			{
				var element = listItem[index];
				var lastElement = index == listItem.Count - 1;

				if (lastElement)
				{
					_insideListItem = false;
				}		
				element.Accept(this);
				if (!lastElement)
				{
					_writer.WriteLine("+");
				}
			}
		}

        /// <summary>
        /// Visits the labeled list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitLabeledList(LabeledList list)
		{
            if (list == null) return;
			foreach (var listItem in list.Items)
			{
				listItem.Accept(this);
			}
		}

        /// <summary>
        /// Visits the link.
        /// </summary>
        /// <param name="link">The link.</param>
        public virtual void VisitLink(Link link)
		{
		    if (link == null) return;
			if (link.Text == null)
			{
				_writer.Write("{0}", link.Href);
			}
			else
			{
				_writer.Write("{0}[{1}]", link.Href, link.Text);
			}	    
		}

        /// <summary>
        /// Visits the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public virtual void VisitMedia(Media media)
		{
            media?.Accept(this);
		}

        /// <summary>
        /// Visits the container.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void VisitContainer(Container container)
		{
            if (container == null) return;
			for (int i = 0; i < container.Count; i++)
			{
				var element = container[i];
				element.Accept(this);
			}
		}

        /// <summary>
        /// Visits the inline container.
        /// </summary>
        /// <param name="inlineContainer">The inline container.</param>
        public virtual void VisitInlineContainer(InlineContainer inlineContainer)
		{
            if (inlineContainer == null) return;
			for (int index = 0; index < inlineContainer.Count; index++)
			{
				var element = inlineContainer[index];
				element.Accept(this);
			}
		}

        /// <summary>
        /// Visits the image.
        /// </summary>
        /// <param name="image">The image.</param>
        public virtual void VisitImage(Image image) => VisitMedia(image, "image");

        /// <summary>
        /// Visits the video.
        /// </summary>
        /// <param name="video">The video.</param>
        public virtual void VisitVideo(Video video) => VisitMedia(video, "video");

        /// <summary>
        /// Visits the audio.
        /// </summary>
        /// <param name="audio">The audio.</param>
        public virtual void VisitAudio(Audio audio) => VisitMedia(audio, "audio");

        /// <summary>
        /// Visits the include.
        /// </summary>
        /// <param name="include">The include.</param>
        public virtual void VisitInclude(Include include)
		{
		    if (include == null) return;
		    var attributes = new StringBuilder();
			if (include.LevelOffset.HasValue)
			{
				attributes.Append($"leveloffset={include.LevelOffset},");
			}
			if (!string.IsNullOrEmpty(include.Lines))
			{
				attributes.Append($"lines=\"{include.Lines}\",");
			}
			if (!string.IsNullOrEmpty(include.Tags))
			{
				attributes.Append($"tags=\"{include.Tags}\",");
			}
			if (include.Indent.HasValue)
			{
				attributes.Append($"indent={include.Indent},");
			}

			_writer.WriteLine("include::{0}[{1}]", include.Path, attributes.ToString(0, Math.Max(0, attributes.Length - 1)));
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the listing.
        /// </summary>
        /// <param name="listing">The listing.</param>
        public virtual void VisitListing(Listing listing)
		{
            if (listing == null) return;
			VisitAttributeList(listing.Attributes);
			_writer.WriteLine(Patterns.Block.Listing);
			_writer.WriteLine(listing.Text);
			_writer.WriteLine(Patterns.Block.Listing);
			foreach (var callout in listing.Callouts)
			{
				VisitCallout(callout);
			}

			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the example.
        /// </summary>
        /// <param name="example">The example.</param>
        public virtual void VisitExample(Example example)
		{
            if (example == null) return;
			VisitAttributeList(example.Attributes);
			var isBlock = example.Count > 1;
		    if (isBlock)
		        _writer.WriteLine(Patterns.Block.Example);
		    VisitContainer(example);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Example);
				_writer.WriteLine();
			}
		}

        /// <summary>
        /// Visits the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void VisitComment(Comment comment)
		{
            if (comment == null) return;
		    if (comment.Style == CommentStyle.MultiLine)
		    {
		        _writer.WriteLine(Patterns.Block.Comment);
		        _writer.WriteLine(comment.Text);
		        _writer.WriteLine(Patterns.Block.Comment);
		        _writer.WriteLine();
		    }
		    else _writer.WriteLine("//{0}", comment.Text);
		}

        /// <summary>
        /// Visits the attribute reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public virtual void VisitAttributeReference(AttributeReference reference)
		{
            if (reference == null) return;
			_writer.Write("{{{0}}}", reference.Text);
		}

        /// <summary>
        /// Visits the fenced.
        /// </summary>
        /// <param name="fenced">The fenced.</param>
        public virtual void VisitFenced(Fenced fenced)
		{
            if (fenced == null) return;
			VisitAttributeList(fenced.Attributes);
			_writer.WriteLine(Patterns.Block.Fenced);
			_writer.WriteLine(fenced.Text);
			_writer.WriteLine(Patterns.Block.Fenced);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the passthrough.
        /// </summary>
        /// <param name="passthrough">The Passthrough.</param>
        public virtual void VisitPassthrough(Passthrough passthrough)
		{
            if (passthrough == null) return;
			VisitAttributeList(passthrough.Attributes);
			_writer.WriteLine(Patterns.Block.Pass);
			_writer.WriteLine(passthrough.Text);
			_writer.WriteLine(Patterns.Block.Pass);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the sidebar.
        /// </summary>
        /// <param name="sidebar">The sidebar.</param>
        public virtual void VisitSidebar(Sidebar sidebar)
		{
            if (sidebar == null) return;
			VisitAttributeList(sidebar.Attributes);
			_writer.WriteLine(Patterns.Block.Sidebar);
			VisitContainer(sidebar);
			_writer.WriteLine(Patterns.Block.Sidebar);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <exception cref="System.NotImplementedException">TODO VisitTable</exception>
        public virtual void VisitTable(Table table)
		{
			throw new NotImplementedException("TODO VisitTable");
		}

        /// <summary>
        /// Visits the document title.
        /// </summary>
        /// <param name="title">The title.</param>
        public virtual void VisitDocumentTitle(DocumentTitle title)
		{
		    if (title == null) return;
		    VisitAttributeList(title.Attributes);
		    if (!string.IsNullOrEmpty(title.Subtitle))
		        _writer.WriteLine("= {0}: {1}", title.Title, title.Subtitle);
		    else
		        _writer.WriteLine("= {0}", title.Title);

		    _writer.WriteLine();
		}

        /// <summary>
        /// Visits the internal anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitInternalAnchor(InternalAnchor anchor)
		{
            if (anchor == null) return;
		    if (!string.IsNullOrEmpty(anchor.XRefLabel))
		        _writer.Write("<<{0},{1}>>", anchor.Id, anchor.XRefLabel);
		    else
		        _writer.Write("<<{0}>>", anchor.Id);
		}

        /// <summary>
        /// Visits the inline anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitInlineAnchor(InlineAnchor anchor)
		{
            if (anchor == null) return;
		    if (!string.IsNullOrEmpty(anchor.XRefLabel))
		        _writer.Write("[[{0},{1}]]", anchor.Id, anchor.XRefLabel);
		    else
		        _writer.Write("[[{0}]]", anchor.Id);
		}

        /// <summary>
        /// Visits the admonition.
        /// </summary>
        /// <param name="admonition">The admonition.</param>
        public virtual void VisitAdmonition(Admonition admonition)
		{
			// TODO: Handle admonition block
            if (admonition == null) return;
			_writer.Write("{0}: ", admonition.Style.ToString().ToUpperInvariant());
			VisitContainer(admonition);
		}

        /// <summary>
        /// Visits the anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitAnchor(Anchor anchor)
		{
            if (anchor == null) return;
		    if (!string.IsNullOrEmpty(anchor.XRefLabel))
		        _writer.WriteLine("[[{0},{1}]]", anchor.Id, anchor.XRefLabel);
		    else
		        _writer.WriteLine("[[{0}]]", anchor.Id);
		}

        /// <summary>
        /// Visits the stem.
        /// </summary>
        /// <param name="stem">The stem.</param>
        public virtual void VisitStem(Stem stem)
		{
		    if (stem == null) return;
		    VisitAttributeList(stem.Attributes);
			_writer.WriteLine(Patterns.Block.Stem);
			_writer.WriteLine(stem.Text);
			_writer.WriteLine(Patterns.Block.Stem);
			_writer.WriteLine();
		}

        /// <summary>
        /// Visits the verse.
        /// </summary>
        /// <param name="verse">The verse.</param>
        public virtual void VisitVerse(Verse verse)
		{
		    if (verse == null) return;
		    VisitAttributeList(verse.Attributes);
			var isBlock = verse.Count > 1;
		    if (isBlock)
		        _writer.WriteLine(Patterns.Block.Verse);
		    VisitContainer(verse);
			if (isBlock)
			{
				_writer.WriteLine(Patterns.Block.Verse);
				_writer.WriteLine();
			}
		}

        /// <summary>
        /// Visits the mark.
        /// </summary>
        /// <param name="mark">The mark.</param>
        public virtual void VisitMark(Mark mark)
		{
		    if (mark == null) return;
		    var roleAttribute = mark.Attributes["role"] as RoleAttribute;
			if (roleAttribute != null)
			{
				_writer.Write("[{0}]", roleAttribute.Value);
			}

			_writer.Write(mark.DoubleDelimited ? "##" : "#");
			VisitInlineContainer(mark);
			_writer.Write(mark.DoubleDelimited ? "##" : "#");
		}

        /// <summary>
        /// Visits the subscript.
        /// </summary>
        /// <param name="subscript">The subscript.</param>
        public virtual void VisitSubscript(Subscript subscript)
		{
            if (subscript == null) return;
			_writer.Write("~{0}~", subscript.Text);
		}

        /// <summary>
        /// Visits the superscript.
        /// </summary>
        /// <param name="superscript">The superscript.</param>
        public virtual void VisitSuperscript(Superscript superscript)
		{
            if (superscript == null) return;
			_writer.Write("^{0}^", superscript.Text);
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
			    if (disposing)
			        _writer?.Dispose();

			    _disposed = true;
			}
		}

        /// <summary>
        /// Visits the media.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="name">The name.</param>
        private void VisitMedia(Media media, string name)
		{
		    if (media == null) return;
		    VisitAttributeList(media.Attributes);
			var attributes = new StringBuilder();
			if (!string.IsNullOrEmpty(media.AlternateText))
			{
				attributes.Append(media.AlternateText + ",");

				if (media.Width.HasValue)
				{
					attributes.Append(media.Width + ",");
				}
				if (media.Height.HasValue)
				{
					attributes.Append(media.Height + ",");
				}
			}
			if (!string.IsNullOrEmpty(media.Title))
			{
				attributes.Append($"title=\"{media.Title}\",");
			}
			if (!string.IsNullOrEmpty(media.Align))
			{
				attributes.Append($"align=\"{media.Align}\",");
			}
			if (!string.IsNullOrEmpty(media.Link))
			{
				attributes.Append($"link=\"{media.Link}\",");
			}
			if (!string.IsNullOrEmpty(media.Float))
			{
				attributes.Append($"float=\"{media.Float}\",");
			}
			if (!string.IsNullOrEmpty(media.Role))
			{
				attributes.Append($"role=\"{media.Role}\",");
			}

			_writer.WriteLine("{0}::{1}[{2}]", name, media.Path, attributes.ToString(0, Math.Max(0, attributes.Length - 1)));
			_writer.WriteLine();
		}
	}
}