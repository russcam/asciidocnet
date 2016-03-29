using NUnit.Framework;

namespace AsciidocNet.Tests.Unit
{
	[TestFixture]
	public class StrongTests : DelimitedContainerInlineElementTests<Strong>
	{
		public override string[] Delimiters { get; } = { "*", "**" };
	}
}