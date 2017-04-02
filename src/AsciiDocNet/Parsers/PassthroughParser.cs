namespace AsciiDocNet
{
    public class PassthroughParser : TextBlockerParserBase<Passthrough>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Pass.IsMatch(reader.Line);
    }
}