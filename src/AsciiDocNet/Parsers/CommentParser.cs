namespace AsciiDocNet
{
    public class CommentParser : TextBlockerParserBase<Comment>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.CommentBlock.IsMatch(reader.Line);
    }
}