using System.Linq;

namespace AsciiDocNet
{
	public class AuthorInfoAttributeEntry : AttributeEntry
	{
		public AuthorInfoAttributeEntry(string value) : base("author", value)
		{
			var values = value.Split(' ');

			if (values.Length == 1)
			{
				FirstName = values[0];
			}
			else if (values.Length == 2)
			{
				FirstName = values[0];
				LastName = values[1];
			}
			else
			{
				FirstName = values[0];
				LastName = values[values.Length - 1];
				MiddleName = string.Join(" ", values.Skip(1).Take(values.Length - 2));
			}
		}

		public string FirstName { get; }

		public string LastName { get; }

		public string MiddleName { get; }
	}
}