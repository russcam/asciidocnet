using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AsciiDocNet
{
	public enum TableVerticalAlignment
	{
		[Value("<")]
		Top,

		[Value(">")]
		Bottom,

		[Value("^")]
		Middle
	}

	public static class TableVerticalAlignmentExtensions
	{
		private static BiDirectionalDictionary<TableVerticalAlignment, string> Styles;

		static TableVerticalAlignmentExtensions()
		{
			Styles = ((TableVerticalAlignment[])Enum.GetValues(typeof(TableVerticalAlignment)))
				.ToBiDirectionalDictionary(s => s,
					s =>
					{
						var member = typeof(TableVerticalAlignment).GetMember(s.ToString());
						return member[0].GetCustomAttribute<ValueAttribute>().Value;
					});
		}

		public static string Value(this TableVerticalAlignment verticalAlignment)
		{
			string value;
			Styles.TryGetByFirst(verticalAlignment, out value);
			return value;
		}

		public static bool TryGetValue(string value, out TableVerticalAlignment verticalAlignment) =>
			Styles.TryGetBySecond(value, out verticalAlignment);

		public static bool TryGetValue(TableVerticalAlignment verticalAlignment, out string value) =>
			Styles.TryGetByFirst(verticalAlignment, out value);
	}
}