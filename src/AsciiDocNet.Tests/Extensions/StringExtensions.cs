using System;
using System.Text.RegularExpressions;

namespace AsciiDocNet.Tests.Extensions
{
	public static class StringExtensions
	{
		private static readonly Regex MultiReturn = new Regex("\r{2,}", RegexOptions.Compiled | RegexOptions.Multiline);
		private static readonly Regex NewLine = new Regex("(?<!\r)\n", RegexOptions.Compiled | RegexOptions.Multiline);

		public static string ConsistentLineEndings(this string input)
		{
			var replaceNewLines = NewLine.Replace(input, Environment.NewLine);
			return MultiReturn.Replace(replaceNewLines, "\r");
		}

		public static string RemoveTrailingNewLine(this string input) => input.TrimEnd('\r', '\n');
	}
}
