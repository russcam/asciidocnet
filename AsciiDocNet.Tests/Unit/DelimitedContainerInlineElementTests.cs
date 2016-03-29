using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public abstract class DelimitedContainerInlineElementTests<TElement> where TElement : IContainerInlineElement
	{
		protected const string Paragraph = "This is a paragraph";

		public abstract string[] Delimiters { get; }

		[Test]
		[TestCaseSource(nameof(Delimiters))]
		public void ShouldParseInlineElement(string delimiter)
		{
			var text = $"{delimiter}{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(1, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[0]);

			var element = (TElement)paragraph.Elements[0];
			Assert.AreEqual(Paragraph, ((TextLiteral)element.Elements[0]).Text);
		}

		[Test]
		[TestCaseSource(nameof(Delimiters))]
		public void ShouldParseInlineElementSurroundedBySpaces(string delimiter)
		{
			var text = $"    {delimiter}{Paragraph}{delimiter}    ";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(3, paragraph.Elements.Count);
			Assert.IsInstanceOf<TElement>(paragraph.Elements[1]);

			var element = (TElement)paragraph.Elements[1];
			Assert.AreEqual(Paragraph, ((TextLiteral)element.Elements[0]).Text);
		}
	}
}