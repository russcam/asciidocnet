using System;
using System.IO;

namespace AsciiDocNet
{
	public class DocumentReader : IDocumentReader, IDisposable
	{
		private readonly TextReader _reader;
		private bool _disposed;

		public DocumentReader(string path)
		{
			if (path == null)
			{
				throw new ArgumentException($"{nameof(path)} cannot be null");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException($"{nameof(path)} must have a length");
			}

			var stream = new FileStream(path, FileMode.Open);
			_reader = new StreamReader(stream);
		}

		public DocumentReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			_reader = reader;
		}

		public string Line { get; private set; }

		public int LineNumber { get; private set; }

		public string Path { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public string ReadLine()
		{
			Line = _reader.ReadLine();
			++LineNumber;
			return Line;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_reader?.Dispose();
				}

				_disposed = true;
			}
		}
	}
}