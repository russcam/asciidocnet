using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class ParagraphVisitorTests : VisitorTestsBase
	{
		[Theory]
		[InlineData("This is a simple paragraph")]
		[InlineData("This is a paragraph with a **bold** element")]
		[InlineData("This is a paragraph with a *bold* element")]
		[InlineData("This is a paragraph with an __italic__ element")]
		[InlineData("This is a paragraph with an _italic_ element")]
		[InlineData("This is a paragraph with a **bold** and an __italic__ element")]
		[InlineData("This is a paragraph with a *bold* and an _italic_ element")]
		public void OutputShouldMatchInput(string input)
		{
			var document = Document.Parse(input);
			AsciiDocAssert.Equal(input, document);
		}
	}
}