using System.Linq;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class CheckListItemTests
	{
		[Test]
		[TestCase("- [ ] List item 1", 1)]
		[TestCase("- [ ] List item 1\n- [x] List Item 2", 2)]
		[TestCase("- [ ] List item 1\n- [x] List Item 2\n- [*] List Item 3", 3)]
		[TestCase("- [ ] List item 1\n- [x] List Item 2\n- [*] List Item 3\n- [X] List Item 4", 4)]
		public void ShouldParseSingleLineListItems(string lines, int count)
		{
			var document = Document.Parse(lines);

			Assert.IsNotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<UnorderedList>(document[0]);

			var list = (UnorderedList)document[0];

			Assert.IsTrue(list.Items.Count == count);
			Assert.IsTrue(list.Items.All(i => i.GetType() == typeof(CheckListItem)));

			Assert.IsFalse(((CheckListItem)list.Items[0]).Checked);

			if (list.Items.Count > 1)
			{
				for (int i = 1; i < list.Items.Count; i++)
				{
					var item = (CheckListItem)list.Items[i];
					Assert.IsTrue(item.Checked);
				}
			}
		}
	}
}