using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsciiDocNet
{
	internal static class PatternMatcher
	{
		public static readonly Regex Admonition =
			new Regex(
				$@"^(?<style>{string.Join("|", Patterns.Admonitions)}):{Patterns.CharacterGroupWhitespace}(?<text>.*?){Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Anchor =
			new Regex(
				$@"^\[\[(?:|(?<id>[{Patterns.CharacterClassAlpha}:_][{Patterns.CharacterClassWord}:.-]*)(?:,{Patterns.CharacterGroupWhitespace}*(?<reference>\S.*))?)\]\]$");

		public static readonly Regex AttributeEntry =
			new Regex($@"^\s*:(?<name>!?[\w_][\w_\-]*!?):{Patterns.CharacterGroupWhitespace}*(?<value>.*?){Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex AttributeNameValue = new Regex("^(?<name>.*?)=\"?(?<value>.*?)\"?");

		public static readonly Regex AttributeReference = new Regex(@"(\\)?\{((set|counter2?):.+?|\w+(?:[\-]\w+)*)(\\)?\}");

		public static readonly Regex AttributeValidName = new Regex(@"^[\w_][\w_\-]*$");

		public static readonly Regex AuthorInfo =
			new Regex(
				$@"^{Patterns.CharacterGroupWhitespace}*(?<firstname>{Patterns.CharacterGroupWord}[{Patterns.CharacterClassWord}\-'.]*)(?: +(?<middlename>{Patterns
					.CharacterGroupWord}[{Patterns.CharacterClassWord}\-'.]*))?(?: +(?<lastname>{Patterns.CharacterGroupWord}[{Patterns.CharacterClassWord}\-'.]*))?(?: +<(?<email>[^>]+)>)?{Patterns
						.CharacterGroupWhitespace}*$");

		public static readonly Regex BlankCharacters = new Regex($@"^{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Block = new Regex($@"^(?<delimiter>([\/|\=|\-|\.|_|\*|\+]{{4}})|`{{3}}|\-{{2}}){Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Callout = new Regex($@"^<?(?<number>\d+)>{Patterns.CharacterGroupWhitespace}+(?<text>.*)$");

		public static readonly Regex CheckListItem =
			new Regex(
				$@"^(?<level>(?:\-|\*){{1,5}}){Patterns.CharacterGroupWhitespace}+\[(?<checked>\*|\s|x|X)\]{Patterns.CharacterGroupWhitespace}+(?<text>.*?)$");

		public static readonly Regex CommentBlock = new Regex($"^{Regex.Escape(Patterns.Block.Comment)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex CommentLine = new Regex(@"^//(?:[^/]|$)");

		public static readonly Regex DocumentTitle =
			new Regex($@"^(?:=|#){Patterns.CharacterGroupWhitespace}+(?<title>\S.*?)(?:{Patterns.CharacterGroupWhitespace}+\1)?$");

		public static readonly Regex ElementAttribute =
			new Regex($@"^{Patterns.CharacterGroupWhitespace}*\[(?<attributes>[^\[].*?[^\]])\]{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Emphasis =
			new Regex($@"(^|[^{Patterns.CharacterClassWord};:}}])(?:\[([^\]]+?)\])?_(\S|\S{Patterns.CharacterClassAll}*?\S)_(?!{Patterns.CharacterGroupWord})");

		public static readonly Regex EmphasisDouble = new Regex($@"\\?(?:\[([^\]]+?)\])?__({Patterns.CharacterClassAll}+?)__");

		public static readonly Regex Example = new Regex($"^{Regex.Escape(Patterns.Block.Example)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Fenced = new Regex($"^{Regex.Escape(Patterns.Block.Fenced)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex ImplicitLink =
			new Regex(@"(^|link:|<|[\s>\(\)\[\];]|[^`])(?<href>\\?(?:https?|file|ftp|irc)://[^\s\[\]<]*[^\s.,\[\]<])(?:\[(?<attributes>(?:\\\]|[^\]])*?)\])?");

		public static readonly Regex Include = new Regex(@"^\\?include::(?<path>[^\[]+)\[(?<attributes>.*?)\]$");

		public static readonly Regex InlineAnchor =
			new Regex(
				$@"\\?(?:\[\[([{Patterns.CharacterClassAlpha}:_][{Patterns.CharacterClassWord}:.-]*)(?:,{Patterns.CharacterGroupWhitespace}*(\S.*?))?\]\]|anchor:(\S+)\[(.*?[^\\])?\])");

		// TODO: handle inline image, using regex above ^
		public static readonly Regex InternalAnchor =
			new Regex(
				$@"\\?(?:<<([{Patterns.CharacterClassWord}"":.\/]{Patterns.CharacterClassAll}*?)>>|xref:([{Patterns.CharacterClassWord}"":.\/]{Patterns
					.CharacterClassAll
					}*?)\[({Patterns.CharacterClassAll}*?)\])");

		public static readonly Regex Mark =
			new Regex($@"(^|[^{Patterns.CharacterClassWord}&;:}}])(?:\[([^\]]+?)\])?#(\S|\S{Patterns.CharacterClassAll}*?\S)#(?!{Patterns.CharacterGroupWord})");

		public static readonly Regex MarkDouble = new Regex($@"\\?(?:\[([^\]]+?)\])?##({Patterns.CharacterClassAll}+?)##");

		public static readonly Regex Monospace =
			new Regex(
				$@"(^|[^{Patterns.CharacterClassWord};:""'`}}])(?:\[([^\]]+?)\])?`(\S|\S{Patterns.CharacterClassAll}*?\S)`(?![{Patterns.CharacterClassWord}""'`])");

		public static readonly Regex MonospaceCompatible =
			new Regex(
				$@"(^|[^{Patterns.CharacterClassWord};:}}])(?:\[([^\]]+?)\])?\+(\S|\S{Patterns.CharacterClassAll}*?\S)\+(?!{Patterns.CharacterClassWord})");

		public static readonly Regex MonospaceDouble = new Regex($@"\\?(?:\[([^\]]+?)\])?``({Patterns.CharacterClassAll}+?)``");
		public static readonly Regex MonospaceDoubleCompatible = new Regex($@"\\?(?:\[([^\]]+?)\])?\+\+({Patterns.CharacterClassAll}+?)\+\+");

		public static readonly Regex QuotationDouble =
			new Regex(
				$@"(^|[^{Patterns.CharacterClassWord};:}}])(?:\[([^\]]+?)\])?""`(\S|\S{Patterns.CharacterClassAll}*?\S)`""(?!{Patterns.CharacterGroupWord})");

		public static readonly Regex QuotationSingle =
			new Regex(
				$@"(^|[^{Patterns.CharacterClassWord};:`}}])(?:\[([^\]]+?)\])?'`(\S|\S{Patterns.CharacterClassAll}*?\S)`'(?!{Patterns.CharacterGroupWord})");

		public static readonly Regex Strong =
			new Regex(
				$@"(^|[^{Patterns.CharacterClassWord};:}}])(?:\[([^\]]+?)\])?\*(\S|\S{Patterns.CharacterClassAll}*?\S)\*(?!{Patterns.CharacterGroupWord})");

		public static readonly Regex StrongDouble = new Regex($@"\\?(?:\[([^\]]+?)\])?\*\*({Patterns.CharacterClassAll}+?)\*\*");
		public static readonly Regex Subscript = new Regex($@"\\?(?:\[([^\]]+?)\])?~(\S+?)~");
		public static readonly Regex Superscript = new Regex($@"\\?(?:\[([^\]]+?)\])?\^(\S+?)\^");

		public static readonly List<InlineElementRule> InlineElementRules = new List<InlineElementRule>
		{
			new InlineElementRule(InlineElementType.ImplicitLink, ImplicitLink, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.InternalAnchor, InternalAnchor, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.InlineAnchor, InlineAnchor, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.StrongDouble, StrongDouble, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Strong, Strong, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.QuotationDouble, QuotationDouble, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.Quotation, QuotationSingle, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.MonospaceDouble, MonospaceDouble, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Monospace, Monospace, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.MonospaceDouble, MonospaceDoubleCompatible, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Monospace, MonospaceCompatible, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.EmphasisDouble, EmphasisDouble, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Emphasis, Emphasis, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.MarkDouble, MarkDouble, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Mark, Mark, InlineElementConstraint.Constrained),
			new InlineElementRule(InlineElementType.Superscript, Superscript, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.Subscript, Subscript, InlineElementConstraint.Unconstrained),
			new InlineElementRule(InlineElementType.AttributeReference, AttributeReference, InlineElementConstraint.Unconstrained),
		};

		public static readonly Regex InlineImage = new Regex(@"\\?(?:image|icon):([^:\[][^\[]*)\[((?:\\\]|[^\]])*?)\]");


		public static readonly Regex LabeledListItem =
			new Regex(
				$@"^(?!\/\/){Patterns.CharacterGroupWhitespace}*(?<label>.*?)((?<level>:{{2,4}})|;;)(?:{Patterns.CharacterGroupWhitespace}+(?<text>.*))?$");

		public static readonly Regex Listing = new Regex($"^{Regex.Escape(Patterns.Block.Listing)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex ListItem =
			new Regex($@"^(?<level>(?:\-|\*){{1,5}}){Patterns.CharacterGroupWhitespace}+(?<text>[^(\[\*|\s|x|X\]\s)].*?)$");

		public static readonly Regex ListItemContinuation = new Regex($@"^{Patterns.CharacterGroupWhitespace}*\+{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Literal = new Regex($"^{Regex.Escape(Patterns.Block.Literal)}{Patterns.CharacterGroupWhitespace}*$");


		public static readonly Regex Media =
			new Regex($@"^(?<media>image|video|audio)::(?<path>\S+?)\[(?<attributes>(?:\\\]|[^\]])*?)\]{Patterns.CharacterGroupWhitespace}*$");


		public static readonly Regex OpenBlock = new Regex($"^{Regex.Escape(Patterns.Block.Open)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex OrderedListItem =
			new Regex(
				$@"^{Patterns.CharacterGroupWhitespace}*((?<level>\.{{1,5}})|(?<number>\d+)\.|(?<upperalpha>[A-Z])\.|(?<loweralpha>[a-z])\.|((?<upperroman>[IVX]+)[\)|\.]|(?<lowerroman>[ivx]+)[\)|\.])){Patterns
					.CharacterGroupWhitespace}+(?<text>.*?){Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Pass = new Regex($"^{Regex.Escape(Patterns.Block.Pass)}{Patterns.CharacterGroupWhitespace}*$");


		public static readonly Regex Quote = new Regex($"^{Regex.Escape(Patterns.Block.Quote)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex RevisionInfo = new Regex(@"^(?:\D*(.*?),)?(?:\s*(?!:)(.*?))(?:\s*(?!^):\s*(.*))?$");

		public static readonly Regex SectionTitle =
			new Regex($@"^(?<level>(?:=|#){{1,6}}){Patterns.CharacterGroupWhitespace}+(?<title>\S.*?)(?:{Patterns.CharacterGroupWhitespace}+\1)?$");

		public static readonly Regex Sidebar = new Regex($"^{Regex.Escape(Patterns.Block.Sidebar)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Source = new Regex($"^{Regex.Escape(Patterns.Block.Source)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Stem = new Regex($"^{Regex.Escape(Patterns.Block.Stem)}{Patterns.CharacterGroupWhitespace}*$");


		public static readonly Regex Table = new Regex($"^{Regex.Escape(Patterns.Table.Any)}{Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Title = new Regex($@"^\.(?<title>[^\s.].*){Patterns.CharacterGroupWhitespace}*$");

		public static readonly Regex Verse = new Regex($"^{Regex.Escape(Patterns.Block.Verse)}{Patterns.CharacterGroupWhitespace}*$");

		private static readonly Dictionary<Type, Regex> TypeBlockDelimiterRegexes = new Dictionary<Type, Regex>
		{
			{ typeof(Fenced), Fenced },
			{ typeof(Passthrough), Pass },
			{ typeof(Stem), Stem },
			{ typeof(Source), Source },
			{ typeof(Literal), Literal },
			{ typeof(Comment), CommentBlock },
			{ typeof(Quote), Quote },
			{ typeof(Example), Example },
			{ typeof(Sidebar), Sidebar },
			{ typeof(Listing), Listing },
			{ typeof(Verse), Verse },
			{ typeof(Open), OpenBlock },
		};

		private static readonly Dictionary<Type, string> TypeBlockDelimiters = new Dictionary<Type, string>
		{
			{ typeof(Fenced), Patterns.Block.Fenced },
			{ typeof(Passthrough), Patterns.Block.Pass },
			{ typeof(Stem), Patterns.Block.Stem },
			{ typeof(Source), Patterns.Block.Source },
			{ typeof(Literal), Patterns.Block.Literal },
			{ typeof(Comment), Patterns.Block.Comment },
			{ typeof(Quote), Patterns.Block.Quote },
			{ typeof(Example), Patterns.Block.Example },
			{ typeof(Sidebar), Patterns.Block.Sidebar },
			{ typeof(Listing), Patterns.Block.Listing },
			{ typeof(Verse), Patterns.Block.Verse },
			{ typeof(Open), Patterns.Block.Open }
		};


		public static string GetDelimiterFor<TElement>() where TElement : IElement
		{
			string delimiter;
			if (!TypeBlockDelimiters.TryGetValue(typeof(TElement), out delimiter))
			{
				throw new ArgumentException($"{typeof(TElement).Name} must be a block element", nameof(TElement));
			}
			return delimiter;
		}

		public static Regex GetDelimiterRegexFor<TElement>() where TElement : IElement
		{
			Regex regex;
			if (!TypeBlockDelimiterRegexes.TryGetValue(typeof(TElement), out regex))
			{
				throw new ArgumentException($"{typeof(TElement).Name} must be a block element", nameof(TElement));
			}
			return regex;
		}
	}
}