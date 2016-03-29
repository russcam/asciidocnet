using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciidocNet
{
	public static class RomanNumerals
	{
		private static readonly string[][] RomanNumeralsStrings =
		{
			new[] { string.Empty, "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" },
			new[] { string.Empty, "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" },
			new[] { string.Empty, "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" },
			new[] { string.Empty, "M", "MM", "MMM" }
		};

		private static readonly Dictionary<char, int> RomanNumeralIntegers = new Dictionary<char, int>()
		{
			{ 'I', 1 },
			{ 'i', 1 },
			{ 'V', 5 },
			{ 'v', 5 },
			{ 'X', 10 },
			{ 'x', 10 },
			{ 'L', 50 },
			{ 'l', 50 },
			{ 'C', 100 },
			{ 'c', 100 },
			{ 'D', 500 },
			{ 'd', 500 },
			{ 'M', 1000 },
			{ 'm', 1000 }
		};

		public static string ToNumeral(int value)
		{
			if (value > 3046)
			{
				// TODO: handle this better...
				throw new ArgumentOutOfRangeException(nameof(value), "must be less than 3046");
			}

			var intArray = value.ToString().Reverse().ToArray();
			var len = intArray.Length;
			var romanNumeral = new StringBuilder(16);
			var i = len;
			while (i-- > 0)
			{
				romanNumeral.Append(RomanNumeralsStrings[i][int.Parse(intArray[i].ToString())]);
			}

			return romanNumeral.ToString();
		}

		public static int ToInt(string value)
		{
			int number = 0;
			int thisValue = 0;
			int nextValue = 0;

			for (var i = 0; i < value.Length; i++)
			{
				if (i == 0)
				{
					if (!RomanNumeralIntegers.TryGetValue(value[i], out thisValue))
					{
						throw new ArgumentException("not a valid roman numeral");
					}
				}

				var notLastChar = i + 1 < value.Length;
				if (notLastChar)
				{
					if (!RomanNumeralIntegers.TryGetValue(value[i + 1], out nextValue))
					{
						throw new ArgumentException("not a valid roman numeral");
					}
				}

				number = notLastChar && thisValue < nextValue ? number - thisValue : number + thisValue;
				thisValue = nextValue;
			}

			return number;
		}
	}
}