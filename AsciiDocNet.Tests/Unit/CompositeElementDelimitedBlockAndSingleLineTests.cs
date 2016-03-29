using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public abstract class CompositeElementDelimitedBlockAndSingleLineTests<TElement> :
		CompositeElementDelimitedBlockTests<TElement> where TElement : Container, IElement, IAttributable
	{
		[Test]
		public void ShouldParseSingleLine()
		{
			var text = $@"[{Name}]
This is a single line
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var example = (TElement)document[0];

			Assert.IsTrue(example.Count == 1);
			Assert.IsInstanceOf<Paragraph>(example[0]);
		}

		[Test]
		public void ShouldParseSingleLineWithTitle()
		{
			var text = $@".Block Title
[{Name}]
This is a single line
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.IsTrue(element.Attributes.HasTitle);
			Assert.AreEqual("Block Title", element.Attributes.Title.Text);

			Assert.IsTrue(element.Count == 1);
			Assert.IsInstanceOf<Paragraph>(element[0]);
		}
	}
}