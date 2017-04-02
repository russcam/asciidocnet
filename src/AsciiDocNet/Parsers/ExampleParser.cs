namespace AsciiDocNet
{
    public class ExampleParser : BlockParserBase<Example>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Example.IsMatch(reader.Line);
    }
}