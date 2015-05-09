using System;
using System.Collections.Generic;
namespace WoWHeadParser
{
    public enum Locale : byte
    {
        [LocalePrefix("www")]
        English,
        [LocalePrefix("ru")]
        Russia,
        [LocalePrefix("de")]
        Germany,
        [LocalePrefix("es")]
        Spain,
        [LocalePrefix("fr")]
        France,
    }

    public static class LocaleHelper
    {
        public static string GetLocalePrefix(this Enum value)
        {
            LocalePrefixAttribute attribute = value.GetAttributeOfType<LocalePrefixAttribute>();
            return attribute == null ? "www" : attribute.Prefix;
        }
    }
}