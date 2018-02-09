using System;
using System.Linq;

namespace AsciiDocNet
{
    /// <summary>
    /// Extension methods for <see cref="AttributeList"/>
    /// </summary>
    public static class AttributeListExtensions
	{
        /// <summary>
        /// Determines whether the attributes has and attribute with the given name
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="blockNames">Names of the blocks,</param>
        /// <returns></returns>
        public static bool ContainBlockName(this AttributeList attributes, params string[] blockNames)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return false;
			}
			return attributes.Any(a => blockNames.Contains(a.Name, StringComparer.OrdinalIgnoreCase));
		}
	}
}