namespace AsciiDocNet
{
	public class Audio : Media
	{
		protected bool Equals(Audio other)
		{
			return base.Equals(other);
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
			return Equals((Audio)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Audio left, Audio right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Audio left, Audio right)
		{
			return !Equals(left, right);
		}

		public Audio(string path) : base(path)
		{
		}

		public override TVisitor Accept<TVisitor>(TVisitor visitor)
		{
			visitor.Visit(this);
			return visitor;
		}
	}
}