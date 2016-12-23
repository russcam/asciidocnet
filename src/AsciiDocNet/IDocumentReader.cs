using System;

namespace AsciiDocNet
{
	public interface IDocumentReader
	{
		/// <summary>
		/// The path to the source
		/// </summary>
		string Path { get; }

		/// <summary>
		/// The current line
		/// </summary>
		string Line { get; set; }

		/// <summary>
		/// The current line number
		/// </summary>
		int LineNumber { get; }

		/// <summary>
		/// Reads the next line, assigning it to <see cref="Line"/> and
		/// incrementing <see cref="LineNumber"/>
		/// </summary>
		/// <returns>the next line</returns>
		string ReadLine();

		/// <summary>
		/// Peeks at the next line
		/// </summary>
		/// <returns></returns>
		string PeekLine();

		/// <summary>
		/// Keeps reading the next line until the predicate is
		/// matched
		/// </summary>
		/// <param name="predicate">The predicate</param>
		/// <returns>The number of lines read</returns>
		int ReadWhile(Func<string, bool> predicate);
	}
}