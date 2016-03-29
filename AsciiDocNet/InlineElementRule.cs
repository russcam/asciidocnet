using System.Text.RegularExpressions;

namespace AsciidocNet
{
	public class InlineElementRule
	{
		public InlineElementType ElementType { get; }

		public Regex Regex { get; }

		public InlineElementConstraint Constraint { get; }

		public InlineElementRule(InlineElementType elementType, Regex regex, InlineElementConstraint constraint)
		{
			ElementType = elementType;
			Regex = regex;
			Constraint = constraint;
		}
	}
}