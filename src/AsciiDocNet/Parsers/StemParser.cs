namespace AsciiDocNet
{
    public class StemParser : TextBlockerParserBase<Stem>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Pass.IsMatch(reader.Line.AsString()) && attributes.ContainBlockName("stem");
    }
}