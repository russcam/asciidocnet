using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// Visits a <see cref="Document" /> and writes to HTML
    /// </summary>
    /// <seealso cref="AsciiDocNet.IDocumentVisitor" />
    /// <seealso cref="System.IDisposable" />
    public class HtmlVisitor : IDocumentVisitor, IDisposable
    {
        // keep the collection of attribute entries as we see them in the document
        private readonly Dictionary<string, AttributeEntry> _attributeEntries = new Dictionary<string, AttributeEntry>();
        private readonly TextWriter _writer;

        private bool _disposed;
        private int _figureNumber = 1;
        private bool _isBlock = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlVisitor"/> class.
        /// </summary>
        /// <param name="writer">The writer to write the output to.</param>
        public HtmlVisitor(TextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Visits the admonition.
        /// </summary>
        /// <param name="admonition">The admonition.</param>
        public virtual void VisitAdmonition(Admonition admonition)
        {
        }

        /// <summary>
        /// Visits the anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitAnchor(Anchor anchor)
        {
        }

        /// <summary>
        /// Visits the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public virtual void VisitAttribute(Attribute attribute)
        {
        }

        /// <summary>
        /// Visits the attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        public virtual void VisitAttributeEntry(AttributeEntry attributeEntry)
        {
            _attributeEntries[attributeEntry.Name] = attributeEntry;
        }

        /// <summary>
        /// Visits the attribute list.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public virtual void VisitAttributeList(AttributeList attributes)
        {
        }

        /// <summary>
        /// Visits the attribute reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public virtual void VisitAttributeReference(AttributeReference reference)
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

        /// <summary>
        /// Visits the audio.
        /// </summary>
        /// <param name="audio">The audio.</param>
        /// <exception cref="System.NotImplementedException">TODO: Html audio</exception>
        public virtual void VisitAudio(Audio audio)
        {
            throw new NotImplementedException("TODO: Html audio");
        }

        /// <summary>
        /// Visits the author
        /// </summary>
        /// <param name="author">The author.</param>
        /// <exception cref="System.NotImplementedException">TODO: Html author</exception>
        public virtual void VisitAuthorInfo(AuthorInfo author)
        {
            throw new NotImplementedException("TODO: Html author");
        }

        /// <summary>
        /// Visits the authors.
        /// </summary>
        /// <param name="authors">The authors.</param>
        /// <exception cref="System.NotImplementedException">TODO: Html authors</exception>
        public virtual void VisitAuthorInfos(IList<AuthorInfo> authors)
        {
            throw new NotImplementedException("TODO: Html authors");
        }

        /// <summary>
        /// Visits the strong.
        /// </summary>
        /// <param name="strong">The strong.</param>
        public virtual void VisitStrong(Strong strong)
        {
            if (strong == null)
            {
                return;
            }

            _writer.Write("<strong>");
            VisitInlineContainer((InlineContainer)strong);
            _writer.Write("</strong>");
        }

        /// <summary>
        /// Visits the check list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitCheckListItem(CheckListItem listItem)
        {
            throw new NotImplementedException("TODO: Checklist item");
        }

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document.</param>
        public virtual void VisitDocument(Document document)
        {
            _writer.WriteLine("<!doctype html>");
            _writer.WriteLine("<html>");

            _writer.WriteLine("<head>");

            foreach (var attribute in document.Attributes)
            {
                VisitAttributeEntry(attribute);
            }

            VisitDocumentTitle(document.Title);

            _writer.WriteLine("</head>");

            _writer.WriteLine("<body class=\"article\">");

            VisitContainer(document);

            _writer.WriteLine("</body>");
            _writer.WriteLine("</html>");
        }

        /// <summary>
        /// Visits the mark.
        /// </summary>
        /// <param name="mark">The mark.</param>
        public virtual void VisitMark(Mark mark)
        {
            string classes = null;
            foreach (var role in mark.Attributes.OfType<RoleAttribute>())
            {
                classes = string.Join(" ", role.Values);
            }

            if (classes != null)
            {
                _writer.Write("<span class=\"{0}\">", classes);
                VisitInlineContainer(mark);
                _writer.Write("</span>");
            }
            else
            {
                _writer.Write("<mark>");
                VisitInlineContainer(mark);
                _writer.Write("</mark>");
            }
        }

        /// <summary>
        /// Visits the container.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void VisitContainer(Container container)
        {
            foreach (var element in container)
            {
                element.Accept(this);
            }
        }

        /// <summary>
        /// Visits the inline container.
        /// </summary>
        /// <param name="inlineContainer">The inline container.</param>
        public virtual void VisitInlineContainer(InlineContainer inlineContainer)
        {
            foreach (var element in inlineContainer)
            {
                element.Accept(this);
            }
        }

        /// <summary>
        /// Visits the image.
        /// </summary>
        /// <param name="image">The image.</param>
        public virtual void VisitImage(Image image)
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

        /// <summary>
        /// Visits the include.
        /// </summary>
        /// <param name="include">The include.</param>
        /// <exception cref="System.NotImplementedException">TODO: write out include</exception>
        public virtual void VisitInclude(Include include)
        {
            throw new NotImplementedException("TODO: write out include");
        }

        /// <summary>
        /// Visits the emphasis.
        /// </summary>
        /// <param name="emphasis">The emphasis.</param>
        public virtual void VisitEmphasis(Emphasis emphasis)
        {
            _writer.Write("<em>{0}</em>", emphasis.Text);
        }

        /// <summary>
        /// Visits the labeled list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitLabeledListItem(LabeledListItem listItem)
        {
            throw new NotImplementedException("TODO: Labeled List Item");
        }

        /// <summary>
        /// Visits the labeled list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitLabeledList(LabeledList list)
        {
            throw new NotImplementedException("TODO: Labeled List");
        }

        /// <summary>
        /// Visits the link.
        /// </summary>
        /// <param name="link">The link.</param>
        public virtual void VisitLink(Link link)
        {
            _writer.Write("<a href=\"{0}\">{1}</a>", link.Href, link.Text);
        }

        /// <summary>
        /// Visits the listing.
        /// </summary>
        /// <param name="listing">The listing.</param>
        public virtual void VisitListing(Listing listing)
        {
        }

        /// <summary>
        /// Visits the callout.
        /// </summary>
        /// <param name="callout">The callout.</param>
        public virtual void VisitCallout(Callout callout)
        {
        }

        /// <summary>
        /// Visits the unordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitUnorderedListItem(UnorderedListItem listItem)
        {

            if (listItem.Count == 1)
            {
                _writer.Write("<li>");
                _isBlock = false;
                VisitContainer(listItem);
                _isBlock = true;
                _writer.WriteLine("</li>");
            }
            else
            {
                _writer.WriteLine("<li>");
                VisitContainer(listItem);
                _writer.WriteLine("</li>");
            }
        }

        /// <summary>
        /// Visits the unordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitUnorderedList(UnorderedList list)
        {
            _writer.WriteLine("<ul>");
            foreach (var listItem in list.Items)
            {
                listItem.Accept(this);
            }
            _writer.WriteLine("</ul>");
        }

        /// <summary>
        /// Visits the text literal.
        /// </summary>
        /// <param name="text">The text.</param>
        public virtual void VisitTextLiteral(TextLiteral text)
        {
            _writer.Write(text.Text);
        }

        /// <summary>
        /// Visits the literal.
        /// </summary>
        /// <param name="literal">The literal.</param>
        public virtual void VisitLiteral(Literal literal)
        {
        }

        /// <summary>
        /// Visits the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public virtual void VisitMedia(Media media)
        {
            media.Accept(this);
        }

        /// <summary>
        /// Visits the monospace.
        /// </summary>
        /// <param name="monospace">The monospace.</param>
        public virtual void VisitMonospace(Monospace monospace)
        {
            if (monospace == null)
            {
                return;
            }

            _writer.Write("<code>");
            VisitInlineContainer(monospace);
            _writer.Write("</code>");
        }

        /// <summary>
        /// Visits the named attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public virtual void VisitNamedAttribute(NamedAttribute attribute)
        {
        }

        /// <summary>
        /// Visits the open.
        /// </summary>
        /// <param name="open">The open.</param>
        public virtual void VisitOpen(Open open)
        {
            _writer.WriteLine("<div class=\"openblock\">");
            _writer.WriteLine("<div class=\"content\">");
            VisitContainer(open);
            _writer.WriteLine("</div>");
            _writer.WriteLine("</div>");
        }

        /// <summary>
        /// Visits the ordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitOrderedListItem(OrderedListItem listItem)
        {
        }

        /// <summary>
        /// Visits the ordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitOrderedList(OrderedList list)
        {

        }

        /// <summary>
        /// Visits the paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        public virtual void VisitParagraph(Paragraph paragraph)
        {
            if (_isBlock)
            {
                _writer.WriteLine("<div class=\"paragraph\">");
                _writer.Write("<p>");

                VisitInlineContainer(paragraph);

                _writer.WriteLine("</p>");
                _writer.WriteLine("</div>");
            }
            else
            {
                _writer.Write("<p>");
                VisitInlineContainer(paragraph);
                _writer.Write("</p>");
            }
        }

        /// <summary>
        /// Visits the QuotationMark.
        /// </summary>
        /// <param name="quotation">The QuotationMark.</param>
        public virtual void VisitQuotationMark(QuotationMark quotation)
        {
            if (quotation == null)
            {
                return;
            }

            _writer.Write(quotation.DoubleDelimited ? "“" : "‘");
            VisitInlineContainer(quotation);
            _writer.Write(quotation.DoubleDelimited ? "”" : "’");
        }

        /// <summary>
        /// Visits the quote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        public virtual void VisitQuote(Quote quote)
        {
        }

        /// <summary>
        /// Visits the section title.
        /// </summary>
        /// <param name="sectionTitle">The section title.</param>
        public virtual void VisitSectionTitle(SectionTitle sectionTitle)
        {
        }

        /// <summary>
        /// Visits the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public virtual void VisitSource(Source source)
        {
            _writer.WriteLine("<div class=\"listingblock\">");
            _writer.WriteLine("<div class=\"content\">");

            _writer.WriteLine("<pre class=\"highlight\"><code class=\"language-{0} hljs\" data-lang=\"{0}\">{1}</code></pre>",
                source.Attributes[1].Name,
                HtmlEncode(source.Text));

            _writer.WriteLine("</div>");
            _writer.WriteLine("</div>");
        }

        /// <summary>
        /// Visits the title.
        /// </summary>
        /// <param name="title">The title.</param>
        public virtual void VisitTitle(Title title)
        {
        }

        /// <summary>
        /// Visits the unset attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        public virtual void VisitUnsetAttributeEntry(UnsetAttributeEntry attributeEntry)
        {
        }

        /// <summary>
        /// Visits the video.
        /// </summary>
        /// <param name="video">The video.</param>
        public virtual void VisitVideo(Video video)
        {
        }

        /// <summary>
        /// Visits the example.
        /// </summary>
        /// <param name="example">The example.</param>
        public virtual void VisitExample(Example example)
        {
        }

        /// <summary>
        /// Visits the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void VisitComment(Comment comment)
        {
        }

        /// <summary>
        /// Visits the fenced.
        /// </summary>
        /// <param name="fenced">The fenced.</param>
        public virtual void VisitFenced(Fenced fenced)
        {
        }

        /// <summary>
        /// Visits the Passthrough.
        /// </summary>
        /// <param name="passthrough">The Passthrough.</param>
        public virtual void VisitPassthrough(Passthrough passthrough)
        {
            if (passthrough == null) return;

            var subsAttribute = passthrough.Attributes["subs"] as NamedAttribute;

            if (subsAttribute != null)
            {

            }

            _writer.Write(passthrough.Text);
        }

        /// <summary>
        /// Visits the sidebar.
        /// </summary>
        /// <param name="sidebar">The sidebar.</param>
        public virtual void VisitSidebar(Sidebar sidebar)
        {
        }

        /// <summary>
        /// Visits the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public virtual void VisitTable(Table table)
        {
        }

        /// <summary>
        /// Visits the document title.
        /// </summary>
        /// <param name="title">The title.</param>
        public virtual void VisitDocumentTitle(DocumentTitle title)
        {
            if (title != null)
            {
                _writer.WriteLine("<title>{0}</Title>", title.Title);
            }
            else
            {
                _writer.WriteLine("<title>AsciiDocNet</Title>");
            }
        }

        /// <summary>
        /// Visits the internal anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitInternalAnchor(InternalAnchor anchor)
        {
        }

        /// <summary>
        /// Visits the inline anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        public virtual void VisitInlineAnchor(InlineAnchor anchor)
        {
        }

        /// <summary>
        /// Visits the stem.
        /// </summary>
        /// <param name="stem">The stem.</param>
        public virtual void VisitStem(Stem stem)
        {
        }

        /// <summary>
        /// Visits the verse.
        /// </summary>
        /// <param name="verse">The verse.</param>
        public virtual void VisitVerse(Verse verse)
        {
        }

        /// <summary>
        /// Visits the subscript.
        /// </summary>
        /// <param name="subscript">The subscript.</param>
        public virtual void VisitSubscript(Subscript subscript)
        {
            _writer.Write("<sub>{0}</sub>", subscript.Text);
        }

        /// <summary>
        /// Visits the superscript.
        /// </summary>
        /// <param name="superscript">The superscript.</param>
        public virtual void VisitSuperscript(Superscript superscript)
        {
            _writer.Write("<sup>{0}</sup>", superscript.Text);
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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
            return Substitutor.SpecialCharacters.Aggregate(
                text,
                (current, specialCharacter) => current.Replace(specialCharacter.Key, specialCharacter.Value));
        }
    }
}