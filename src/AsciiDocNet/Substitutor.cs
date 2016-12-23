using System.Collections.Generic;

namespace AsciiDocNet
{
	internal static class Substitutor
	{
		public static IReadOnlyDictionary<string, string> SpecialCharacters => 
			new Dictionary<string, string> {
				{ "&", "&amp;"},
				{ "<", "&lt;"},
				{ ">", "&gt;"},
			}; 
	}
}