using System.Collections.Generic;

namespace AsciidocNet
{
	public abstract class NoopVisitor : IDocumentVisitor
	{
		public virtual void Visit(Admonition admonition)
		{
			VisitAttributable(admonition);
			Visit((Container)admonition);
		}

		public virtual void Visit(Anchor anchor)
		{
		}

		public virtual void Visit(Attribute attribute)
		{
		}

		public virtual void Visit(AttributeEntry attributeEntry)
		{
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

			if (attributes.HasTitle)
			{
				attributes.Title.Accept(this);
			}

			foreach (var attribute in attributes)
			{
				attribute.Accept(this);
			}
		}

		public virtual void Visit(AttributeReference reference)
		{
		}

		public virtual void Visit(Audio audio)
		{
			VisitAttributable(audio);
		}

		public virtual void Visit(AuthorInfo author)
		{
		}

		public virtual void Visit(IList<AuthorInfo> authors)
		{
			foreach (var author in authors)
			{
				author.Accept(this);
			}
		}

		public virtual void Visit(Strong strong)
		{
		}

		public virtual void Visit(CheckListItem listItem)
		{
			VisitAttributable(listItem);
			Visit((Container)listItem);
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

		public virtual void Visit(Mark mark)
		{
		}

		public virtual void Visit(Container elements)
		{
			if (elements == null)
			{
				return;
			}

			for (int index = 0; index < elements.Count; index++)
			{
				var element = elements[index];
				element.Accept(this);
			}
		}

		public virtual void Visit(IList<IInlineElement> inlineElements)
		{
			for (int index = 0; index < inlineElements.Count; index++)
			{
				var inlineElement = inlineElements[index];
				inlineElement.Accept(this);
			}
		}

		public virtual void Visit(Image image)
		{
			VisitAttributable(image);
		}

		public virtual void Visit(Include include)
		{
		}

		public virtual void Visit(Emphasis emphasis)
		{
		}

		public virtual void Visit(LabeledListItem listItem)
		{
			VisitAttributable(listItem);
			Visit((Container)listItem);
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
		}

		public virtual void Visit(Listing listing)
		{
			VisitAttributable(listing);
			VisitCallouts(listing);
		}

		public virtual void Visit(Callout callout)
		{
		}

		public virtual void Visit(UnorderedListItem listItem)
		{
			VisitAttributable(listItem);
			Visit((Container)listItem);
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
		}

		public virtual void Visit(Literal literal)
		{
			VisitAttributable(literal);
		}

		public virtual void Visit(Media media)
		{
		}

		public virtual void Visit(Monospace monospace)
		{
		}

		public virtual void Visit(NamedAttribute attribute)
		{
		}

		public virtual void Visit(Open open)
		{
			VisitAttributable(open);
			Visit((Container)open);
		}

		public virtual void Visit(OrderedListItem listItem)
		{
			VisitAttributable(listItem);
			Visit((Container)listItem);
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
			VisitAttributable(paragraph);
			VisitContainerInlineElement(paragraph);
		}

		public virtual void Visit(Quotation quotation)
		{
		}

		public virtual void Visit(Quote quote)
		{
			VisitAttributable(quote);
			Visit((Container)quote);
		}

		public virtual void Visit(SectionTitle sectionTitle)
		{
			VisitContainerInlineElement(sectionTitle);
		}

		public virtual void Visit(Source source)
		{
			VisitAttributable(source);
			VisitCallouts(source);
		}

		public virtual void Visit(Title title)
		{
		}

		public virtual void Visit(UnsetAttributeEntry attributeEntry)
		{
		}

		public virtual void Visit(Video video)
		{
			VisitAttributable(video);
		}

		public virtual void Visit(Example example)
		{
			VisitAttributable(example);
			Visit((Container)example);
		}

		public virtual void Visit(Comment comment)
		{
			VisitAttributable(comment);
		}

		public virtual void Visit(Fenced fenced)
		{
			VisitAttributable(fenced);
		}

		public virtual void Visit(Pass pass)
		{
			VisitAttributable(pass);
		}

		public virtual void Visit(Sidebar sidebar)
		{
			VisitAttributable(sidebar);
			Visit((Container)sidebar);
		}

		public virtual void Visit(Table table)
		{
			VisitAttributable(table);
			Visit((Container)table);
		}

		public virtual void Visit(DocumentTitle title)
		{
			VisitAttributable(title);
		}

		public virtual void Visit(InternalAnchor anchor)
		{
		}

		public virtual void Visit(InlineAnchor anchor)
		{
		}

		public virtual void Visit(Stem stem)
		{
			VisitAttributable(stem);
		}

		public virtual void Visit(Verse verse)
		{
			VisitAttributable(verse);
			Visit((Container)verse);
		}

		public virtual void Visit(Subscript subscript)
		{
		}

		public virtual void Visit(Superscript superscript)
		{
		}

		private void VisitAttributable(IAttributable attributable)
		{
			if (attributable == null)
			{
				return;
			}
			Visit(attributable.Attributes);
		}

		private void VisitCallouts(Listing element)
		{
			if (element == null)
			{
				return;
			}
			foreach (var callout in element.Callouts)
			{
				Visit(callout);
			}
		}

		private void VisitContainerInlineElement(IContainerInlineElement container)
		{
			if (container == null)
			{
				return;
			}
			Visit(container.Elements);
		}
	}
}