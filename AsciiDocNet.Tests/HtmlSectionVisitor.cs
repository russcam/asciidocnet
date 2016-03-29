using System.IO;

namespace AsciidocNet.Tests
{
	public class HtmlSectionVisitor : HtmlVisitor
	{
		public HtmlSectionVisitor(TextWriter writer) : base(writer)
		{
		}

		public override void Visit(Document document)
		{
			foreach (var attribute in document.Attributes)
			{
				Visit(attribute);
			}
			Visit((Container)document);
		}
	}
}