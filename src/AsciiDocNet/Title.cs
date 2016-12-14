namespace AsciiDocNet
{
	public class Title
	{
		public Title(string text)
		{
			Text = text;
		}

		public string Text { get; set; }

		public static bool operator ==(Title left, Title right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Title left, Title right)
		{
			return !Equals(left, right);
		}

		public virtual TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : IDocumentVisitor
		{
			visitor.Visit(this);
			return visitor;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((Title)obj);
		}

		public override int GetHashCode()
		{
			return Text?.GetHashCode() ?? 0;
		}

		protected bool Equals(Title other)
		{
			return string.Equals(Text, other.Text);
		}
	}
}