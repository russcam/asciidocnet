namespace AsciiDocNet
{
    public class VerseParser : BlockParserBase<Verse>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Verse.IsMatch(reader.Line) && attributes.ContainBlockName("verse");
    }
}