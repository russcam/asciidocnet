using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class DocumentTests : VisitorTestsBase
	{
		public IEnumerable<FileInfo> Files
		{
			get
			{
				return Directory.GetFiles(@"C:\Users\russ\source\elasticsearch-net-1.x\docs\asciidoc", "*.asciidoc", SearchOption.AllDirectories)
					.Select(f => new FileInfo(f));
			}
		}

		[Test]
		[TestCaseSource(nameof(Files))]
		public void ShouldParseDocument(FileInfo file)
		{
			var document = Document.Load(file.FullName);
			document.Accept(Visitor);
			File.WriteAllText(file.Name, Builder.ToString());
		}
	}
}