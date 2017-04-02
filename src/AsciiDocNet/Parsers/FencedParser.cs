namespace AsciiDocNet
{
    public class FencedParser : TextBlockerParserBase<Fenced>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Fenced.IsMatch(reader.Line);
    }
}