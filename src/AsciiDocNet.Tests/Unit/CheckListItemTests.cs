using System.Linq;
using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class CheckListItemTests
	{
		[Theory]
		[InlineData("- [ ] List item 1", 1)]
		[InlineData("- [ ] List item 1\n- [x] List Item 2", 2)]
		[InlineData("- [ ] List item 1\n- [x] List Item 2\n- [*] List Item 3", 3)]
		[InlineData("- [ ] List item 1\n- [x] List Item 2\n- [*] List Item 3\n- [X] List Item 4", 4)]
		public void ShouldParseSingleLineListItems(string lines, int count)
		{
			var document = Document.Parse(lines);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			var list = (UnorderedList)document[0];

			Assert.True(list.Items.Count == count);
			Assert.True(list.Items.All(i => i.GetType() == typeof(CheckListItem)));

			Assert.False(((CheckListItem)list.Items[0]).Checked);

			if (list.Items.Count > 1)
			{
				for (int i = 1; i < list.Items.Count; i++)
				{
					var item = (CheckListItem)list.Items[i];
					Assert.True(item.Checked);
				}
			}
		}

		[Fact]
		public void ShouldParseChecklistWithInteractiveCheckboxes()
		{
			var document = Document.Parse(@"[%interactive]
* [*] checked
* [x] also checked
* [ ] not checked
*     normal list item
");

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<UnorderedList>(document[0]);

			// TODO: [%interactive] should be parsed onto Unorderedlist
			var unorderList = (UnorderedList)document[0];
			
			Assert.True(unorderList.Items.Count == 4);

			Assert.IsType<CheckListItem>(unorderList.Items[0]);
			Assert.IsType<CheckListItem>(unorderList.Items[1]);
			Assert.IsType<CheckListItem>(unorderList.Items[2]);
			Assert.IsType<UnorderedListItem>(unorderList.Items[3]);		
		}
	}
}