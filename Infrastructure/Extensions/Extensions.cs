using System;
using System.Collections.Generic;
using System.Globalization;

namespace Infrastructure.Extensions
{
    public static class Extensions
    {
        public static string NullIfWhiteSpaceTrim(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return value.TrimStart('\'').TrimEnd('\'').Trim();
            }
        }

        public static TV GetDictValueOrNull<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default)
        {
            if (key == null)
            {
                return defaultValue;
            }

            return dict.TryGetValue(key, out TV value) ? value : defaultValue;
        }

        public static bool ContainsKeyNullCheck<TK, TV>(this IDictionary<TK, TV> dict, TK key)
        {
            if (key == null)
            {
                return false;
            }

            return dict.ContainsKey(key);
        }

        public static TEnum GetEnumValueString<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value.NullIfWhiteSpaceTrim());
        }

        public static int? GetNullableIntFromString(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int i))
            {
                return i;
            }
            else
            {
                return null;
            }
        }

        public static bool GetBooleanFromString(this string value)
        {
            return Convert.ToBoolean(Convert.ToInt32(value));
        }

        public static bool TryParseEnumValue<TEnum>(this string value)
        {
            var isIntValue = int.TryParse(value, out int intValue);

            if (isIntValue)
            {
                return Enum.IsDefined(typeof(TEnum), intValue);
            }

            return false;
        }

        public static DateTime? GetDateTimeString(this string value, string format = "dd.MM.yyyy")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                DateTime date;
                DateTime.TryParseExact(value.NullIfWhiteSpaceTrim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                return date;
            }
        }
    }
}
