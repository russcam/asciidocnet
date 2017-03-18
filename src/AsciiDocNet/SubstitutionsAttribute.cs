using System.Collections.Generic;
using System.Linq;

namespace AsciiDocNet
{
    // TODO: handle this better...
    /// <summary>
    /// Controls the substitutions that can be made for a block.
    /// </summary>
    /// <seealso cref="AsciiDocNet.NamedAttribute" />
    public class SubstitutionsAttribute : NamedAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SubstitutionsAttribute"/> class.
        /// </summary>
        /// <param name="substitutions">The substitutions.</param>
        public SubstitutionsAttribute(IEnumerable<DelimitedBlockSubstitutions> substitutions)
			: base("subs", string.Join(",", substitutions?.Select(s => s.ToString().ToLowerInvariant())), false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstitutionsAttribute"/> class.
        /// </summary>
        /// <param name="substitutions">The substitutions.</param>
        public SubstitutionsAttribute(string substitutions)
			: this(substitutions.Split(',').Select(s => s.ToEnum<DelimitedBlockSubstitutions>()))
		{
		}

        /// <summary>
        /// Gets or sets the substitutions.
        /// </summary>
        /// <value>
        /// The substitutions.
        /// </value>
        public IEnumerable<DelimitedBlockSubstitutions> Substitutions
		{
			get { return Value?.Split(',').Select(v => v.ToEnum<DelimitedBlockSubstitutions>()); }
			set
			{
			    if (value == null)
			        Value = null;
			    else
			        string.Join(",", value.Select(s => s.ToString().ToLowerInvariant()));
			}
		}
	}
}