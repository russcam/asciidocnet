using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class RomanNumeralsTest
	{
		public IEnumerable<int> TestCases => Enumerable.Range(1, 3046);

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void ShouldConvertToRomanNumeralAndBack(int input)
		{
			var numeral = RomanNumerals.ToNumeral(input);
			var converted = RomanNumerals.ToInt(numeral);
			Assert.AreEqual(converted, input);
		}
	}
}