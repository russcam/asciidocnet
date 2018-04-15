using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class AdmonitionParser : ProcessBufferParserBase
    {
	    private static readonly ReadOnlySpan<char>[] Admonitions = 
		    Patterns.Admonitions.OrderBy(a => a.Length).Select(a => a.AsSpan()).ToArray();
	    
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes)
        {
	        if (reader.Line == null)
		        return false;

	        if (reader.Line.Value.Length < Admonitions[0].Length + 3)
		        return false;

	        for (var i = 0; i < Admonitions.Length; i++)
	        {
		        var admonition = Admonitions[0];
		        if (reader.Line.Value.StartsWith(admonition) &&
		            reader.Line.Value[admonition.Length] == ':' &&
		            char.IsWhiteSpace(reader.Line.Value[admonition.Length + 1]) &&
		            !char.IsWhiteSpace(reader.Line.Value[admonition.Length + 2]))
			        return true;
	        }

	        return false;
        }

	    protected override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Admonition.Match(reader.Line.AsString());

            if (!match.Success)
            {
                throw new ArgumentException("not an admonition");
            }

            buffer.Add(match.Groups["text"].Value);
            reader.ReadLine();
            while (reader.Line.AsString() != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()))
            {
                buffer.Add(reader.Line.AsString());
                reader.ReadLine();
            }

            var admonition = new Admonition(match.Groups["style"].Value.ToEnum<AdmonitionStyle>());
            admonition.Attributes.Add(attributes);
            ProcessParagraph(admonition, ref buffer);
            container.Add(admonition);
            attributes = null;

            reader.ReadLine();
        }
    }
}