namespace AsciiDocNet
{
    public class LiteralParser : TextBlockerParserBase<Literal>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Literal.IsMatch(reader.Line);
    }
}