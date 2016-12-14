using System.Text.RegularExpressions;

namespace AsciiDocNet
{
	internal class ParsingContext
	{
		public ParsingContext(Container parent, Regex regex)
		{
			Parent = parent;
			Regex = regex;
		}

		public Container Parent { get; }

		public Regex Regex { get; }

		public bool IsMatch(string input)
		{
			return Regex?.IsMatch(input) ?? false;
		}
	}
}