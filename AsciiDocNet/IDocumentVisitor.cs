using System.Collections.Generic;

namespace AsciidocNet
{
	public interface IDocumentVisitor
	{
		void Visit(Admonition admonition);

		void Visit(Anchor anchor);

		void Visit(Attribute attribute);

		void Visit(AttributeEntry attributeEntry);

		void Visit(AttributeList attributes);

		void Visit(AttributeReference reference);

		void Visit(Audio audio);

		void Visit(AuthorInfo author);

		void Visit(IList<AuthorInfo> authors);

		void Visit(Strong strong);

		void Visit(CheckListItem listItem);

		void Visit(Document document);

		void Visit(Mark mark);

		void Visit(Container elements);

		void Visit(IList<IInlineElement> inlineElements);

		void Visit(Image image);

		void Visit(Include include);

		void Visit(Emphasis emphasis);

		void Visit(LabeledListItem listItem);

		void Visit(LabeledList list);

		void Visit(Link link);

		void Visit(Listing listing);

		void Visit(Callout callout);

		void Visit(UnorderedListItem listItem);

		void Visit(UnorderedList list);

		void Visit(TextLiteral text);

		void Visit(Literal literal);

		void Visit(Media media);

		void Visit(Monospace monospace);

		void Visit(NamedAttribute attribute);

		void Visit(Open open);

		void Visit(OrderedListItem listItem);

		void Visit(OrderedList list);

		void Visit(Paragraph paragraph);

		void Visit(Quotation quotation);

		void Visit(Quote quote);

		void Visit(SectionTitle sectionTitle);

		void Visit(Source source);

		void Visit(Title title);

		void Visit(UnsetAttributeEntry attributeEntry);

		void Visit(Video video);

		void Visit(Example example);

		void Visit(Comment comment);

		void Visit(Fenced fenced);

		void Visit(Pass pass);

		void Visit(Sidebar sidebar);

		void Visit(Table table);

		void Visit(DocumentTitle title);

		void Visit(InternalAnchor anchor);

		void Visit(InlineAnchor anchor);

		void Visit(Stem stem);

		void Visit(Verse verse);

		void Visit(Subscript subscript);

		void Visit(Superscript superscript);
	}
}