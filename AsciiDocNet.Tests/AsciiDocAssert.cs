using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AsciiDocNet.Tests
{
	public class AsciiDocAssert
	{
		public static void Equal(string asciidoc, Document document)
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

			Assert.Equal(asciidoc, builder.ToString().TrimEnd('\r', '\n'));
		}
	}
}