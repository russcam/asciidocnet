using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class ParagraphTests
	{
		[Test]
		public void ShouldParseParagraphWithLiteral()
		{
			var paragraph = "This is a simple paragraph";

			var document = Document.Parse(paragraph);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.AreEqual(1, element.Elements.Count);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[0]);
		}

		[Test]
		[TestCase("This is a paragraph with a **bold** element")]
		[TestCase("This is a paragraph with a *bold* element")]
		public void ShouldParseParagraphWithBold(string input)
		{
			var document = Document.Parse(input);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.AreEqual(3, element.Elements.Count);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[0]);
			Assert.IsInstanceOf<Strong>(element.Elements[1]);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[2]);
		}

		[Test]
		[TestCase("This is a paragraph with an __italic__ element")]
		[TestCase("This is a paragraph with an _italic_ element")]
		public void ShouldParseParagraphWithItalic(string input)
		{
			var document = Document.Parse(input);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.AreEqual(3, element.Elements.Count);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[0]);
			Assert.IsInstanceOf<Emphasis>(element.Elements[1]);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[2]);
		}

		[Test]
		[TestCase("This is a paragraph with a **bold** and an __italic__ element")]
		[TestCase("This is a paragraph with a *bold* and an _italic_ element")]
		public void ShouldParseParagraphWithBoldAndItalic(string input)
		{
			var document = Document.Parse(input);

			Assert.AreEqual(1, document.Count);
			Assert.IsInstanceOf<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.AreEqual(5, element.Elements.Count);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[0]);
			Assert.IsInstanceOf<Strong>(element.Elements[1]);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[2]);
			Assert.IsInstanceOf<Emphasis>(element.Elements[3]);
			Assert.IsInstanceOf<TextLiteral>(element.Elements[4]);
		}

		[Test]
		public void ShouldParseParagraphWithAttributeReference()
		{
			var input = "Whenever distances need to be specified, e.g. for a {ref_current}/query-dsl-geo-distance-query.html[geo distance query], the distance unit can be specified as a double number representing distance in meters, as a new instance of a `Distance`, or as a string of the form number and distance unit e.g. \"`2.72km`\"";

			var document = Document.Parse(input);

			AsciiDocAssert.AreEqual(input, document);
		}
	}
}