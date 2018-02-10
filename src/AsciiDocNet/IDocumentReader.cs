using System;

namespace AsciiDocNet
{
    /// <summary>
    /// Reads an Asciidoc document
    /// </summary>
    public interface IDocumentReader
	{
		/// <summary>
		/// The path to the source from which lines are read
		/// </summary>
		string Path { get; }

		/// <summary>
		/// The current line
		/// </summary>
		ReadOnlySpan<char>? Line { get; }
		
		/// <summary>
		/// The current position
		/// </summary>
		int Position { get; }

		/// <summary>
		/// The current line number
		/// </summary>
		int LineNumber { get; }

		/// <summary>
		/// Reads the next line, assigning it to <see cref="Line"/> and
		/// incrementing <see cref="LineNumber"/>
		/// </summary>
		/// <returns>the next line</returns>
		ReadOnlySpan<char>? ReadLine();
	}
}