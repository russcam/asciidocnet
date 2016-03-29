namespace AsciidocNet
{
	/// <summary>
	/// An <see cref="AttributeEntry"/> that has been unset
	/// </summary>
	public class UnsetAttributeEntry : AttributeEntry
	{
		public UnsetAttributeEntry(string name) : base(name, null)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}