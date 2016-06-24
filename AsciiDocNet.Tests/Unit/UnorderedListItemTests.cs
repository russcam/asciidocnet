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

		[Fact(Skip = "TODO")]
		public void ShouldParseMultiElementListItem()
		{
			var document = Document.Parse(@"- List Item 
This is a paragraph
+
[source, csharp]
----
This is a code block
----
");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var unorderListItem = (UnorderedList)document[0];

			Assert.True(unorderListItem.Items.Count == 1);

			var listItem = unorderListItem.Items[0];

			Assert.IsType<Paragraph>(listItem[0]);
			Assert.IsType<Source>(listItem[1]);
		}
	}
}