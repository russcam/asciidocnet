using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AsciiDocNet
{
	public static class TableStyleExtensions
	{
		private static BiDirectionalDictionary<TableStyle, string> Styles;

		static TableStyleExtensions()
		{
			Styles = ((TableStyle[])Enum.GetValues(typeof(TableStyle)))
				.ToBiDirectionalDictionary(s => s,
					s =>
					{
						var member = typeof(TableStyle).GetMember(s.ToString());
						return member[0].GetCustomAttribute<ValueAttribute>().Value;
					});
		}

		public static string Value(this TableStyle style)
		{
			string value;
			Styles.TryGetByFirst(style, out value);
			return value;
		}

		public static bool TryGetValue(string value, out TableStyle style) =>
			Styles.TryGetBySecond(value, out style);

		public static bool TryGetValue(TableStyle style, out string value) =>
			Styles.TryGetByFirst(style, out value);
	}
}