using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    /// <summary>
    /// Parses text from an <see cref="IDocumentReader"/> into an <see cref="Document"/>
    /// </summary>
    /// <seealso cref="AsciiDocNet.IParser" />
    public class Parser : DescendingParserBase, IParser
    {
        /// <summary>
        /// Parses text from <see cref="IDocumentReader"/> into an <see cref="Document"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An new instance of <see cref="Document"/></returns>
        public Document Parse(IDocumentReader reader)
        {
            var document = new Document(reader.Path);
            var buffer = new List<string>(8);
            AttributeList attributes = null;
            Parse(document, reader, null, ref buffer, ref attributes);
            return document;
        }

        public override void Parse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer, ref AttributeList attributes)
        {
            reader.ReadLine();        
            DescendingParse(container, reader, predicate, ref buffer, ref attributes);
        }
    }
}