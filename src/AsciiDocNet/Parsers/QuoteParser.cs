namespace AsciiDocNet
{
    public class QuoteParser : BlockParserBase<Quote>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Quote.IsMatch(reader.Line);
    }
}