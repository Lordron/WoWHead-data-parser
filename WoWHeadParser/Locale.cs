using System;
using System.Collections.Generic;
namespace WoWHeadParser
{
    public enum Locale : byte
    {
        Old,
        English,
        Russia,
        Germany,
        Spain,
        France,
    }

    public static class LocaleMgr
    {
        private static Dictionary<Locale, string> _wowheadlocalePrefix = new Dictionary<Locale, string>
        {
            {Locale.Old, "old."},
            {Locale.English, "www."},
            {Locale.Russia, "ru."},
            {Locale.Germany, "de."},
            {Locale.France, "fr."},
            {Locale.Spain, "es."},
        };

        public static Uri GetAddress(Locale locale)
        {
            string address = string.Format("http://{0}wowhead.com/", _wowheadlocalePrefix[locale]);
            return new Uri(address);
        }
    }
}