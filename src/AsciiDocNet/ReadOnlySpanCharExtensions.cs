using System;

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
	}
}