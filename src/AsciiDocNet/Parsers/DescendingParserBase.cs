using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public abstract class DescendingParserBase : InlineElementParserBase
    {
        private static readonly IMatchingElementParser[] Parsers = {
            new DocumentTitleParser(),
            new TitleParser(),
            new AnchorParser(),
            new ElementAttributeParser(),
            new AttributeEntryParser(),
            new SectionTitleParser(),
            new IncludeParser(),
            new MediaParser(),
            new SingleLineCommentParser(),
            new TableParser(),
            new UnorderedListParser(),
            new CheckListParser(),
            new OrderedListParser(),
            new LabeledListParser(),
            new CommentParser(),
            new ExampleParser(), 
            new FencedParser(), 
            new SourceParser(),
            new ListingParser(),
            new LiteralParser(), 
            new OpenParser(),
            new StemParser(), 
            new PassthroughParser(), 
            new VerseParser(),
            new QuoteParser(),
            new SidebarParser(),
            new AdmonitionParser(),
             
        };

        protected void DescendingParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            while (reader.Line.AsString() != null)
            {
                if (delimiterRegex != null && delimiterRegex.IsMatch(reader.Line.AsString()))
                {
                    ProcessParagraph(container, ref buffer, ref attributes);
                    return;
                }

                var parsed = false;
                for (var index = 0; index < Parsers.Length; index++)
                {
                    var parser = Parsers[index];
                    if (parser.IsMatch(reader, container, attributes))
                    {
                        parser.Parse(container, reader, delimiterRegex, ref buffer, ref attributes);
                        parsed = true;
                        break;
                    }
                }

                if (!parsed)
                {
                    if (PatternMatcher.BlankCharacters.IsMatch(reader.Line.AsString()))
                        ProcessBuffer(container, ref buffer, ref attributes);
                    else
                        buffer.Add(reader.Line.AsString());

                    reader.ReadLine();
                }
            }

            ProcessBuffer(container, ref buffer, ref attributes);
        }

        private void ProcessBuffer(Container container, ref List<string> buffer, ref AttributeList attributes)
        {
            if (buffer.Count > 0)
            {
                if (attributes.ContainBlockName("quote"))
                {
                    ProcessLine<Quote>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("pass"))
                {
                    ProcessSimple<Passthrough>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("example"))
                {
                    ProcessLine<Example>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("stem"))
                {
                    ProcessSimple<Stem>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("sidebar"))
                {
                    ProcessLine<Sidebar>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("source"))
                {
                    ProcessSimple<Source>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("listing"))
                {
                    ProcessSimple<Listing>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("literal"))
                {
                    ProcessSimple<Literal>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("comment"))
                {
                    ProcessSimple<Comment>(container, ref buffer, ref attributes);
                }
                else if (attributes.ContainBlockName("verse"))
                {
                    ProcessLine<Verse>(container, ref buffer, ref attributes);
                }
                else
                {
                    ProcessParagraph(container, ref buffer, ref attributes);
                }

                buffer = new List<string>(8);
            }

            attributes = null;
        }

        private void ProcessLine<TElement>(Container container, ref List<string> buffer, ref AttributeList attributes)
            where TElement : Container, IElement, IAttributable, new()
        {
            var element = new TElement();
            element.Attributes.Add(attributes);
            ProcessParagraph(element, ref buffer);
            container.Add(element);
        }

        // TODO: Simple elements i.e. those that simply have a verbatim string value, should probably still take a paragraph or literal
        private static void ProcessSimple<TElement>(Container container, ref List<string> buffer, ref AttributeList attributes)
            where TElement : IElement, IText, IAttributable, new()
        {
            var element = new TElement { Text = string.Join(Environment.NewLine, buffer) };
            element.Attributes.Add(attributes);
            container.Add(element);
        }
    }
}