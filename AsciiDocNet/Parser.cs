using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
	public class Parser : IParser
	{
		public Document Process(IDocumentReader reader)
		{
			var document = new Document(reader.Path);
			reader.ReadLine();
			var buffer = new List<string>(8);
			AttributeList attributes = null;
			Process(document, reader, null, ref buffer, ref attributes);
			return document;
		}

		private InlineElementRuleMatch<TInlineElement> CreateContainerInlineElement<TInlineElement>(
			Match match,
			InlineElementConstraint constraint)
			where TInlineElement : InlineContainer, IInlineElement, IAttributable, new()
		{
			string unEscapedAttributes = null;
			var firstMatch = match.Groups[0].Value;

			if (firstMatch.StartsWith("\\"))
			{
				if (constraint == InlineElementConstraint.Constrained && !string.IsNullOrEmpty(match.Groups[2].Value))
				{
					unEscapedAttributes = match.Groups[2].Value;
				}
				else
				{
					var element = new TInlineElement();
					firstMatch = firstMatch.Substring(1, firstMatch.Length - 1);
					foreach (var inlineElement in ProcessInlineElements(firstMatch, element.ContainElementType))
					{
						element.Add(inlineElement);
					}

					return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length);
				}
			}

			if (constraint == InlineElementConstraint.Constrained)
			{
				if (unEscapedAttributes != null)
				{
					// TODO: Parse the unEscapedAttributes and add to element

					var element = new TInlineElement();

					foreach (var inlineElement in ProcessInlineElements(match.Groups[3].Value, element.ContainElementType))
					{
						element.Add(inlineElement);
					}

					var startIndex = match.Value[0] == ' ' || match.Value[0] == '\t' ? match.Index + 1 : match.Index;
					var endIndex = match.Index + match.Length;
					return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex);
				}
				else
				{
					var attributes = ParseQuotedAttributes(match.Groups[2].Value);
					var element = new TInlineElement();

					foreach (var inlineElement in ProcessInlineElements(match.Groups[3].Value, element.ContainElementType))
					{
						element.Add(inlineElement);
					}

					if (attributes != null)
					{
						element.Attributes.Add(attributes);
					}

					var startIndex = match.Value[0] == ' ' || match.Value[0] == '\t' ? match.Index + 1 : match.Index;
					var endIndex = match.Index + match.Length;

					//TODO: do something with match.Groups[1].Value
					return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex);
				}
			}
			else
			{
				var element = new TInlineElement();

				foreach (var inlineElement in ProcessInlineElements(match.Groups[2].Value, element.ContainElementType))
				{
					element.Add(inlineElement);
				}

				var attributes = ParseQuotedAttributes(match.Groups[1].Value);
				if (attributes != null)
				{
					element.Attributes.Add(attributes);
				}

				return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length);
			}
		}

		private InlineElementRuleMatch<TInlineElement> CreateInlineElement<TInlineElement>(Match match, InlineElementConstraint constraint)
			where TInlineElement : IInlineElement, IText, IAttributable, new()
		{
			string unEscapedAttributes = null;
			var firstMatch = match.Groups[0].Value;

			if (firstMatch.StartsWith("\\"))
			{
				if (constraint == InlineElementConstraint.Constrained && !string.IsNullOrEmpty(match.Groups[2].Value))
				{
					unEscapedAttributes = match.Groups[2].Value;
				}
				else
				{
					var element = new TInlineElement
					{
						Text = firstMatch.Substring(1, firstMatch.Length - 1)
					};

					return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length);
				}
			}

			if (constraint == InlineElementConstraint.Constrained)
			{
				if (unEscapedAttributes != null)
				{
					// TODO: Parse the unEscapedAttributes and add to element

					var element = new TInlineElement
					{
						Text = match.Groups[3].Value
					};

					var startIndex = match.Value[0] == ' ' ? match.Index + 1 : match.Index;
					var endIndex = match.Index + match.Length;
					return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex);
				}
				else
				{
					var attributes = ParseQuotedAttributes(match.Groups[2].Value);

					var element = new TInlineElement
					{
						Text = match.Groups[3].Value
					};

					if (attributes != null)
					{
						element.Attributes.Add(attributes);
					}

					var startIndex = match.Value[0] == ' ' ? match.Index + 1 : match.Index;
					var endIndex = match.Index + match.Length;

					//TODO: do something with match.Groups[1].Value
					return new InlineElementRuleMatch<TInlineElement>(element, startIndex, endIndex);
				}
			}
			else
			{
				var element = new TInlineElement
				{
					Text = match.Groups[2].Value
				};

				var attributes = ParseQuotedAttributes(match.Groups[1].Value);
				if (attributes != null)
				{
					element.Attributes.Add(attributes);
				}

				return new InlineElementRuleMatch<TInlineElement>(element, match.Index, match.Index + match.Length);
			}
		}

		private void ParseAnchor(string input, ref AttributeList attributes)
		{
			var match = PatternMatcher.Anchor.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not an anchor");
			}

			var id = match.Groups["id"].Value;
			var reference = !string.IsNullOrEmpty(match.Groups["reference"].Value) ? match.Groups["reference"].Value : null;
			var anchor = new Anchor(id, reference);

			if (attributes != null)
			{
				attributes.Add(anchor);
			}
			else
			{
				attributes = new AttributeList { anchor };
			}
		}

		private void ParseAttributeEntry(Container parent, string input)
		{
			var attributeEntry = ParseAttributeEntry(input);

			var document = parent as Document;

			if (document != null)
			{
				if (document.Count == 0)
				{
					document.Attributes.Add(attributeEntry);
				}
				else
				{
					document.Add(attributeEntry);
				}
			}
			else
			{
				parent.Add(attributeEntry);
			}
		}

		private AttributeEntry ParseAttributeEntry(string input)
		{
			var match = PatternMatcher.AttributeEntry.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not an attribute entry");
			}

			var name = match.Groups["name"].Value.ToLowerInvariant();
			AttributeEntry attributeEntry;
			if (name.StartsWith("!"))
			{
				attributeEntry = new UnsetAttributeEntry(name.Substring(1));
			}
			else if (name.EndsWith("!"))
			{
				attributeEntry = new UnsetAttributeEntry(name.Substring(0, name.Length - 1));
			}
			else
			{
				switch (name)
				{
					case "author":
						attributeEntry = new AuthorInfoAttributeEntry(match.Groups["value"].Value);
						break;
					default:
						attributeEntry = new AttributeEntry(name, match.Groups["value"].Value);
						break;
				}
			}

			return attributeEntry;
		}

		private AuthorInfo ParseAuthor(string input)
		{
			var match = PatternMatcher.AuthorInfo.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not an author info");
			}

			var author = new AuthorInfo
			{
				FirstName = match.Groups["firstname"].Value.Replace("_", " "),
				Email = string.IsNullOrEmpty(match.Groups["email"].Value) ? null : match.Groups["email"].Value
			};

			var middle = string.IsNullOrEmpty(match.Groups["middlename"].Value) ? null : match.Groups["middlename"].Value;
			var last = string.IsNullOrEmpty(match.Groups["lastname"].Value) ? null : match.Groups["lastname"].Value;

			if (middle != null && last != null)
			{
				author.MiddleName = middle.Replace("_", " ");
				author.LastName = last.Replace("_", " ");
			}
			else if (middle != null)
			{
				author.LastName = middle.Replace("_", " ");
			}

			return author;
		}

		private void ParseAuthors(Document document, string line)
		{
			if (line.IndexOf(';') > -1)
			{
				var authors = line.Split(';');
				foreach (var author in authors)
				{
					if (PatternMatcher.AuthorInfo.IsMatch(author))
					{
						document.Authors.Add(ParseAuthor(author));
					}
				}
			}
			else if (PatternMatcher.AuthorInfo.IsMatch(line))
			{
				document.Authors.Add(ParseAuthor(line));
			}
		}

		private void ParseCallout(Listing element, string line)
		{
			var match = PatternMatcher.Callout.Match(line);
			if (!match.Success)
			{
				throw new ArgumentException("not a callout");
			}

			var number = int.Parse(match.Groups["number"].Value);
			var text = match.Groups["text"].Value;
			element.Callouts.Add(new Callout(number, text));
		}

		private void ParseComment(Container parent, string input, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(parent, ref buffer);

			var comment = new Comment(input.Substring(2));
			parent.Add(comment);
		}

		private void ParseDocumentTitle(Document document, IDocumentReader reader, ref AttributeList attributes)
		{
			var match = PatternMatcher.DocumentTitle.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not a document title");
			}

			var title = match.Groups["title"].Value;
			var lastColonIndex = title.LastIndexOf(":", StringComparison.OrdinalIgnoreCase);
			var documentTitle = lastColonIndex > -1
				? new DocumentTitle(title.Substring(0, lastColonIndex), title.Substring(lastColonIndex + 1))
				: new DocumentTitle(title);

			documentTitle.Attributes.Add(attributes);
			document.Title = documentTitle;

			attributes = null;
		}

		private void ParseElementAttributes(string input, ref AttributeList attributes)
		{
			var match = PatternMatcher.ElementAttribute.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not a block attribute");
			}

			var attributesValue = match.Groups["attributes"].Value;
			var inputs = SplitOnCharacterOutsideQuotes(attributesValue);
			var attributeLists = inputs.Select(ParseElementAttributesWithPosition);

			if (attributes == null)
			{
				attributes = new AttributeList();
			}

			attributes = attributeLists.Aggregate(attributes, (first, second) => first.Add(second));
		}

		private AttributeList ParseElementAttributesWithPosition(string input, int position)
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
					var name = input.Substring(start, index).ToLowerInvariant();
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
							switch (name)
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
							var name = input.Substring(0, minIndex).ToLowerInvariant();
							attributes.Add(new Attribute(name));
							index = minIndex - 1;
							start = minIndex - 1;
						}
					}
				}
				else if (last)
				{
					var name = input.Substring(start).ToLowerInvariant();
					attributes.Add(new Attribute(name));
				}
			}

			return attributes;
		}

		private void ParseInclude(Container parent, string input, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(parent, ref buffer);

			var match = PatternMatcher.Include.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not an include", nameof(input));
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
								int offset;
								if (int.TryParse(attributeMatch.Groups["value"].Value, out offset))
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
								int indent;
								if (int.TryParse(attributeMatch.Groups["value"].Value, out indent))
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

			parent.Add(include);
			attributes = null;
		}

		private void ParseMedia(Container parent, string input, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(parent, ref buffer);
			var match = PatternMatcher.Media.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not a media", nameof(input));
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

			parent.Add(media);
			attributes = null;
		}

		private AttributeList ParseQuotedAttributes(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			if (value.IndexOf("{", StringComparison.CurrentCultureIgnoreCase) > -1)
			{
				// TODO: Handle attribute references. Do nothing for now, let them fall through
			}

			value = value.Trim();

			if (value.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
			{
				value = value.Split(new[] { ',' }, 2)[0];
			}

			if (value.Length == 0)
			{
				return new AttributeList();
			}

			if (value.StartsWith(".") || value.StartsWith("#"))
			{
				var segments = value.Split(new[] { '#' }, 2);
				string id = null;
				HashSet<string> roles = new HashSet<string>();
				if (segments.Length > 1)
				{
					var subSegments = segments[1].Split('.');

					if (subSegments.Length > 1)
					{
						id = subSegments[0];
						for (int i = 1; i < subSegments.Length; i++)
						{
							roles.Add(subSegments[i]);
						}
					}
				}

				foreach (var role in segments[0].Split('.'))
				{
					roles.Add(role);
				}

				var attributes = new AttributeList();
				if (id != null)
				{
					attributes.Add(new IdAttribute(id, false));
				}
				if (roles.Any())
				{
					attributes.Add(new RoleAttribute(roles, false));
				}

				return attributes;
			}

			return new AttributeList { new RoleAttribute(value, false) };
		}

		// TODO: based on html output, a section title should define a section block element into which all proceeding elements should be added, until the next section title is hit
		private void ParseSectionTitle(Container parent, string input, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(parent, ref buffer);

			var match = PatternMatcher.SectionTitle.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not a section title");
			}

			var title = match.Groups["title"].Value;
			var inlineElements = ProcessInlineElements(title);
			var level = match.Groups["level"].Value.Length;
			var sectionTitle = new SectionTitle(inlineElements, level);
			sectionTitle.Attributes.Add(attributes);
			parent.Add(sectionTitle);
			attributes = null;
		}

		private void ParseTitle(string input, ref AttributeList attributes)
		{
			var match = PatternMatcher.Title.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException("not a block title");
			}

			var title = new Title(match.Groups["title"].Value);
			if (attributes != null)
			{
				attributes.Add(title);
			}
			else
			{
				attributes = new AttributeList { title };
			}
		}

		private void Process(Container container, IDocumentReader reader, Regex delimiterRegex, ref List<string> buffer, ref AttributeList attributes)
		{
			while (reader.Line != null)
			{
				if (delimiterRegex != null && delimiterRegex.IsMatch(reader.Line))
				{
					ProcessParagraph(container, ref buffer, ref attributes);
					return;
				}

				// look for document title on first line or if we've only parsed comments so far
				if ((reader.LineNumber == 1 || container.GetType() == typeof(Document) && container.All(e => e is Comment)) &&
				    PatternMatcher.DocumentTitle.IsMatch(reader.Line))
				{
					var document = (Document)container;
					ParseDocumentTitle(document, reader, ref attributes);

					reader.ReadLine();
					if (reader.Line == null)
					{
						break;
					}

					ParseAuthors(document, reader.Line);
				}
				else if (PatternMatcher.Title.IsMatch(reader.Line))
				{
					// if we have an existing lines in the buffer and
					// we also have attributes, then set these
					ProcessParagraph(container, ref buffer, ref attributes);
					ParseTitle(reader.Line, ref attributes);
				}
				else if (PatternMatcher.Anchor.IsMatch(reader.Line))
				{
					// if we have an existing lines in the buffer and
					// we also have attributes, then set these
					ProcessParagraph(container, ref buffer, ref attributes);
					ParseAnchor(reader.Line, ref attributes);
				}
				else if (PatternMatcher.ElementAttribute.IsMatch(reader.Line))
				{
					// if we have an existing lines in the buffer and
					// we also have attributes, then set these
					ProcessParagraph(container, ref buffer, ref attributes);
					ParseElementAttributes(reader.Line, ref attributes);
				}
				else if (PatternMatcher.AttributeEntry.IsMatch(reader.Line))
				{
					ProcessParagraph(container, ref buffer);
					ParseAttributeEntry(container, reader.Line);
				}
				else if (PatternMatcher.SectionTitle.IsMatch(reader.Line))
				{
					ParseSectionTitle(container, reader.Line, ref buffer, ref attributes);
				}
				else if (PatternMatcher.Include.IsMatch(reader.Line))
				{
					ParseInclude(container, reader.Line, ref buffer, ref attributes);
				}
				else if (PatternMatcher.Media.IsMatch(reader.Line))
				{
					ParseMedia(container, reader.Line, ref buffer, ref attributes);
				}
				else if (PatternMatcher.CommentLine.IsMatch(reader.Line))
				{
					ParseComment(container, reader.Line, ref buffer, ref attributes);
				}
				else if (PatternMatcher.Table.IsMatch(reader.Line))
				{
					ProcessTable(container, reader, ref buffer, ref attributes);
				}
				else if (PatternMatcher.ListItem.IsMatch(reader.Line))
				{
					ProcessUnorderedListItem(new ParsingContext(container, delimiterRegex), reader, ref buffer, ref attributes);
					continue;
				}
				else if (PatternMatcher.CheckListItem.IsMatch(reader.Line))
				{
					ProcessCheckListItem(new ParsingContext(container, delimiterRegex), reader, ref buffer, ref attributes);
					continue;
				}
				else if (PatternMatcher.OrderedListItem.IsMatch(reader.Line))
				{
					ProcessOrderedListItem(new ParsingContext(container, delimiterRegex), reader, ref buffer, ref attributes);
					continue;
				}
				else if (PatternMatcher.LabeledListItem.IsMatch(reader.Line))
				{
					ProcessLabeledListItem(new ParsingContext(container, delimiterRegex), reader, ref buffer, ref attributes);
					continue;
				}
				else if (PatternMatcher.Block.IsMatch(reader.Line))
				{
					var delimiter = PatternMatcher.Block.Match(reader.Line).Groups["delimiter"].Value;

					// TODO: Handle Admonitions
					switch (delimiter)
					{
						case Patterns.Block.Comment:
							Process<Comment>(container, reader, ref buffer, ref attributes);
							break;
						case Patterns.Block.Example:
							ProcessBlock<Example>(container, reader, ref buffer, ref attributes);
							break;
						case Patterns.Block.Fenced:
							Process<Fenced>(container, reader, ref buffer, ref attributes);
							break;
						case Patterns.Block.Listing:
							if (attributes.ContainBlockName("source"))
							{
								ProcessListing<Source>(container, reader, ref buffer, ref attributes);
							}
							else
							{
								ProcessListing<Listing>(container, reader, ref buffer, ref attributes);
							}
							continue;
						case Patterns.Block.Literal:
							Process<Literal>(container, reader, ref buffer, ref attributes);
							break;
						case Patterns.Block.Open:
							ProcessBlock<Open>(container, reader, ref buffer, ref attributes);
							break;
						case Patterns.Block.Pass:
							if (attributes.ContainBlockName("stem"))
							{
								Process<Stem>(container, reader, ref buffer, ref attributes);
							}
							else
							{
								Process<Pass>(container, reader, ref buffer, ref attributes);
							}
							break;
						case Patterns.Block.Quote:
							if (attributes.ContainBlockName("verse"))
							{
								ProcessBlock<Verse>(container, reader, ref buffer, ref attributes);
							}
							else
							{
								ProcessBlock<Quote>(container, reader, ref buffer, ref attributes);
							}
							break;
						case Patterns.Block.Sidebar:
							ProcessBlock<Sidebar>(container, reader, ref buffer, ref attributes);
							break;
						default:
							throw new InvalidOperationException($"Unrecognized block delimiter: {delimiter}");
					}
				}
				else if (PatternMatcher.Admonition.IsMatch(reader.Line))
				{
					ProcessAdmonition(container, reader, ref buffer, ref attributes);
				}
				else if (PatternMatcher.BlankCharacters.IsMatch(reader.Line))
				{
					ProcessBuffer(container, ref buffer, ref attributes);
				}
				else
				{
					buffer.Add(reader.Line);
				}

				reader.ReadLine();
			}

			ProcessBuffer(container, ref buffer, ref attributes);
		}

		private void Process(Container parent, IDocumentReader reader, Regex delimiterRegex)
		{
			var buffer = new List<string>(8);
			AttributeList attributes = null;
			Process(parent, reader, delimiterRegex, ref buffer, ref attributes);
		}

		private void Process<TElement>(Container parent, IDocumentReader reader, ref List<string> buffer,
			ref AttributeList attributes)
			where TElement : IText, IElement, IAttributable, new()
		{
			var delimiterRegex = PatternMatcher.GetDelimiterRegexFor<TElement>();
			var isDelimiter = delimiterRegex.IsMatch(reader.Line);

			if (isDelimiter)
			{
				ProcessParagraph(parent, ref buffer);
				reader.ReadLine();
				while (reader.Line != null && !delimiterRegex.IsMatch(reader.Line))
				{
					buffer.Add(reader.Line);
					reader.ReadLine();
				}
			}
			else
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
				while (reader.Line != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line))
				{
					buffer.Add(reader.Line);
					reader.ReadLine();
				}
			}

			var element = new TElement { Text = string.Join(Environment.NewLine, buffer) };
			element.Attributes.Add(attributes);
			parent.Add(element);
			attributes = null;
			buffer = new List<string>(8);
		}

		private void ProcessAdmonition(Container container, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			var match = PatternMatcher.Admonition.Match(reader.Line);

			if (!match.Success)
			{
				throw new ArgumentException("not an admonition");
			}

			buffer.Add(match.Groups["text"].Value);
			reader.ReadLine();
			while (reader.Line != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line))
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
			}

			var admonition = new Admonition(match.Groups["style"].Value.ToEnum<AdmonitionStyle>());
			admonition.Attributes.Add(attributes);
			ProcessParagraph(admonition, ref buffer);
			container.Add(admonition);
			attributes = null;
		}

		private void ProcessBlock<TElement>(Container parent, IDocumentReader reader, ref List<string> buffer,
			ref AttributeList attributes)
			where TElement : Container, IElement, IAttributable, new()
		{
			var delimiterRegex = PatternMatcher.GetDelimiterRegexFor<TElement>();
			var element = new TElement();
			element.Attributes.Add(attributes);
			if (delimiterRegex.IsMatch(reader.Line))
			{
				ProcessParagraph(parent, ref buffer);
				reader.ReadLine();
				Process(element, reader, delimiterRegex);
			}
			else
			{
				ProcessParagraph(element, ref buffer);
				Process(element, reader, PatternMatcher.BlankCharacters);
			}

			parent.Add(element);
			attributes = null;
		}

		private void ProcessBuffer(Container parent, ref List<string> buffer, ref AttributeList attributes)
		{
			if (buffer.Count > 0)
			{
				if (attributes.ContainBlockName("quote"))
				{
					ProcessLine<Quote>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("pass"))
				{
					ProcessSimple<Pass>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("example"))
				{
					ProcessLine<Example>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("stem"))
				{
					ProcessSimple<Stem>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("sidebar"))
				{
					ProcessLine<Sidebar>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("source"))
				{
					ProcessSimple<Source>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("listing"))
				{
					ProcessSimple<Listing>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("literal"))
				{
					ProcessSimple<Literal>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("comment"))
				{
					ProcessSimple<Comment>(parent, ref buffer, ref attributes);
				}
				else if (attributes.ContainBlockName("verse"))
				{
					ProcessLine<Verse>(parent, ref buffer, ref attributes);
				}
				else
				{
					ProcessParagraph(parent, ref buffer, ref attributes);
				}

				buffer = new List<string>(8);
			}

			attributes = null;
		}

		private void ProcessCheckListItem(ParsingContext context, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(context.Parent, ref buffer);
			var match = PatternMatcher.CheckListItem.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not a check list item");
			}

			var level = match.Groups["level"].Value;
			var isChecked = !string.IsNullOrWhiteSpace(match.Groups["checked"].Value);
			var text = match.Groups["text"].Value;

			var listItem = new CheckListItem(level.Length, isChecked);
			listItem.Attributes.Add(attributes);

			buffer.Add(text);
			reader.ReadLine();

			while (reader.Line != null &&
			       !PatternMatcher.ListItemContinuation.IsMatch(reader.Line) &&
			       !PatternMatcher.BlankCharacters.IsMatch(reader.Line) &&
			       !PatternMatcher.CheckListItem.IsMatch(reader.Line) &&
			       !PatternMatcher.ListItem.IsMatch(reader.Line) &&
			       !context.IsMatch(reader.Line))
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
			}

			// TODO: handle list item continuations (i.e. continued with +)
			ProcessParagraph(listItem, ref buffer);

			UnorderedList unorderedList;
			if (context.Parent.Count > 0)
			{
				unorderedList = context.Parent[context.Parent.Count - 1] as UnorderedList;

				if (unorderedList != null && unorderedList.Items.Count > 0 && unorderedList.Items[0].Level == listItem.Level)
				{
					unorderedList.Items.Add(listItem);
				}
				else
				{
					unorderedList = new UnorderedList { Items = { listItem } };
					context.Parent.Add(unorderedList);
				}
			}
			else
			{
				unorderedList = new UnorderedList { Items = { listItem } };
				context.Parent.Add(unorderedList);
			}

			attributes = null;
		}

		// TODO: Handle nested inline elements
		private IList<IInlineElement> ProcessInlineElements(string block, InlineElementType elementType = InlineElementType.All)
		{
			var elements = new List<IInlineElement>();
			if (string.IsNullOrWhiteSpace(block))
			{
				return elements;
			}

			var matches = new List<InlineElementRuleMatch>();

			foreach (var rule in PatternMatcher.InlineElementRules.Where(r => elementType.HasFlag(r.ElementType)))
			{
				if (rule.Regex.IsMatch(block))
				{
					var currentRule = rule;
					var outerMatches = matches;
					block = currentRule.Regex.Replace(block, match =>
					{
						switch (currentRule.ElementType)
						{
							case InlineElementType.Emphasis:
							case InlineElementType.EmphasisDouble:
								var emphasis = CreateInlineElement<Emphasis>(match, currentRule.Constraint);

								if (currentRule.ElementType == InlineElementType.EmphasisDouble)
								{
									emphasis.Element.DoubleDelimited = true;
								}

								outerMatches.Add(emphasis);
								break;
							case InlineElementType.Strong:
							case InlineElementType.StrongDouble:
								var strong = CreateContainerInlineElement<Strong>(match, currentRule.Constraint);

								if (currentRule.ElementType == InlineElementType.StrongDouble)
								{
									strong.Element.DoubleDelimited = true;
								}

								outerMatches.Add(strong);
								break;
							case InlineElementType.Monospace:
							case InlineElementType.MonospaceDouble:
								var monospace = CreateContainerInlineElement<Monospace>(match, currentRule.Constraint);
								if (currentRule.ElementType == InlineElementType.MonospaceDouble)
								{
									monospace.Element.DoubleDelimited = true;
								}

								outerMatches.Add(monospace);
								break;
							case InlineElementType.Subscript:
								var subscript = CreateInlineElement<Subscript>(match, currentRule.Constraint);
								outerMatches.Add(subscript);

								break;
							case InlineElementType.Superscript:
								var superscript = CreateInlineElement<Superscript>(match, currentRule.Constraint);
								outerMatches.Add(superscript);

								break;
							case InlineElementType.Quotation:
							case InlineElementType.QuotationDouble:
								var quotation = CreateContainerInlineElement<Quotation>(match, currentRule.Constraint);
								if (currentRule.ElementType == InlineElementType.QuotationDouble)
								{
									quotation.Element.DoubleDelimited = true;
								}

								outerMatches.Add(quotation);
								break;
							case InlineElementType.Mark:
							case InlineElementType.MarkDouble:
								var mark = CreateContainerInlineElement<Mark>(match, currentRule.Constraint);

								if (currentRule.ElementType == InlineElementType.MarkDouble)
								{
									mark.Element.DoubleDelimited = true;
								}

								outerMatches.Add(mark);
								break;
							case InlineElementType.InternalAnchor:
								var internalAnchorParts = match.Groups[1].Value;
								InternalAnchor anchor;
								if (internalAnchorParts.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
								{
									var parts = internalAnchorParts.Split(',');
									// TODO: Process inline elements of parts[1]
									anchor = new InternalAnchor(parts[0], parts[1]);
								}
								else
								{
									anchor = new InternalAnchor(internalAnchorParts);
								}

								outerMatches.Add(new InlineElementRuleMatch(anchor, match.Index, match.Index + match.Length));
								break;
							case InlineElementType.InlineAnchor:
								var inlineAnchorId = match.Groups[1].Value;
								var xRefLabel = match.Groups[2].Value;

								// TODO: process inline elements of xRefLabel
								var inlineAnchor = !string.IsNullOrEmpty(xRefLabel)
									? new InlineAnchor(inlineAnchorId, xRefLabel)
									: new InlineAnchor(inlineAnchorId);

								outerMatches.Add(new InlineElementRuleMatch(inlineAnchor, match.Index, match.Index + match.Length));
								break;
							case InlineElementType.AttributeReference:
								var text = match.Groups[2].Value;
								var attributeReference = new AttributeReference(text);
								outerMatches.Add(new InlineElementRuleMatch(attributeReference, match.Index, match.Index + match.Length));
								break;
							case InlineElementType.ImplicitLink:
								// TODO: split attributes out
								var attributes = match.Groups["attributes"].Value;
								var href = match.Groups["href"].Value;
								var link = new Link(href, attributes);
								outerMatches.Add(new InlineElementRuleMatch(link, match.Index + match.Groups[1].Length, match.Index + match.Length));
								break;
						}

						// we want to keep the index of matches for ordering purposes so
						// replace any matches with whitespace. This could be optimized
						return new string(' ', match.Length);
					});
				}
			}

			if (matches.Any())
			{
				matches = matches.OrderBy(t => t.StartIndex).ToList();
				for (int i = 0; i < matches.Count; i++)
				{
					var match = matches[i];
					var isLastMatch = i == matches.Count - 1;

					if (i == 0 && match.StartIndex > 0)
					{
						elements.Add(new TextLiteral(block.Substring(0, matches[0].StartIndex)));
					}

					elements.Add(match.Element);

					if (!isLastMatch)
					{
						// is there a literal between where this match ends and next one begins
						var nextMatch = matches[i + 1];
						var length = nextMatch.StartIndex - match.EndIndex;
						if (length > 0)
						{
							elements.Add(new TextLiteral(block.Substring(match.EndIndex, length)));
						}
					}
					else
					{
						if (match.EndIndex < block.Length)
						{
							elements.Add(new TextLiteral(block.Substring(match.EndIndex)));
						}
					}
				}
			}
			else
			{
				elements.Add(new TextLiteral(block));
			}

			return elements;
		}

		private void ProcessLabeledListItem(ParsingContext context, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(context.Parent, ref buffer);
			var match = PatternMatcher.LabeledListItem.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not a labeled list item");
			}

			var label = match.Groups["label"].Value;
			var level = match.Groups["level"].Value.Length;

			// levels start at 0
			if (level > 0)
			{
				level -= 2;
			}

			var labeledListItem = new LabeledListItem(label, level);
			labeledListItem.Attributes.Add(attributes);

			var text = match.Groups["text"].Value;

			// labeled lists are lenient with whitespace so can have whitespace after the label
			// and before any content.
			if (!string.IsNullOrWhiteSpace(text))
			{
				buffer.Add(text);
				reader.ReadLine();
			}
			else
			{
				reader.ReadLine();
				while (reader.Line != null &&
				       PatternMatcher.BlankCharacters.IsMatch(reader.Line))
				{
					reader.ReadLine();
				}
			}

			while (reader.Line != null &&
			       !PatternMatcher.ListItemContinuation.IsMatch(reader.Line) &&
			       !PatternMatcher.BlankCharacters.IsMatch(reader.Line) &&
			       !PatternMatcher.LabeledListItem.IsMatch(reader.Line) &&
			       !context.IsMatch(reader.Line))
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
			}

			// TODO: handle multi element list items (i.e. continued with +)
			// TODO: This may not be a paragraph.
			ProcessParagraph(labeledListItem, ref buffer);

			LabeledList labeledList;
			if (context.Parent.Count > 0)
			{
				labeledList = context.Parent[context.Parent.Count - 1] as LabeledList;

				if (labeledList != null && labeledList.Items.Count > 0 && labeledList.Items[0].Level == labeledListItem.Level)
				{
					labeledList.Items.Add(labeledListItem);
				}
				else
				{
					labeledList = new LabeledList { Items = { labeledListItem } };
					context.Parent.Add(labeledList);
				}
			}
			else
			{
				labeledList = new LabeledList { Items = { labeledListItem } };
				context.Parent.Add(labeledList);
			}

			attributes = null;
		}

		private void ProcessLine<TElement>(Container parent, ref List<string> buffer, ref AttributeList attributes)
			where TElement : Container, IElement, IAttributable, new()
		{
			var element = new TElement();
			element.Attributes.Add(attributes);
			ProcessParagraph(element, ref buffer);
			parent.Add(element);
		}

		private void ProcessListing<TElement>(Container parent, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
			where TElement : Listing, IElement, new()
		{
			var delimiterRegex = PatternMatcher.GetDelimiterRegexFor<TElement>();
			var isDelimiter = delimiterRegex.IsMatch(reader.Line);

			if (isDelimiter)
			{
				ProcessParagraph(parent, ref buffer);
				reader.ReadLine();
				while (reader.Line != null && !delimiterRegex.IsMatch(reader.Line))
				{
					buffer.Add(reader.Line);
					reader.ReadLine();
				}
			}
			else
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
				while (reader.Line != null && !PatternMatcher.BlankCharacters.IsMatch(reader.Line))
				{
					buffer.Add(reader.Line);
					reader.ReadLine();
				}
			}

			var element = new TElement { Text = string.Join(Environment.NewLine, buffer) };
			element.Attributes.Add(attributes);

			reader.ReadLine();
			if (isDelimiter)
			{
				while (reader.Line != null && PatternMatcher.Callout.IsMatch(reader.Line))
				{
					ParseCallout(element, reader.Line);
					reader.ReadLine();
				}
			}

			parent.Add(element);
			attributes = null;
			buffer = new List<string>(8);
		}

		private void ProcessOrderedListItem(ParsingContext context, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(context.Parent, ref buffer);
			var match = PatternMatcher.OrderedListItem.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not an ordered list item");
			}

			var level = match.Groups["level"].Value;
			var orderedListItem = new OrderedListItem(level.Length);
			orderedListItem.Attributes.Add(attributes);

			var number = match.Groups["number"].Value;
			var upperAlpha = match.Groups["upperalpha"].Value;
			var lowerAlpha = match.Groups["loweralpha"].Value;
			var upperRoman = match.Groups["upperRoman"].Value;
			var lowerRoman = match.Groups["lowerRoman"].Value;

			if (!string.IsNullOrEmpty(number))
			{
				orderedListItem.Number = int.Parse(number);
			}
			else if (!string.IsNullOrEmpty(upperAlpha))
			{
				orderedListItem.Numbering = NumberStyle.UpperAlpha;
				orderedListItem.Number = Array.IndexOf(Patterns.UpperAlphabet, upperAlpha) + 1;
			}
			else if (!string.IsNullOrEmpty(lowerAlpha))
			{
				orderedListItem.Numbering = NumberStyle.LowerAlpha;
				orderedListItem.Number = Array.IndexOf(Patterns.LowerAlphabet, lowerAlpha) + 1;
			}
			else if (!string.IsNullOrEmpty(upperRoman))
			{
				orderedListItem.Numbering = NumberStyle.UpperRoman;
				orderedListItem.Number = RomanNumerals.ToInt(upperRoman);
			}
			else if (!string.IsNullOrEmpty(lowerRoman))
			{
				orderedListItem.Numbering = NumberStyle.LowerRoman;
				orderedListItem.Number = RomanNumerals.ToInt(lowerRoman);
			}

			var text = match.Groups["text"].Value;
			buffer.Add(text);
			reader.ReadLine();

			while (reader.Line != null &&
			       !PatternMatcher.ListItemContinuation.IsMatch(reader.Line) &&
			       !PatternMatcher.BlankCharacters.IsMatch(reader.Line) &&
			       !PatternMatcher.OrderedListItem.IsMatch(reader.Line) &&
			       !context.IsMatch(reader.Line))
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
			}

			// TODO: handle multi element list items (i.e. continued with +)
			ProcessParagraph(orderedListItem, ref buffer);

			OrderedList orderedList;
			if (context.Parent.Count > 0)
			{
				orderedList = context.Parent[context.Parent.Count - 1] as OrderedList;

				if (orderedList != null && orderedList.Items.Count > 0 && orderedList.Items[0].Level == orderedListItem.Level)
				{
					orderedList.Items.Add(orderedListItem);
				}
				else
				{
					orderedList = new OrderedList { Items = { orderedListItem } };
					context.Parent.Add(orderedList);
				}
			}
			else
			{
				orderedList = new OrderedList { Items = { orderedListItem } };
				context.Parent.Add(orderedList);
			}

			attributes = null;
		}

		private void ProcessParagraph(Container parent, ref List<string> buffer)
		{
			AttributeList attributes = null;
			ProcessParagraph(parent, ref buffer, ref attributes);
		}

		private void ProcessParagraph(Container parent, ref List<string> buffer, ref AttributeList attributes)
		{
			if (buffer.Count > 0 && !buffer.All(string.IsNullOrWhiteSpace))
			{
				var inlineElements = ProcessInlineElements(string.Join(Environment.NewLine, buffer));
				var paragraph = new Paragraph(inlineElements);
				paragraph.Attributes.Add(attributes);
				parent.Add(paragraph);

				attributes = null;
				buffer = new List<string>(8);
			}
		}

		// TODO: Simple elements i.e. those that simply have a verbatim string value, should probably still take a paragraph or literal
		private void ProcessSimple<TElement>(Container parent, ref List<string> buffer, ref AttributeList attributes)
			where TElement : IElement, IText, IAttributable, new()
		{
			var element = new TElement
			{
				Text = string.Join(Environment.NewLine, buffer)
			};
			element.Attributes.Add(attributes);
			parent.Add(element);
		}

		private void ProcessTable(Container parent, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(parent, ref buffer);

			var match = PatternMatcher.Table.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not the start of a table");
			}

			var delimiter = match.Groups[1].Value;
			var rows = new List<TableRow>();
			reader.ReadLine();
			while (reader.Line != null &&
			       !Regex.IsMatch(reader.Line, Regex.Escape(match.Groups[0].Value)))
			{
				reader.ReadLine();
			}

			throw new NotImplementedException("TODO");
		}

		private void ProcessUnorderedListItem(ParsingContext context, IDocumentReader reader, ref List<string> buffer, ref AttributeList attributes)
		{
			ProcessParagraph(context.Parent, ref buffer);
			var match = PatternMatcher.ListItem.Match(reader.Line);
			if (!match.Success)
			{
				throw new ArgumentException("not an unordered list item");
			}

			var level = match.Groups["level"].Value;
			var text = match.Groups["text"].Value;
			var listItem = new UnorderedListItem(level.Length);
			listItem.Attributes.Add(attributes);

			buffer.Add(text);
			reader.ReadLine();

			while (reader.Line != null &&
			       !PatternMatcher.ListItemContinuation.IsMatch(reader.Line) &&
			       !PatternMatcher.BlankCharacters.IsMatch(reader.Line) &&
			       !PatternMatcher.ListItem.IsMatch(reader.Line) &&
			       !context.IsMatch(reader.Line))
			{
				buffer.Add(reader.Line);
				reader.ReadLine();
			}

			// TODO: handle list item continuations (i.e. continued with +)
			ProcessParagraph(listItem, ref buffer);

			UnorderedList unorderedList;
			if (context.Parent.Count > 0)
			{
				unorderedList = context.Parent[context.Parent.Count - 1] as UnorderedList;

				if (unorderedList != null && unorderedList.Items.Count > 0 && unorderedList.Items[0].Level == listItem.Level)
				{
					unorderedList.Items.Add(listItem);
				}
				else
				{
					unorderedList = new UnorderedList { Items = { listItem } };
					context.Parent.Add(unorderedList);
				}
			}
			else
			{
				unorderedList = new UnorderedList { Items = { listItem } };
				context.Parent.Add(unorderedList);
			}

			attributes = null;
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

	public class TableRow
	{
	}
}