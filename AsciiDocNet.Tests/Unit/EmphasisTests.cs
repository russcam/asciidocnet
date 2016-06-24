using System.Collections.Generic;

namespace AsciiDocNet.Tests.Unit
{
	public class EmphasisTests : DelimitedInlineElementTests<Emphasis, EmphasisTestsDelimiters>
	{
	}

    public class EmphasisTestsDelimiters : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{ 
				yield return new object[] { "_" };
				yield return new object[] { "__" };
			}
		}
	}
}
