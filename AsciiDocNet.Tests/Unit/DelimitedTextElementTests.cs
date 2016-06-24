using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class SingleLineAndDelimitedTextElementTests<TElement> : DelimitedTextElementTests<TElement> where TElement : IElement, IText, IAttributable
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
			var element = (TElement)document[0];

			Assert.Equal("This is a single line", element.Text);
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

			Assert.Equal("This is a single line", element.Text);
		}
	}

	public abstract class DelimitedTextElementTests<TElement> where TElement : IElement, IText, IAttributable
	{
		public virtual string Name { get; } = typeof(TElement).Name.ToLowerInvariant();

		public virtual string Delimiter { get; } = PatternMatcher.GetDelimiterFor<TElement>();

		public string Text => "This is a simple paragraph";

		[Fact]
		public void ShouldParseDelimitedWithAnchorAndTitleAndStyle()
		{
			var text = $@".Block Title
[[anchor-id]]
[{Name}]
{Delimiter}
{Text}
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

			Assert.Equal(Text, element.Text);
		}

		[Fact]
		public void ShouldParseDelimitedWithParagraph()
		{
			var text = $@"[{Name}]
{Delimiter}
{Text}
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.True(document.Count == 1);

			Assert.IsType<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.Equal(Text, element.Text);
		}
	}
}