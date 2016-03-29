using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class StrongTests : DelimitedContainerInlineElementTests<Strong>
	{
		public override string[] Delimiters { get; } = { "*", "**" };
	}
}