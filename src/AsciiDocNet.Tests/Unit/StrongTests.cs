using System.Collections.Generic;

namespace AsciiDocNet.Tests.Unit
{
	public class StrongTests : DelimitedContainerInlineElementTests<Strong, StrongTestsDelimiters>
	{
	}

    public class StrongTestsDelimiters : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{ 
				yield return new object[] { "*" };
				yield return new object[] { "**" };
			}
		}
	}
}
