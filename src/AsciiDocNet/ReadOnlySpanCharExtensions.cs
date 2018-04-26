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
		public static string AsString(this ReadOnlySpan<char>? span) => 
			span == null ? null : AsString(span.Value);

		public static string AsString(this ReadOnlySpan<char> span) => 
			new string(span.ToArray());

		public static bool IsWhitespace(this ReadOnlySpan<char> span)
		{
			for (var i = 0; i < span.Length; i++)
			{
				var ch = span[i];
				if (!char.IsWhiteSpace(ch)) //TODO: ch != ' ' && ch != '\t') ?
					return false;
			}

			return true;
		}

		public static bool EndsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> end) => 
			end.Length == 0 || 
			end.Length <= span.Length && span.Slice(span.Length - end.Length).SequenceEqual(end);

		public static bool IsEqual(this ReadOnlySpan<char> span, ReadOnlySpan<char> other) => 
			span.SequenceEqual(other);

		public static bool IsNotEqual(this ReadOnlySpan<char> span, ReadOnlySpan<char> other) =>
			!IsEqual(span, other);
	}
}