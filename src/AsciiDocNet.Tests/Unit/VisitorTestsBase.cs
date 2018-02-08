using System.IO;
using System.Text;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class VisitorTestsBase
	{
		protected VisitorTestsBase()
		{
			Builder = new StringBuilder();
			Visitor = new AsciiDocVisitor(new StringWriter(Builder));
		}

		protected AsciiDocVisitor Visitor { get; }

		protected StringBuilder Builder { get; }
	}
}