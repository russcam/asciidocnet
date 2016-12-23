using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AsciiDocNet
{
	public enum TableHorizontalAlignment
	{
		[Value("<")]
		Left,

		[Value(">")]
		Right,

		[Value("^")]
		Center
	}

	public static class TableHorizontalAlignmentExtensions
	{
		private static BiDirectionalDictionary<TableHorizontalAlignment, string> Styles;

		static TableHorizontalAlignmentExtensions()
		{
			Styles = ((TableHorizontalAlignment[])Enum.GetValues(typeof(TableHorizontalAlignment)))
				.ToBiDirectionalDictionary(s => s,
					s =>
					{
						var member = typeof(TableHorizontalAlignment).GetMember(s.ToString());
						return member[0].GetCustomAttribute<ValueAttribute>().Value;
					});
		}

		public static string Value(this TableHorizontalAlignment horizontalAlignment)
		{
			string value;
			Styles.TryGetByFirst(horizontalAlignment, out value);
			return value;
		}

		public static bool TryGetValue(string value, out TableHorizontalAlignment horizontalAlignment) => 
			Styles.TryGetBySecond(value, out horizontalAlignment);

		public static bool TryGetValue(TableHorizontalAlignment horizontalAlignment, out string value) =>
			Styles.TryGetByFirst(horizontalAlignment, out value);
	}
}