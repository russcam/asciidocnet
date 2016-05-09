using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public abstract class DelimitedContainerInlineElementTests<TElement> where TElement : InlineContainer
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

			Assert.AreEqual(1, paragraph.Count);
			Assert.IsInstanceOf<TElement>(paragraph[0]);

			var element = (TElement)paragraph[0];
			Assert.AreEqual(Paragraph, ((TextLiteral)element[0]).Text);
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

			Assert.AreEqual(3, paragraph.Count);
			Assert.IsInstanceOf<TElement>(paragraph[1]);

			var element = (TElement)paragraph[1];
			Assert.AreEqual(Paragraph, ((TextLiteral)element[0]).Text);
		}
	}
}