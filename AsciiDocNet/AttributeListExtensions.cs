using System;
using System.Linq;

namespace AsciidocNet
{
	public static class AttributeListExtensions
	{
		public static bool ContainBlockName(this AttributeList attributes, string blockName)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return false;
			}
			return attributes.Any(a => a.Name.Equals(blockName));
		}

		public static bool ContainBlockName(this AttributeList attributes, string[] blockNames)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return false;
			}
			return attributes.Any(a => blockNames.Contains(a.Name, StringComparer.InvariantCultureIgnoreCase));
		}
	}
}