using System;

namespace AsciiDocNet
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ValueAttribute : System.Attribute
	{
		public string Value { get; }

		public ValueAttribute(string value)
		{
			Value = value;
		}
	}
}