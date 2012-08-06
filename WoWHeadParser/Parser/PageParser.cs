using System;
using System.Collections.Generic;
using System.Text;
using Sql;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser
{
    public class PageParser
    {
        public int Flags;

        public int MaxCount;

        public string Address;

        public StringBuilder Content = new StringBuilder(1024);

        public PageParser(Locale locale, int flags)
        {
            Flags = flags;
            Locale = locale;
        }

        public virtual void Parse(string page, uint id)
        {
        }

        public virtual void PutStringData()
        {
            Content.Append(Builder.ToString());
        }

        public void TryParse(string page, uint id)
        {
            //try
            {
                Parse(page, id);
            }
            //catch (Exception e)
            {
                //Console.WriteLine("Error while parsing: Parser: {0}, Item Id: {1} - {2}", GetType().Name, id, e);
            }
        }

        public override string ToString()
        {
            PutStringData();
            return Content.ToString();
        }

        #region Sql Builder

        protected static SqlBuilderSettings _builderSettings = new SqlBuilderSettings(Settings.Default.QueryType, Settings.Default.WithoutHeader, Settings.Default.AllowEmptyValues, Settings.Default.AppendDeleteQuery);

        public SqlBuilder Builder = new SqlBuilder(_builderSettings);

        #endregion

        #region Locales

        public Locale Locale
        {
            get { return _locale > Locale.Old ? _locale : Locale.English; }
            set { _locale = value; }
        }

        private Locale _locale;

        private Dictionary<Locale, string> _locales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public bool HasLocales { get { return _locale > Locale.English; } }

        public string LocalePosfix { get { return _locales[Locale]; } }

        #endregion
    }
}