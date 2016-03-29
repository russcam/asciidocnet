using System.Collections.Generic;

namespace AsciidocNet
{
	public static class Substitutor
	{
		public static IReadOnlyDictionary<string, string> SpecialCharacters => 
			new Dictionary<string, string> {
				{ "&", "&amp;"},
				{ "<", "&lt;"},
				{ ">", "&gt;"},
			}; 
	}
}