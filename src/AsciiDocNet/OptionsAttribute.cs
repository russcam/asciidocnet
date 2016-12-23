using System;
using System.Collections.Generic;

namespace AsciiDocNet
{
    /// <summary>
    /// Specifies the options for the element.
    /// </summary>
    /// <seealso cref="AsciiDocNet.NamedAttribute" />
    public class OptionsAttribute : NamedAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public OptionsAttribute(string value, bool singleQuoted) : base("options", value, singleQuoted)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsAttribute"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public OptionsAttribute(IEnumerable<string> values, bool singleQuoted) 
            : this(string.Join(",", values), singleQuoted)
		{
		}

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string[] Values
		{
			get { return Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
			set { Value = string.Join(",", value); }
		}
	}
}