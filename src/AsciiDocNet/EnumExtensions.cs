using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AsciiDocNet
{
    /// <summary>
    /// Houses extension methods for enums
    /// </summary>
    internal static class EnumExtensions
    {
        internal static ConcurrentDictionary<string, object> EnumCache = new ConcurrentDictionary<string, object>();

        internal static T ToEnum<T>(this string input) where T : struct
        {
            var enumType = typeof(T);
            var key = $"{enumType.Name}.{input}";

	        if (EnumCache.TryGetValue(key, out var cachedValue))
            {
                return (T)cachedValue;
            }

            foreach (var name in Enum.GetNames(enumType))
            {
                if (name.Equals(input, StringComparison.OrdinalIgnoreCase))
                {
                    var value = (T)Enum.Parse(enumType, name, true);
                    EnumCache.TryAdd(key, value);
                    return value;
                }

                var enumFieldInfo = enumType.GetRuntimeField(name);
                var enumMemberAttribute = enumFieldInfo.GetCustomAttribute<DisplayAttribute>();

                if (enumMemberAttribute != null && enumMemberAttribute.Name == input)
                {
                    var value = (T)Enum.Parse(enumType, name);
                    EnumCache.TryAdd(key, value);
                    return value;
                }
            }

            throw new ArgumentException($"cannot parse enum from string {input}", nameof(input));
        }
    }
}