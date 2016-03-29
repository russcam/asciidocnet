using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class ParagraphVisitorTests : VisitorTestsBase
	{
		[Test]
		[TestCase("This is a simple paragraph")]
		[TestCase("This is a paragraph with a **bold** element")]
		[TestCase("This is a paragraph with a *bold* element")]
		[TestCase("This is a paragraph with an __italic__ element")]
		[TestCase("This is a paragraph with an _italic_ element")]
		[TestCase("This is a paragraph with a **bold** and an __italic__ element")]
		[TestCase("This is a paragraph with a *bold* and an _italic_ element")]
		public void OutputShouldMatchInput(string input)
		{
			var document = Document.Parse(input);
			AsciiDocAssert.AreEqual(input, document);
		}
	}
}