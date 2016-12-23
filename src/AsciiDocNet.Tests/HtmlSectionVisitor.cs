using System.IO;

namespace AsciiDocNet.Tests
{
	public class HtmlSectionVisitor : HtmlVisitor
	{
		public HtmlSectionVisitor(TextWriter writer) : base(writer)
		{
		}

		public override void VisitDocument(Document document)
		{
			foreach (var attribute in document.Attributes)
			{
				VisitAttributeEntry(attribute);
			}
			VisitContainer(document);
		}
	}
}