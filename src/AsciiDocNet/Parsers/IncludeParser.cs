using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class IncludeParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Include.IsMatch(reader.Line);


        public override void InternalParse(Container container, IDocumentReader reader, Func<string, bool> predicate, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Include.Match(reader.Line);
            if (!match.Success)
            {
                throw new ArgumentException("not an include");
            }

            var include = new Include(match.Groups["path"].Value);
            var attributesValue = match.Groups["attributes"].Value;

            if (!string.IsNullOrEmpty(attributesValue))
            {
                var attributeValues = SplitOnCharacterOutsideQuotes(attributesValue);
                foreach (var attributeValue in attributeValues)
                {
                    var attributeMatch = PatternMatcher.AttributeNameValue.Match(attributeValue);
                    if (attributeMatch.Success)
                    {
                        switch (attributeMatch.Groups["name"].Value.ToLowerInvariant())
                        {
                            case "leveloffset":
	                            if (int.TryParse(attributeMatch.Groups["value"].Value, out var offset))
                                {
                                    include.LevelOffset = offset;
                                }
                                break;
                            case "lines":
                                include.Lines = attributeMatch.Groups["value"].Value;
                                break;
                            case "tag":
                            case "tags":
                                include.Tags = attributeMatch.Groups["value"].Value;
                                break;
                            case "indent":
	                            if (int.TryParse(attributeMatch.Groups["value"].Value, out var indent))
                                {
                                    include.Indent = indent;
                                }
                                break;
                            default:
                                throw new NotImplementedException("TODO: add attribute to include attribute list");
                        }
                    }
                }
            }

            container.Add(include);
            attributes = null;

            reader.ReadLine();
        }

        private string[] SplitOnCharacterOutsideQuotes(string input, char character = ',')
        {
            var output = new List<string>();
            if (string.IsNullOrEmpty(input))
            {
                return output.ToArray();
            }

            var start = 0;
            var inDoubleQuotes = false;
            var inSingleQuotes = false;

            for (var index = 0; index < input.Length; index++)
            {
                var currentChar = input[index];
                if (currentChar == '"' && !inSingleQuotes)
                {
                    inDoubleQuotes = !inDoubleQuotes;
                }
                else if (currentChar == '\'' && !inDoubleQuotes)
                {
                    inSingleQuotes = !inSingleQuotes;
                }

                var atLastChar = index == input.Length - 1;

                if (atLastChar)
                {
                    output.Add(input.Substring(start));
                }
                else if (currentChar == character && !inDoubleQuotes && !inSingleQuotes)
                {
                    output.Add(input.Substring(start, index - start));
                    start = index + 1;
                }
            }

            return output.ToArray();
        }
    }
}