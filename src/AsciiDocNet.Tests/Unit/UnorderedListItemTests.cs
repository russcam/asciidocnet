using System.Linq;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class UnorderedListItemTests
	{
		[Theory]
		[InlineData("- List item 1", 1)]
		[InlineData("- List item 1\n- List Item 2", 2)]
		[InlineData("- List item 1\n- List Item 2\n- List Item 3", 3)]
		public void ShouldParseSingleLineListItems(string lines, int count)
		{
			var document = Document.Parse(lines);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var list = (UnorderedList)document[0];

			Assert.True(list.Items.Count == count);
			Assert.True(list.Items.All(i => i.GetType() == typeof(UnorderedListItem)));
		}

		[Fact]
		public void ShouldParseListItemContinuationWithSourceBlock()
		{
			var document = Document.Parse(@"- List Item 
This is a paragraph
+
[source, csharp]
----
This is a code block
----
- List Item 2");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var unorderList = (UnorderedList)document[0];

			Assert.True(unorderList.Items.Count == 2);

			var listItem = unorderList.Items[0];

			Assert.IsType<Paragraph>(listItem[0]);
			Assert.IsType<Source>(listItem[1]);
		}		
		
		[Fact]
		public void ShouldParseListItemContinuationWithMultipleElements()
		{
			var document = Document.Parse(@"

* The header in AsciiDoc must start with a document title.
+
----
= Document Title
----
+
Keep in mind that the header is optional.

* Optional Author and Revision information immediately follows the header title.
+
----
= Document Title
Doc Writer <doc.writer@asciidoc.org>
v1.0, 2013-01-01
----

");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var unorderList = (UnorderedList)document[0];

			Assert.True(unorderList.Items.Count == 2);

			var firstListItem = unorderList.Items[0];
			Assert.True(firstListItem.Count == 3);			
			Assert.IsType<Paragraph>(firstListItem[0]);
			Assert.IsType<Listing>(firstListItem[1]);
			Assert.IsType<Paragraph>(firstListItem[2]);
			
			var secondListItem = unorderList.Items[1];
			Assert.True(secondListItem.Count == 2);			
			Assert.IsType<Paragraph>(secondListItem[0]);
			Assert.IsType<Listing>(secondListItem[1]);
		}
		
		[Fact]
		public void ShouldParseListItemContinuationWithOpenBlock()
		{
			var document = Document.Parse(@"* The header in AsciiDoc must start with a document title.
+
--
Here's an example of a document title:

----
= Document Title
----

NOTE: The header is optional.
--
");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var unorderList = (UnorderedList)document[0];

			Assert.True(unorderList.Items.Count == 1);

			var listItem = unorderList.Items[0];
			Assert.True(listItem.Count == 2);			
			Assert.IsType<Paragraph>(listItem[0]);
			Assert.IsType<Open>(listItem[1]);

			var open = (Open) listItem[1];
			
			Assert.True(open.Count == 3);
			Assert.IsType<Paragraph>(open[0]);
			Assert.IsType<Listing>(open[1]);
			Assert.IsType<Admonition>(open[2]);	
		}
	}
}