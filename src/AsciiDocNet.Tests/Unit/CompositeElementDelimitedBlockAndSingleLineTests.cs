using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class CompositeElementDelimitedBlockAndSingleLineTests<TElement> :
		CompositeElementDelimitedBlockTests<TElement> where TElement : Container, IElement, IAttributable
	{
		[Fact]
		public void ShouldParseSingleLine()
		{
			var text = $@"[{Name}]
This is a single line
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var example = (TElement)document[0];

			Assert.True(example.Count == 1);
			Assert.IsType<Paragraph>(example[0]);
		}

		[Fact]
		public void ShouldParseSingleLineWithTitle()
		{
			var text = $@".Block Title
[{Name}]
This is a single line
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.True(element.Attributes.HasTitle);
			Assert.Equal("Block Title", element.Attributes.Title.Text);

			Assert.True(element.Count == 1);
			Assert.IsType<Paragraph>(element[0]);
		}
	}
}