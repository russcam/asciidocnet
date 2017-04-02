namespace AsciiDocNet
{
    public class ListingParser : ListingParserBase<Listing>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Listing.IsMatch(reader.Line);
    }
}