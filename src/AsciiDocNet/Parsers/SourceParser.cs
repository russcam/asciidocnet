namespace AsciiDocNet
{
    public class SourceParser : ListingParserBase<Source>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Listing.IsMatch(reader.Line) && attributes.ContainBlockName("source");
    }
}