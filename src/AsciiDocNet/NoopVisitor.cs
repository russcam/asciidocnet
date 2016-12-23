using System.Collections.Generic;

namespace AsciiDocNet
{
    /// <summary>
    /// A base visitor from which to derive visitors to
    /// visit AsciiDoc documents
    /// </summary>
    /// <remarks>
    /// Override only the visit methods for elements to visit.
    /// </remarks>
    /// <seealso cref="AsciiDocNet.IDocumentVisitor" />
    public abstract class NoopVisitor : IDocumentVisitor
    {
        /// <summary>
        /// Visits the admonition.
        /// </summary>
        /// <param name="admonition">The admonition.</param>
        public virtual void VisitAdmonition(Admonition admonition)
        {
            VisitAttributable(admonition);
            VisitContainer(admonition);
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
        }

        /// <summary>
        /// Visits the attribute list.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public virtual void VisitAttributeList(AttributeList attributes)
        {
            if (attributes == null) return;

            if (attributes.HasAnchor)
            {
                attributes.Anchor.Accept(this);
            }

            if (attributes.HasTitle)
            {
                attributes.Title.Accept(this);
            }

            foreach (var attribute in attributes)
            {
                attribute.Accept(this);
            }
        }

        /// <summary>
        /// Visits the attribute reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public virtual void VisitAttributeReference(AttributeReference reference)
        {
        }

        /// <summary>
        /// Visits the audio.
        /// </summary>
        /// <param name="audio">The audio.</param>
        public virtual void VisitAudio(Audio audio)
        {
            VisitAttributable(audio);
        }

        /// <summary>
        /// Visits the author
        /// </summary>
        /// <param name="author">The author.</param>
        public virtual void VisitAuthorInfo(AuthorInfo author)
        {
        }

        /// <summary>
        /// Visits the authors.
        /// </summary>
        /// <param name="authors">The authors.</param>
        public virtual void VisitAuthorInfos(IList<AuthorInfo> authors)
        {
            foreach (var author in authors)
            {
                author.Accept(this);
            }
        }

        /// <summary>
        /// Visits the strong.
        /// </summary>
        /// <param name="strong">The strong.</param>
        public virtual void VisitStrong(Strong strong)
        {
        }

        /// <summary>
        /// Visits the check list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitCheckListItem(CheckListItem listItem)
        {
            VisitAttributable(listItem);
            VisitContainer(listItem);
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
        /// Visits the mark.
        /// </summary>
        /// <param name="mark">The mark.</param>
        public virtual void VisitMark(Mark mark)
        {
            VisitInlineContainer(mark);
        }

        /// <summary>
        /// Visits the container.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void VisitContainer(Container container)
        {
            if (container == null) return;
            for (int index = 0; index < container.Count; index++)
            {
                var element = container[index];
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
                var inlineElement = inlineContainer[index];
                inlineElement.Accept(this);
            }
        }

        /// <summary>
        /// Visits the image.
        /// </summary>
        /// <param name="image">The image.</param>
        public virtual void VisitImage(Image image)
        {
            VisitAttributable(image);
        }

        /// <summary>
        /// Visits the include.
        /// </summary>
        /// <param name="include">The include.</param>
        public virtual void VisitInclude(Include include)
        {
        }

        /// <summary>
        /// Visits the emphasis.
        /// </summary>
        /// <param name="emphasis">The emphasis.</param>
        public virtual void VisitEmphasis(Emphasis emphasis)
        {
        }

        /// <summary>
        /// Visits the labeled list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitLabeledListItem(LabeledListItem listItem)
        {
            VisitAttributable(listItem);
            VisitContainer(listItem);
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
        }

        /// <summary>
        /// Visits the listing.
        /// </summary>
        /// <param name="listing">The listing.</param>
        public virtual void VisitListing(Listing listing)
        {
            VisitAttributable(listing);
            VisitCallouts(listing);
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
            VisitAttributable(listItem);
            VisitContainer(listItem);
        }

        /// <summary>
        /// Visits the unordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        public virtual void VisitUnorderedList(UnorderedList list)
        {
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
        }

        /// <summary>
        /// Visits the literal.
        /// </summary>
        /// <param name="literal">The literal.</param>
        public virtual void VisitLiteral(Literal literal)
        {
            VisitAttributable(literal);
        }

        /// <summary>
        /// Visits the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public virtual void VisitMedia(Media media)
        {
        }

        /// <summary>
        /// Visits the monospace.
        /// </summary>
        /// <param name="monospace">The monospace.</param>
        public virtual void VisitMonospace(Monospace monospace)
        {
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
            VisitAttributable(open);
            VisitContainer(open);
        }

        /// <summary>
        /// Visits the ordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        public virtual void VisitOrderedListItem(OrderedListItem listItem)
        {
            VisitAttributable(listItem);
            VisitContainer(listItem);
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
            VisitAttributable(paragraph);
            VisitInlineContainer(paragraph);
        }

        /// <summary>
        /// Visits the quotation mark.
        /// </summary>
        /// <param name="quotation">The QuotationMark.</param>
        public virtual void VisitQuotationMark(QuotationMark quotation)
        {
        }

        /// <summary>
        /// Visits the quote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        public virtual void VisitQuote(Quote quote)
        {
            VisitAttributable(quote);
            VisitContainer(quote);
        }

        /// <summary>
        /// Visits the section title.
        /// </summary>
        /// <param name="sectionTitle">The section title.</param>
        public virtual void VisitSectionTitle(SectionTitle sectionTitle)
        {
            VisitInlineContainer(sectionTitle);
        }

        /// <summary>
        /// Visits the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public virtual void VisitSource(Source source)
        {
            VisitAttributable(source);
            VisitCallouts(source);
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
            VisitAttributable(video);
        }

        /// <summary>
        /// Visits the example.
        /// </summary>
        /// <param name="example">The example.</param>
        public virtual void VisitExample(Example example)
        {
            VisitAttributable(example);
            VisitContainer(example);
        }

        /// <summary>
        /// Visits the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void VisitComment(Comment comment)
        {
            VisitAttributable(comment);
        }

        /// <summary>
        /// Visits the fenced.
        /// </summary>
        /// <param name="fenced">The fenced.</param>
        public virtual void VisitFenced(Fenced fenced)
        {
            VisitAttributable(fenced);
        }

        /// <summary>
        /// Visits the passthrough.
        /// </summary>
        /// <param name="passthrough">The Passthrough.</param>
        public virtual void VisitPassthrough(Passthrough passthrough)
        {
            VisitAttributable(passthrough);
        }

        /// <summary>
        /// Visits the sidebar.
        /// </summary>
        /// <param name="sidebar">The sidebar.</param>
        public virtual void VisitSidebar(Sidebar sidebar)
        {
            VisitAttributable(sidebar);
            VisitContainer(sidebar);
        }

        /// <summary>
        /// Visits the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public virtual void VisitTable(Table table)
        {
            VisitAttributable(table);
            VisitContainer(table);
        }

        /// <summary>
        /// Visits the document title.
        /// </summary>
        /// <param name="title">The title.</param>
        public virtual void VisitDocumentTitle(DocumentTitle title)
        {
            VisitAttributable(title);
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
            VisitAttributable(stem);
        }

        /// <summary>
        /// Visits the verse.
        /// </summary>
        /// <param name="verse">The verse.</param>
        public virtual void VisitVerse(Verse verse)
        {
            VisitAttributable(verse);
            VisitContainer(verse);
        }

        /// <summary>
        /// Visits the subscript.
        /// </summary>
        /// <param name="subscript">The subscript.</param>
        public virtual void VisitSubscript(Subscript subscript)
        {
        }

        /// <summary>
        /// Visits the superscript.
        /// </summary>
        /// <param name="superscript">The superscript.</param>
        public virtual void VisitSuperscript(Superscript superscript)
        {
        }

        /// <summary>
        /// Visits the attributable.
        /// </summary>
        /// <param name="attributable">The attributable.</param>
        private void VisitAttributable(IAttributable attributable)
        {
            if (attributable == null) return;
            VisitAttributeList(attributable.Attributes);
        }

        /// <summary>
        /// Visits the callouts.
        /// </summary>
        /// <param name="element">The element.</param>
        private void VisitCallouts(Listing element)
        {
            if (element == null) return;
            foreach (var callout in element.Callouts)
            {
                VisitCallout(callout);
            }
        }
    }
}