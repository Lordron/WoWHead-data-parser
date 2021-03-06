﻿using Sql;
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

        public Type FlagsType;

        public ParserData.Parser Parser;

        public Locale Locale;

        public StringBuilder Content = new StringBuilder(1024);

        public PageParser(Locale locale, int flags, int size = 1, Type type = null)
            : this(size, type)
        {
            Flags = flags;
            Locale = locale;
        }

        protected PageParser(int size, Type type)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");

            Builders = new SqlBuilder[size];
            for (int i = 0; i < size; ++i)
            {
                Builders[i] = new SqlBuilder();
            }

            if (type != null)
            {
                if (!type.IsEnum)
                    throw new InvalidOperationException("Type passed into PageParser is not enum type!");
            }

            FlagsType = type;
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

        public SqlBuilder[] Builders;

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