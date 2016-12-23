using System;
using System.IO;

namespace AsciiDocNet
{
    /// <summary>
    /// Reads an Asciidoc document
    /// </summary>
    /// <seealso cref="AsciiDocNet.IDocumentReader" />
    /// <seealso cref="System.IDisposable" />
    public class DocumentReader : IDocumentReader, IDisposable
	{
		private readonly TextReader _reader;
		private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentReader"/> class.
        /// </summary>
        /// <param name="path">The path to the document.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public DocumentReader(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}

			if (path.Length == 0)
			{
				throw new ArgumentException($"{nameof(path)} must have a length");
			}

			var stream = new FileStream(path, FileMode.Open);
			_reader = new StreamReader(stream);
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

			_reader = reader;
		}

        /// <summary>
        /// Gets the current line
        /// </summary>
        public string Line { get; private set; }

        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Gets the path to the source
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Reads the next line, assigning it to <see cref="Line" /> and
        /// incrementing <see cref="LineNumber" />
        /// </summary>
        /// <returns>
        /// the next line
        /// </returns>
        public string ReadLine()
		{
			Line = _reader.ReadLine();
			++LineNumber;
			return Line;
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
		{
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _reader?.Dispose();
            }

            _disposed = true;
		}
	}
}