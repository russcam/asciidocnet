namespace AsciiDocNet
{
	public class IdAttribute : NamedAttribute
	{
		public IdAttribute(string value, bool singleQuoted) : base("id", value, singleQuoted)
		{
		}

		public IdAttribute(string value, string reference, bool singleQuoted) : base("id", value, singleQuoted)
		{
			Reference = reference;
		}

		public string Reference { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}