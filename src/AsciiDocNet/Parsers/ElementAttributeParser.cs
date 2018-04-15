using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
    public class ElementAttributeParser : ProcessBufferParserBase
    {
        public override bool IsMatch(IDocumentReader reader, Container container, AttributeList attributes) =>
            PatternMatcher.ElementAttribute.IsMatch(reader.Line.AsString());

	    protected override void InternalParse(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer,
            ref AttributeList attributes)
        {
            var match = PatternMatcher.ElementAttribute.Match(reader.Line.AsString());
            if (!match.Success)
                throw new ArgumentException("not a block attribute");

            var attributesValue = match.Groups["attributes"].Value.Trim();

            if (attributes == null)
                attributes = new AttributeList();

            if (attributesValue.IndexOf(",", StringComparison.Ordinal) == -1)
            {
                switch (attributesValue)
                {
                    case "float":
                        attributes.IsFloating = true;
                        reader.ReadLine();
                        return;
                    case "discrete":
                        attributes.IsDiscrete = true;
                        reader.ReadLine();
                        return;
                    default:
                        attributes.Add(ParseElementAttributesWithPosition(attributesValue, 0));
                        reader.ReadLine();
                        return;
                }
            }

            var inputs = SplitOnCharacterOutsideQuotes(attributesValue);

            if (inputs[0] == "quote" || inputs[0] == "verse")
            {
                for (var index = 0; index < inputs.Length; index++)
                {
                    var i = inputs[index];
                    attributes.Add(new Attribute(i, true));
                }
                reader.ReadLine();
                return;
            }

            var attributeLists = inputs.Select(ParseElementAttributesWithPosition);
            attributes = attributeLists.Aggregate(attributes, (first, second) => first.Add(second));

            reader.ReadLine();
        }

        private static string[] SplitOnCharacterOutsideQuotes(string input, char character = ',')
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

        private static AttributeList ParseElementAttributesWithPosition(string input, int position)
        {
            var attributes = new AttributeList();
            input = input.Trim();
            var start = 0;

            for (var index = 0; index < input.Length; index++)
            {
                var currentChar = input[index];
                var last = index == input.Length - 1;

                if (currentChar == '%')
                {
                    var options = new List<string>();
                    var optionsStartIndex = index + 1;

                    for (int i = optionsStartIndex; i < input.Length; i++)
                    {
                        var lastChar = i == input.Length - 1;

                        if (input[i] == '%')
                        {
                            options.Add(input.Substring(optionsStartIndex, i - optionsStartIndex));
                            optionsStartIndex = i + 1;
                        }
                        else if (lastChar || input[i] == '#' || input[i] == '.' || input[i] == '=')
                        {
                            options.Add(lastChar
                                ? input.Substring(optionsStartIndex, i - (optionsStartIndex - 1))
                                : input.Substring(optionsStartIndex, i - 1));

                            attributes.Add(new OptionsAttribute(options.ToArray(), false));
                            index = i;
                            start = i;
                            break;
                        }
                    }
                }
                else if (currentChar == '#')
                {
                    var startIdIndex = index + 1;
                    for (var i = startIdIndex; i < input.Length; i++)
                    {
                        var lastChar = i == input.Length - 1;

                        if (lastChar)
                        {
                            var value = input.Substring(startIdIndex);
                            attributes.Add(new IdAttribute(value, false));
                            index = i;
                            start = i;
                            break;
                        }
                        if (input[i] == '.' || input[i] == '%' || input[i] == '=')
                        {
                            var value = input.Substring(startIdIndex, i - 1);
                            attributes.Add(new IdAttribute(value, false));
                            index = i - 1;
                            start = i - 1;
                            break;
                        }
                    }
                }
                else if (currentChar == '.')
                {
                    var options = new List<string>();
                    var roleStartIndex = index + 1;

                    for (var i = roleStartIndex; i < input.Length; i++)
                    {
                        var lastChar = i == input.Length - 1;

                        if (input[i] == '.')
                        {
                            options.Add(input.Substring(roleStartIndex, i - roleStartIndex));
                            roleStartIndex = i + 1;
                        }
                        else if (lastChar || input[i] == '#' || input[i] == '%' || input[i] == '=')
                        {
                            options.Add(lastChar
                                ? input.Substring(roleStartIndex, i - (roleStartIndex - 1))
                                : input.Substring(roleStartIndex, i - 1));

                            attributes.Add(new RoleAttribute(options, false));
                            index = i;
                            start = i;
                            break;
                        }
                    }
                }
                else if (currentChar == '=')
                {
                    var name = input.Substring(start, index);
                    for (int i = index + 1; i < input.Length; i++)
                    {
                        var lastChar = i == input.Length - 1;

                        if (lastChar || input[i] == '#' || input[i] == '%' || input[i] == '.')
                        {
                            var singleQuoted = input[index + 1] == '\'';

                            var value = singleQuoted || input[index + 1] == '"'
                                ? input.Substring(index + 2, i - (name.Length + 2))
                                : input.Substring(index + 1, i - name.Length);

                            // TODO: handle known named elements
                            switch (name.ToLowerInvariant())
                            {
                                case "id":
                                    if (value.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        var valueParts = value.Split(',');
                                        attributes.Add(new IdAttribute(valueParts[0], valueParts[1], singleQuoted));
                                    }
                                    else
                                    {
                                        attributes.Add(new IdAttribute(value, singleQuoted));
                                    }
                                    break;
                                case "role":
                                    attributes.Add(new RoleAttribute(value, singleQuoted));
                                    break;
                                case "options":
                                case "opts":
                                    attributes.Add(new OptionsAttribute(value, singleQuoted));
                                    break;
                                case "subs":
                                    attributes.Add(new SubstitutionsAttribute(value));
                                    break;
                                default:
                                    attributes.Add(new NamedAttribute(name, value, singleQuoted));
                                    break;
                            }

                            index = i;
                            start = i;
                            break;
                        }
                    }
                }
                else if (index == 0 && position == 0)
                {
                    var hashIndex = input.IndexOf("#", StringComparison.OrdinalIgnoreCase);
                    var dotIndex = input.IndexOf(".", StringComparison.OrdinalIgnoreCase);
                    var percentIndex = input.IndexOf("%", StringComparison.OrdinalIgnoreCase);

                    if (hashIndex > -1 || dotIndex > -1 || percentIndex > -1)
                    {
                        var minIndex = new[] { hashIndex, dotIndex, percentIndex }.Where(i => i != -1).Min();
                        if (minIndex > 1)
                        {
                            var name = input.Substring(0, minIndex);
                            attributes.Add(new Attribute(name));
                            index = minIndex - 1;
                            start = minIndex - 1;
                        }
                    }
                }
                else if (last)
                {
                    var name = input.Substring(start);
                    attributes.Add(new Attribute(name));
                }
            }

            return attributes;
        }

    }
}