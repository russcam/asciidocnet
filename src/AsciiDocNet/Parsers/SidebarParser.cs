namespace AsciiDocNet
{
    public class SidebarParser : BlockParserBase<Sidebar>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Sidebar.IsMatch(reader.Line);
    }
}