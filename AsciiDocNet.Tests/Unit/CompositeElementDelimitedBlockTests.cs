using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public abstract class CompositeElementDelimitedBlockTests<TElement> where TElement : Container, IElement, IAttributable
	{
		public virtual string Name { get; } = typeof(TElement).Name.ToLowerInvariant();

		public virtual string Delimiter { get; } = PatternMatcher.GetDelimiterFor<TElement>();

		[Test]
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
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.IsTrue(element.Attributes.HasTitle);
			Assert.AreEqual("Block Title", element.Attributes.Title.Text);
			Assert.IsTrue(element.Attributes.HasAnchor);
			Assert.AreEqual("anchor-id", element.Attributes.Anchor.Id);

			Assert.IsTrue(element.Count == 1);
			Assert.IsInstanceOf<Paragraph>(element[0]);
		}

		[Test]
		public void ShouldParseDelimitedWithParagraph()
		{
			var text = $@"[{Name}]
{Delimiter}
This is a simple paragraph
{Delimiter}
";
			var document = Document.Parse(text);

			Assert.NotNull(document);
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.IsTrue(element.Count == 1);
			Assert.IsInstanceOf<Paragraph>(element[0]);
		}

		[Test]
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
			Assert.IsTrue(document.Count == 1);

			Assert.IsInstanceOf<TElement>(document[0]);
			var element = (TElement)document[0];

			Assert.IsTrue(element.Count == 2);
			Assert.IsInstanceOf<Paragraph>(element[0]);
			Assert.IsInstanceOf<Fenced>(element[1]);
		}
	}
}