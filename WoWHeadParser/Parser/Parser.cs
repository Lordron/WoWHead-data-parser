using System;
using System.Collections.Generic;
namespace WoWHeadParser
{
    public abstract class Parser
    {
        public abstract string PreParse();

        public abstract string Parse(PageItem block);

        public abstract string Address { get; }

        public abstract string Name { get; }

        public abstract int MaxCount { get; }

        public string SafeParser(PageItem item)
        {
            try
            {
                return Parse(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        #region Locales

        public Locale Locale = Locale.English;

        public Dictionary<Locale, string> Locales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public bool HasLocales { get { return Locale > Locale.English; } }

        #endregion
    }
}