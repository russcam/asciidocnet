using System;
using System.Collections.Generic;

namespace AsciiDocNet
{
	internal static class BiDirectionalDictionaryExtensions
	{
		public static BiDirectionalDictionary<TFirst, TSecond> ToBiDirectionalDictionary<TSource, TFirst, TSecond>(
			this IEnumerable<TSource> source, 
			Func<TSource, TFirst> firstSelector, 
			Func<TSource, TSecond> secondSelector)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (firstSelector == null) throw new ArgumentNullException(nameof(firstSelector));
			if (secondSelector == null) throw new ArgumentNullException(nameof(secondSelector));
			var dictionary = new BiDirectionalDictionary<TFirst, TSecond>();
			foreach (var element in source)
			{
				dictionary.Add(firstSelector(element), secondSelector(element));
			}
			return dictionary;
		}
	}
}