using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class UnorderedListItemTests
	{
		[Test]
		[TestCase("- List item 1", 1)]
		[TestCase("- List item 1\n- List Item 2", 2)]
		[TestCase("- List item 1\n- List Item 2\n- List Item 3", 3)]
		public void ShouldParseSingleLineListItems(string lines, int count)
		{
			var document = Document.Parse(lines);

			Assert.IsNotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<UnorderedList>(document[0]);

			var list = (UnorderedList)document[0];

			Assert.IsTrue(list.Items.Count == count);
		}

		[Test]
		[Ignore("TODO")]
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
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<UnorderedList>(document[0]);

			var unorderListItem = (UnorderedList)document[0];

			Assert.IsTrue(unorderListItem.Items.Count == 1);

			var listItem = unorderListItem.Items[0];

			Assert.IsInstanceOf<Paragraph>(listItem[0]);
			Assert.IsInstanceOf<Source>(listItem[1]);
		}
	}
}