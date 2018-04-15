using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class MediaParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.Media.IsMatch(reader.Line.AsString());

	    protected override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.Media.Match(reader.Line.AsString());
            if (!match.Success)
            {
                throw new ArgumentException("not a media");
            }

            var path = match.Groups["path"].Value;
            Media media;

            switch (match.Groups["media"].Value.ToLowerInvariant())
            {
                case "image":
                    media = new Image(path);
                    break;
                case "video":
                    media = new Video(path);
                    break;
                case "audio":
                    media = new Audio(path);
                    break;
                default:
                    throw new ArgumentException("unrecognized media type");
            }

            media.Attributes.Add(attributes);
            var attributesValue = match.Groups["attributes"].Value;
            int? width = null;
            int? height = null;

            if (!string.IsNullOrEmpty(attributesValue))
            {
                var attributeValues = SplitOnCharacterOutsideQuotes(attributesValue);

                for (int index = 0; index < attributeValues.Length; index++)
                {
                    var attributeValue = attributeValues[index];
                    int dimension;

                    if (index == 0)
                    {
                        media.AlternateText = attributeValue;
                    }
                    else if (index == 1 && int.TryParse(attributeValue, out dimension))
                    {
                        width = dimension;
                    }
                    else if (index == 2 && int.TryParse(attributeValue, out dimension))
                    {
                        height = dimension;
                    }
                    else
                    {
                        var attributeMatch = PatternMatcher.AttributeNameValue.Match(attributeValue);

                        if (attributeMatch.Success)
                        {
                            switch (attributeMatch.Groups["name"].Value.ToLowerInvariant())
                            {
                                case "link":
                                    media.Link = attributeMatch.Groups["value"].Value;
                                    break;
                                case "title":
                                    media.Title = attributeMatch.Groups["value"].Value;
                                    break;
                                case "float":
                                    media.Float = attributeMatch.Groups["value"].Value;
                                    break;
                                case "align":
                                    media.Align = attributeMatch.Groups["value"].Value;
                                    break;
                                case "role":
                                    media.Role = attributeMatch.Groups["value"].Value;
                                    break;
                                default:
                                    throw new NotImplementedException("TODO: add attribute to media attribute list");
                            }
                        }
                    }
                }
            }
            if (width.HasValue && height.HasValue)
            {
                media.SetWidthAndHeight(width.Value, height.Value);
            }

            container.Add(media);
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