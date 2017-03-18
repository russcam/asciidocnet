using System;
using System.Collections.Generic;

namespace AsciiDocNet.Tests.Unit
{
	public class QuotationTests : DelimitedPairContainerInlineElementTests<QuotationMark, QuotationTestsPairs>
	{
	}

	public class QuotationTestsPairs : ReusableClassData
	{
		protected override IEnumerable<object[]> Data 
		{ 
			get 
			{ 
				yield return new object[] { Tuple.Create("\"`", "`\"") };
				yield return new object[] { Tuple.Create("'`", "`'") };
			}
		}
	}
}
