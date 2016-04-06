namespace AsciiDocNet
{
	public static class Patterns
	{
		public const string CharacterClassAll = @"[\s\S]";
		public const string CharacterClassAlpha = "a-zA-Z";
		public const string CharacterClassWord = CharacterClassAlpha + "0-9_";
		public const string CharacterGroupWhitespace = "[ \t]";
		public const string CharacterGroupWord = "[" + CharacterClassWord + "]";

		public static readonly string[] Admonitions = { "NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION" };
		public static readonly string[] LowerAlphabet = "abcdefghijklmnopqrstuvwxyz".Split();
		public static readonly string[] UpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Split();

		public static class Block
		{
			public const string Comment = "////";
			public const string Example = "====";
			public const string Fenced = "```";
			public const string Listing = "----";
			public const string Literal = "....";
			public const string Open = "--";
			public const string Pass = Stem;
			public const string Quote = "____";
			public const string Sidebar = "****";
			public const string Source = Listing;
			public const string Stem = "++++";
			public const string Verse = Quote;
		}

		public static class Table
		{
			public const string Any = @"(\||,|\:|\!])===";
			public const string Colon = ":===";
			public const string Comma = ",===";
			public const string Exclamation = "!===";
			public const string Pipe = "|===";
		}
	}
}