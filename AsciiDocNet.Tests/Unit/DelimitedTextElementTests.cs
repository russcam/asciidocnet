using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public abstract class SingleLineAndDelimitedTextElementTests<TElement> : DelimitedTextElementTests<TElement> where TElement : IElement, IText, IAttributable
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
			var element = (TElement)document[0];

			Assert.AreEqual("This is a single line", element.Text);
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

			Assert.AreEqual("This is a single line", element.Text);
		}
	}

	[TestFixture]
	public abstract class DelimitedTextElementTests<TElement> where TElement : IElement, IText, IAttributable
	{
		public virtual string Name { get; } = typeof(TElement).Name.ToLowerInvariant();

		public virtual string Delimiter { get; } = PatternMatcher.GetDelimiterFor<TElement>();

		public string Text => "This is a simple paragraph";

		[Test]
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
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.IsTrue(element.Attributes.HasTitle);
			Assert.AreEqual("Block Title", element.Attributes.Title.Text);
			Assert.IsTrue(element.Attributes.HasAnchor);
			Assert.AreEqual("anchor-id", element.Attributes.Anchor.Id);

			Assert.AreEqual(Text, element.Text);
		}

		[Test]
		public void ShouldParseDelimitedWithParagraph()
		{
			var text = $@"[{Name}]
{Delimiter}
{Text}
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.AreEqual(Text, element.Text);
		}
	}
}