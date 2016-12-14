using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class CompositeElementDelimitedBlockTests<TElement> where TElement : Container, IElement, IAttributable
	{
		public virtual string Name { get; } = typeof(TElement).Name.ToLowerInvariant();

		public virtual string Delimiter { get; } = PatternMatcher.GetDelimiterFor<TElement>();

		[Fact]
		public void ShouldParseDelimitedWithAnchorAndTitleAndStyle()
		{
			var text = $@".Block Title
[[anchor-id]]
[{Name}]
{Delimiter}
This is a simple paragraph
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.True(element.Attributes.HasTitle);
			Assert.Equal("Block Title", element.Attributes.Title.Text);
			Assert.True(element.Attributes.HasAnchor);
			Assert.Equal("anchor-id", element.Attributes.Anchor.Id);

			Assert.True(element.Count == 1);
			Assert.IsType<Paragraph>(element[0]);
		}

		[Fact]
		public void ShouldParseDelimitedWithParagraph()
		{
			var text = $@"[{Name}]
{Delimiter}
This is a simple paragraph
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.True(element.Count == 1);
			Assert.IsType<Paragraph>(element[0]);
		}

		[Fact]
		public void ShouldParseDelimitedWithParagraphAndListing()
		{
			var text = $@"[{Name}]
{Delimiter}
This is a simple paragraph
```
This is a fenced block
```
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.True(element.Count == 2);
			Assert.IsType<Paragraph>(element[0]);
			Assert.IsType<Fenced>(element[1]);
		}
	}
}