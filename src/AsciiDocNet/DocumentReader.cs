using System;
using System.IO;

namespace AsciiDocNet
{
    /// <summary>
    /// Reads an Asciidoc document
    /// </summary>
    /// <seealso cref="AsciiDocNet.IDocumentReader" />
    public class DocumentReader : IDocumentReader
    {
	    private const char ReturnCharacter = '\r';
	    private const char NewlineCharacter = '\n';
	    private readonly string _document;
	    private ReadOnlySpan<char> _span;

	    /// <summary>
        /// Initializes a new instance of the <see cref="DocumentReader"/> class.
        /// </summary>
        /// <param name="path">The path to the document.</param>
        /// <exception cref="System.ArgumentNullException">path is null</exception>
        /// <exception cref="System.ArgumentException">path is empty</exception>
        public DocumentReader(string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			if (path.Length == 0)
				throw new ArgumentException($"{nameof(path)} must have a length");

			_document = File.ReadAllText(path);
			_span = _document.AsSpan();
            Path = path;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public DocumentReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			_document = reader.ReadToEnd();
			_span = _document.AsSpan();
		}

	    /// <summary>
	    /// Gets the current line
	    /// </summary>
	    public ReadOnlySpan<char>? Line { get; private set; }

	    /// <summary>
        /// Gets the current line number
        /// </summary>
        public int LineNumber { get; private set; }

	    /// <summary>
	    /// Gets the current position
	    /// </summary>
	    public int Position { get; private set; }

	    /// <summary>
        /// Gets the path to the source
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Reads the next line, assigning it to <see cref="Line" /> and
        /// incrementing <see cref="LineNumber" />
        /// </summary>
        /// <returns>
        /// the next line
        /// </returns>
        public ReadOnlySpan<char>? ReadLine()
        {
	        if (Position >= _span.Length)
	        {
		        Line = null;
		        return Line;
	        }

	        var startPosition = Position;
	        var position = Position;
	        for (; position < _span.Length; position++)
	        {
		        var c = _span[position];
		        if (c == ReturnCharacter || c == NewlineCharacter)
		        {
			        var end = position;
			        // is this an \r\n?
			        if (c == ReturnCharacter && position + 1 < _span.Length && _span[position + 1] == NewlineCharacter)
			        {
				        position++;
			        }
			        position++;
			        Position = position;
			        LineNumber++;
			        Line = _span.Slice(startPosition, end - startPosition);
			        return Line;
		        }
	        }

	        Position = position;
	        Line = _span.Slice(startPosition, position - startPosition);
	        return Line;
        }
	}
}