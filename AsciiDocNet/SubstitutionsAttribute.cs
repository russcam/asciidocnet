using System.Collections.Generic;
using System.Linq;

namespace AsciidocNet
{
	// TODO: handle this better...
	public class SubstitutionsAttribute : NamedAttribute
	{
		public SubstitutionsAttribute(IEnumerable<DelimitedBlockSubstitutions> substitutions)
			: base("subs", string.Join(",", substitutions?.Select(s => s.ToString().ToLowerInvariant())), false)
		{
		}

		public SubstitutionsAttribute(string substitutions)
			: this(substitutions.Split(',').Select(s => s.ToEnum<DelimitedBlockSubstitutions>()))
		{
		}

		public IEnumerable<DelimitedBlockSubstitutions> Substitutions
		{
			get { return Value?.Split(',').Select(v => v.ToEnum<DelimitedBlockSubstitutions>()); }
			set
			{
				if (value == null)
				{
					Value = null;
				}
				else
				{
					string.Join(",", value.Select(s => s.ToString().ToLowerInvariant()));
				}
			}
		}
	}
}