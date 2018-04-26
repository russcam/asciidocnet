using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class AnchorParser : IMatchingElementParser
    {
	    private static readonly ReadOnlySpan<char> OpeningCharacters = "[[".AsSpan();
	    private static readonly ReadOnlySpan<char> ClosingCharacters = "]]".AsSpan();
	    
        public bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes)
        {
	        if (reader.Line == null)
		        return false;

	        var span = reader.Line.Value;

	        if (!span.StartsWith(OpeningCharacters) || !span.EndsWith(ClosingCharacters))
		        return false;

	        var contents = span.Slice(OpeningCharacters.Length, span.Length - ClosingCharacters.Length);
	        return contents.Length > 0 && char.IsLetter(contents[0]);
	    }

	    public void Parse(
            Container container,
            IDocumentReader reader,
            Regex delimiterRegex,
            ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Anchor.Match(reader.Line.AsString());
            if (!match.Success)
            {
                throw new ArgumentException("not an anchor");
            }

            var id = match.Groups["id"].Value;

            var reference = !string.IsNullOrEmpty(match.Groups["reference"].Value) 
                ? match.Groups["reference"].Value 
                : null;

            var anchor = new Anchor(id, reference);

            if (attributes != null)
            {
                attributes.Add(anchor);
            }
            else
            {
                attributes = new AttributeList { anchor };
            }

            reader.ReadLine();
        }
    }
}