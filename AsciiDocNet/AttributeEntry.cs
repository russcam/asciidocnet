namespace AsciidocNet
{
	/// <summary>
	///     An attribute set in the header of the document
	/// </summary>
	public class AttributeEntry : Attribute, IElement
	{
		public AttributeEntry(string name, string value) : base(name)
		{
			Value = value;
		}

		public string Value { get; set; }

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}

		public Container Parent { get; set; }
	}
}