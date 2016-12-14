using Xunit;

namespace AsciiDocNet.Tests.Unit
{
	public class InlineTests
	{
		[Fact]
		public void Test()
		{
			var input = "This is a paragraph with **strong** and _emphasis_ text with `monospace` too!";

			var document = Document.Parse(input);

		}
	}
}