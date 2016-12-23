using System;
using System.Collections.Generic;

namespace AsciiDocNet
{
    /// <summary>
    /// Specifies the roles for an element.
    /// </summary>
    /// <example>
    /// [.summary]
    /// * Review 1
    /// * Review 2
    /// </example>    
    /// <example>
    /// [role="summary"]
    /// * Review 1
    /// * Review 2
    /// </example>    
    /// <example>
    /// [.summary.main]
    /// * Review 1
    /// * Review 2
    /// </example>    
    /// <example>
    /// [role="summary,main"]
    /// * Review 1
    /// * Review 2
    /// </example>
    /// <seealso cref="AsciiDocNet.NamedAttribute" />
    public class RoleAttribute : NamedAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public RoleAttribute(string value, bool singleQuoted) : base("role", value, singleQuoted)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAttribute"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="singleQuoted">if set to <c>true</c> [single quoted].</param>
        public RoleAttribute(IEnumerable<string> values, bool singleQuoted) : this(string.Join(",", values), singleQuoted)
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