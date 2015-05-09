using Sql;
using System;
using System.Collections.Generic;
using System.Text;
using WoWHeadParser.Properties;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser.Parser
{
    public class PageParser
    {
        public int Flags;

        public ParserData.Parser Parser;

        public Locale Locale;

        public StringBuilder Content = new StringBuilder(1024);

        public PageParser(Locale locale, int flags)
        {
            Flags = flags;
            Locale = locale;
        }

        public virtual void Parse(string page, uint id)
        {
        }

        public void TryParse(string page, uint id)
        {
            try
            {
                Parse(page, id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while parsing: Parser: {0}, Item Id: {1} - {2}", GetType().Name, id, e);
            }
        }

        public override string ToString()
        {
            foreach (SqlBuilder builder in Builders)
            {
                Content.Append(builder.ToString());
            }
            return Content.ToString();
        }

        #region Sql Builder

        public List<SqlBuilder> Builders = new List<SqlBuilder>
            {
                new SqlBuilder()
            };

        public SqlBuilder Builder { get { return Builders[0]; } }

        #endregion

        #region Locales

        private Dictionary<Locale, string> _locales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public bool HasLocales { get { return Locale > Locale.English; } }

        public string LocalePosfix { get { return _locales[Locale]; } }

        #endregion
    }
}