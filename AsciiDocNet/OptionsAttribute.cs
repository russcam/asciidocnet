using System;
using System.Collections.Generic;

namespace AsciidocNet
{
	public class OptionsAttribute : NamedAttribute
	{
		public OptionsAttribute(string value, bool singleQuoted) : base("options", value, singleQuoted)
		{
		}

		public OptionsAttribute(IEnumerable<string> values, bool singleQuoted) : this(string.Join(",", values), singleQuoted)
		{
		}

		public string[] Values
		{
			get { return Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
			set { Value = string.Join(",", value); }
		}
	}
}