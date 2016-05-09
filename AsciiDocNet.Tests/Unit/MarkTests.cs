using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class MarkTests : DelimitedContainerInlineElementTests<Mark>
	{
		public override string[] Delimiters { get; } = { "#", "##" };

		[Test]
		[TestCaseSource(nameof(Delimiters))]
		public void ShouldParseMarkWithStyle(string delimiter)
		{
			var text = $"[green]{delimiter}{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(1, paragraph.Count);
			Assert.IsInstanceOf<Mark>(paragraph[0]);

			var element = (Mark)paragraph[0];
			Assert.AreEqual(Paragraph, ((TextLiteral)element[0]).Text);
			Assert.AreEqual("green", ((RoleAttribute)element.Attributes[0]).Value);
		}

		[Test]
		[TestCaseSource(nameof(Delimiters))]
		public void ShouldNotParseMarkFromSingleDelimiter(string delimiter)
		{
			var text = $"{Paragraph}{delimiter}";
			var document = Document.Parse(text);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var paragraph = (Paragraph)document[0];

			Assert.AreEqual(1, paragraph.Count);
			Assert.IsInstanceOf<TextLiteral>(paragraph[0]);

			var element = (TextLiteral)paragraph[0];
			Assert.AreEqual(Paragraph + delimiter, element.Text);
		}
	}
}