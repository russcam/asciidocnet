using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AsciidocNet.Tests
{
	public class AsciiDocAssert
	{
		public static void AreEqual(string asciidoc, Document document)
		{
			var directoryAttribute = document.Attributes.FirstOrDefault(a => a.Name == "docdir");
			if (directoryAttribute != null)
			{
				document.Attributes.Remove(directoryAttribute);
			}

			var builder = new StringBuilder();
			using (var visitor = new AsciiDocVisitor(new StringWriter(builder)))
			{
				document.Accept(visitor);
			}

			Assert.AreEqual(asciidoc, builder.ToString().TrimEnd('\r', '\n'));
		}
	}
}