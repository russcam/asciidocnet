using System;

namespace AsciiDocNet
{
    public class VerseParser : BlockParserBase<Verse>
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) => 
	        reader.Line != null && 
		    reader.Line.Value.Length >= 4 && 
		    (reader.Line.Value.Length == 4
		 	   ? reader.Line.Value.Slice(0, 4).IsEqual(Patterns.Block.Verse)
		 	   : reader.Line.Value.Slice(0, 4).IsEqual(Patterns.Block.Verse) &&
		 		 reader.Line.Value.Slice(4).IsWhitespace()) && 
		    attributes.ContainBlockName("verse");
    }
}