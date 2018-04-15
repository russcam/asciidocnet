using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class ProcessBufferParserBase : InlineElementParserBase, IMatchingElementParser
    {
        public abstract bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes);

	    protected abstract void InternalParse(
            Container container, 
            IDocumentReader reader, 
            Regex delimiterRegex,
            ref List<string> buffer,
            ref AttributeList attributes);

        public override void Parse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            ProcessParagraph(container, ref buffer, ref attributes);
            InternalParse(container, reader, delimiterRegex, ref buffer, ref attributes);
        }
    }
}