using System.IO;
using System.Text;
using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	public abstract class VisitorTestsBase
	{
		[SetUp]
		public void VisitorTestsBaseSetup()
		{
			Builder = new StringBuilder();
			Visitor = new AsciiDocVisitor(new StringWriter(Builder));
		}

		public AsciiDocVisitor Visitor { get; private set; }

		public StringBuilder Builder { get; private set; }
	}
}