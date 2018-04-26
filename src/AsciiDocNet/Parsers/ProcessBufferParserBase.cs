using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class ProcessBufferParserBase : DescendingParserBase, IMatchingElementParser
    {
        public abstract bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes);

        public abstract void InternalParse(
            Container container, 
            IDocumentReader reader, 
            Func<string, bool> predicate,
            ref List<string> buffer,
            ref AttributeList attributes);

        public override void Parse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            ProcessParagraph(container, ref buffer, ref attributes);
            InternalParse(container, reader, predicate, ref buffer, ref attributes);
        }
    }
}