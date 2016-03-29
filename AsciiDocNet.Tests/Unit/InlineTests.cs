using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class InlineTests
	{

		[Test]
		public void Test()
		{
			var input = "This is a paragraph with **strong** and _emphasis_ text with `monospace` too!";

			var document = Document.Parse(input);

		}
	}
}