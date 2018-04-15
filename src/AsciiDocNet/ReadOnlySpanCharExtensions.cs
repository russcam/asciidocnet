using System;
using System.Collections.Generic;

namespace AsciiDocNet
{
	public static class ReadOnlySpanCharExtensions
	{
		/// <summary>
		/// New instance of a string from a <see cref="ReadOnlySpan{Char}"/>
		/// </summary>
		/// <param name="span">the span</param>
		/// <returns>a new instance of a string</returns>
		public static string AsString(this ReadOnlySpan<char>? span)
		{
			return span == null
				? null
				: new string(span.Value.ToArray());
		}
		
		public static bool IsWhitespace(this ReadOnlySpan<char> span)
		{
			for (var i = 0; i < span.Length; i++)
			{
				var ch = span[i];
				if (char.IsWhiteSpace(ch)) //ch != ' ' && ch != '\t')
					return false;
			}

			return true;
		}
		
		public static bool IsEqual(this ReadOnlySpan<char> span, string other) => 
			span.SequenceEqual(other.AsSpan());
	}
}