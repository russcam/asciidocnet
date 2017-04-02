namespace AsciiDocNet
{
    public class OpenParser : BlockParserBase<Open>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.OpenBlock.IsMatch(reader.Line);
    }
}