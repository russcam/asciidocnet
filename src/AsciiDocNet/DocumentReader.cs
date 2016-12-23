using System;
using System.Collections.Generic;
using System.IO;

namespace AsciiDocNet
{
	public class DocumentReader : IDocumentReader, IDisposable
	{
		private readonly TextReader _reader;
		private bool _disposed;
		private Queue<string> _buffer;

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
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"No file found at {path}");
			}

			_reader = new StreamReader(new FileStream(path, FileMode.Open));
		}

		public DocumentReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			_reader = reader;
		}

		public string Line { get; set; }

		public int LineNumber { get; private set; }

		public string Path { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public string ReadLine()
		{
			if (_buffer != null && _buffer.Count > 0)
			{
				Line = _buffer.Dequeue();
			}
			else
			{
				Line = _reader.ReadLine();
			}

			++LineNumber;
			return Line;
		}

		public string PeekLine()
		{
			if (_buffer != null && _buffer.Count > 0)
			{
				return _buffer.Peek();
			}

			var line = _reader.ReadLine();

			if (_buffer == null)
			{
				_buffer = new Queue<string>();
			}

			_buffer.Enqueue(line);
			return line;
		}

		public int ReadWhile(Func<string, bool> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			string line;
			int count = 0;
			while ((line = ReadLine()) != null && !predicate(line))
			{
				++LineNumber;
				++count;
			}
			Line = line;
			return count;
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