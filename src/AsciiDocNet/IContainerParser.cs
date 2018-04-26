using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public interface IElementParser
    {
        void Parse(
            Container container,
            IDocumentReader reader,
            Func<string, bool> predicate,
            ref List<string> buffer,
            ref AttributeList attributes);
    }

    public interface IMatchingElementParser : IElementParser
    {
        bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes);
    }
}