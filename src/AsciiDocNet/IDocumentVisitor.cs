using System.Collections.Generic;

namespace AsciiDocNet
{
    /// <summary>
    /// Visits elements of an AsciiDoc
    /// </summary>
    public interface IDocumentVisitor
    {
        /// <summary>
        /// Visits the admonition.
        /// </summary>
        /// <param name="admonition">The admonition.</param>
        void VisitAdmonition(Admonition admonition);

        /// <summary>
        /// Visits the anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        void VisitAnchor(Anchor anchor);

        /// <summary>
        /// Visits the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        void VisitAttribute(Attribute attribute);

        /// <summary>
        /// Visits the attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        void VisitAttributeEntry(AttributeEntry attributeEntry);

        /// <summary>
        /// Visits the attribute list.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        void VisitAttributeList(AttributeList attributes);

        /// <summary>
        /// Visits the attribute reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void VisitAttributeReference(AttributeReference reference);

        /// <summary>
        /// Visits the audio.
        /// </summary>
        /// <param name="audio">The audio.</param>
        void VisitAudio(Audio audio);

        /// <summary>
        /// Visits the author
        /// </summary>
        /// <param name="author">The author.</param>
        void VisitAuthorInfo(AuthorInfo author);

        /// <summary>
        /// Visits the authors.
        /// </summary>
        /// <param name="authors">The authors.</param>
        void VisitAuthorInfos(IList<AuthorInfo> authors);

        /// <summary>
        /// Visits the strong.
        /// </summary>
        /// <param name="strong">The strong.</param>
        void VisitStrong(Strong strong);

        /// <summary>
        /// Visits the check list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        void VisitCheckListItem(CheckListItem listItem);

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document.</param>
        void VisitDocument(Document document);

        /// <summary>
        /// Visits the mark.
        /// </summary>
        /// <param name="mark">The mark.</param>
        void VisitMark(Mark mark);

        /// <summary>
        /// Visits the container.
        /// </summary>
        /// <param name="container">The container.</param>
        void VisitContainer(Container container);

        /// <summary>
        /// Visits the inline container.
        /// </summary>
        /// <param name="inlineContainer">The inline container.</param>
        void VisitInlineContainer(InlineContainer inlineContainer);

        /// <summary>
        /// Visits the image.
        /// </summary>
        /// <param name="image">The image.</param>
        void VisitImage(Image image);

        /// <summary>
        /// Visits the include.
        /// </summary>
        /// <param name="include">The include.</param>
        void VisitInclude(Include include);

        /// <summary>
        /// Visits the emphasis.
        /// </summary>
        /// <param name="emphasis">The emphasis.</param>
        void VisitEmphasis(Emphasis emphasis);

        /// <summary>
        /// Visits the labeled list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        void VisitLabeledListItem(LabeledListItem listItem);

        /// <summary>
        /// Visits the labeled list.
        /// </summary>
        /// <param name="list">The list.</param>
        void VisitLabeledList(LabeledList list);

        /// <summary>
        /// Visits the link.
        /// </summary>
        /// <param name="link">The link.</param>
        void VisitLink(Link link);

        /// <summary>
        /// Visits the listing.
        /// </summary>
        /// <param name="listing">The listing.</param>
        void VisitListing(Listing listing);

        /// <summary>
        /// Visits the callout.
        /// </summary>
        /// <param name="callout">The callout.</param>
        void VisitCallout(Callout callout);

        /// <summary>
        /// Visits the unordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        void VisitUnorderedListItem(UnorderedListItem listItem);

        /// <summary>
        /// Visits the unordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        void VisitUnorderedList(UnorderedList list);

        /// <summary>
        /// Visits the text literal.
        /// </summary>
        /// <param name="text">The text.</param>
        void VisitTextLiteral(TextLiteral text);

        /// <summary>
        /// Visits the literal.
        /// </summary>
        /// <param name="literal">The literal.</param>
        void VisitLiteral(Literal literal);

        /// <summary>
        /// Visits the media.
        /// </summary>
        /// <param name="media">The media.</param>
        void VisitMedia(Media media);

        /// <summary>
        /// Visits the monospace.
        /// </summary>
        /// <param name="monospace">The monospace.</param>
        void VisitMonospace(Monospace monospace);

        /// <summary>
        /// Visits the named attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        void VisitNamedAttribute(NamedAttribute attribute);

        /// <summary>
        /// Visits the open.
        /// </summary>
        /// <param name="open">The open.</param>
        void VisitOpen(Open open);

        /// <summary>
        /// Visits the ordered list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        void VisitOrderedListItem(OrderedListItem listItem);

        /// <summary>
        /// Visits the ordered list.
        /// </summary>
        /// <param name="list">The list.</param>
        void VisitOrderedList(OrderedList list);

        /// <summary>
        /// Visits the paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        void VisitParagraph(Paragraph paragraph);

        /// <summary>
        /// Visits the quotation mark.
        /// </summary>
        /// <param name="quotation">The QuotationMark.</param>
        void VisitQuotationMark(QuotationMark quotation);

        /// <summary>
        /// Visits the quote.
        /// </summary>
        /// <param name="quote">The quote.</param>
        void VisitQuote(Quote quote);

        /// <summary>
        /// Visits the section title.
        /// </summary>
        /// <param name="sectionTitle">The section title.</param>
        void VisitSectionTitle(SectionTitle sectionTitle);

        /// <summary>
        /// Visits the source.
        /// </summary>
        /// <param name="source">The source.</param>
        void VisitSource(Source source);

        /// <summary>
        /// Visits the title.
        /// </summary>
        /// <param name="title">The title.</param>
        void VisitTitle(Title title);

        /// <summary>
        /// Visits the unset attribute entry.
        /// </summary>
        /// <param name="attributeEntry">The attribute entry.</param>
        void VisitUnsetAttributeEntry(UnsetAttributeEntry attributeEntry);

        /// <summary>
        /// Visits the video.
        /// </summary>
        /// <param name="video">The video.</param>
        void VisitVideo(Video video);

        /// <summary>
        /// Visits the example.
        /// </summary>
        /// <param name="example">The example.</param>
        void VisitExample(Example example);

        /// <summary>
        /// Visits the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        void VisitComment(Comment comment);

        /// <summary>
        /// Visits the fenced.
        /// </summary>
        /// <param name="fenced">The fenced.</param>
        void VisitFenced(Fenced fenced);

        /// <summary>
        /// Visits the passthrough.
        /// </summary>
        /// <param name="passthrough">The Passthrough.</param>
        void VisitPassthrough(Passthrough passthrough);

        /// <summary>
        /// Visits the sidebar.
        /// </summary>
        /// <param name="sidebar">The sidebar.</param>
        void VisitSidebar(Sidebar sidebar);

        /// <summary>
        /// Visits the table.
        /// </summary>
        /// <param name="table">The table.</param>
        void VisitTable(Table table);

        /// <summary>
        /// Visits the document title.
        /// </summary>
        /// <param name="title">The title.</param>
        void VisitDocumentTitle(DocumentTitle title);

        /// <summary>
        /// Visits the internal anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        void VisitInternalAnchor(InternalAnchor anchor);

        /// <summary>
        /// Visits the inline anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        void VisitInlineAnchor(InlineAnchor anchor);

        /// <summary>
        /// Visits the stem.
        /// </summary>
        /// <param name="stem">The stem.</param>
        void VisitStem(Stem stem);

        /// <summary>
        /// Visits the verse.
        /// </summary>
        /// <param name="verse">The verse.</param>
        void VisitVerse(Verse verse);

        /// <summary>
        /// Visits the subscript.
        /// </summary>
        /// <param name="subscript">The subscript.</param>
        void VisitSubscript(Subscript subscript);

        /// <summary>
        /// Visits the superscript.
        /// </summary>
        /// <param name="superscript">The superscript.</param>
        void VisitSuperscript(Superscript superscript);
    }
}