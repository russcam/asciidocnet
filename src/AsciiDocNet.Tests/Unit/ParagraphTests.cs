using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class ParagraphTests
	{
		[Fact]
		public void ShouldParseParagraphWithLiteral()
		{
			var paragraph = "This is a simple paragraph";

			var document = Document.Parse(paragraph);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.Equal(1, element.Count);
			Assert.IsType<TextLiteral>(element[0]);
		}

		[Theory]
		[InlineData("This is a paragraph with a **bold** element")]
		[InlineData("This is a paragraph with a *bold* element")]
		public void ShouldParseParagraphWithBold(string input)
		{
			var document = Document.Parse(input);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.Equal(3, element.Count);
			Assert.IsType<TextLiteral>(element[0]);
			Assert.IsType<Strong>(element[1]);
			Assert.IsType<TextLiteral>(element[2]);
		}

		[Theory]
		[InlineData("This is a paragraph with an __italic__ element")]
		[InlineData("This is a paragraph with an _italic_ element")]
		public void ShouldParseParagraphWithItalic(string input)
		{
			var document = Document.Parse(input);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.Equal(3, element.Count);
			Assert.IsType<TextLiteral>(element[0]);
			Assert.IsType<Emphasis>(element[1]);
			Assert.IsType<TextLiteral>(element[2]);
		}

		[Theory]
		[InlineData("This is a paragraph with a **bold** and an __italic__ element")]
		[InlineData("This is a paragraph with a *bold* and an _italic_ element")]
		public void ShouldParseParagraphWithBoldAndItalic(string input)
		{
			var document = Document.Parse(input);

			Assert.Equal(1, document.Count);
			Assert.IsType<Paragraph>(document[0]);

			var element = (Paragraph)document[0];

			Assert.Equal(5, element.Count);
			Assert.IsType<TextLiteral>(element[0]);
			Assert.IsType<Strong>(element[1]);
			Assert.IsType<TextLiteral>(element[2]);
			Assert.IsType<Emphasis>(element[3]);
			Assert.IsType<TextLiteral>(element[4]);
		}

		[Fact]
		public void ShouldParseParagraphWithAttributeReference()
		{
			var input = "Whenever distances need to be specified, e.g. for a {ref_current}/query-dsl-geo-distance-query.html[geo distance query], the distance unit can be specified as a double number representing distance in meters, as a new instance of a `Distance`, or as a string of the form number and distance unit e.g. \"`2.72km`\"";

			var document = Document.Parse(input);

			AsciiDocAssert.Equal(input, document);
		}
	}
}