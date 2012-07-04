using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WoWHeadParser.Page;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser
{
    public class DataParser
    {
        public DataParser()
        {
            Prepare();
        }

        #region Virtual

        public virtual void Prepare()
        {
        }

        public virtual string PreParse()
        {
            return string.Empty;
        }

        public virtual PageItem Parse(string page, uint id)
        {
            return new PageItem(id, page);
        }

        public virtual string Address { get { return string.Empty; } }

        public virtual string Name { get { return string.Empty; } }

        public virtual int MaxCount { get { return 0; } }

        public virtual string WelfName { get { return string.Empty; } }

        #endregion

        public List<PageItem> Items = new List<PageItem>(2048);

        public bool TryParse(string page, uint id)
        {
            PageItem item;
            try
            {
                item = Parse(page, id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while parsing: Parser: {0}, Item: {1} - {2}", GetType().Name, id, e);
                return false;
            }
            Items.Add(item);
            return true;
        }

        public void Sort()
        {
            SortOrder sortOrder = (SortOrder)Settings.Default.SortOrder;
            if (sortOrder > SortOrder.None)
                Items.Sort(new PageItemComparer(sortOrder));
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(Items.Count * 1024);

            string preParse = PreParse().TrimStart();
            if (!string.IsNullOrEmpty(preParse))
                content.Append(preParse);

            Items.ForEach(x => content.Append(x.ToString()));
            return content.ToString();
        }

        #region Locales

        private Locale _locale;

        public Locale Locale 
        {
            get { return _locale > Locale.Old ? _locale : Locale.English; }
            set { _locale = value; }
        }

        public Dictionary<Locale, string> Locales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public bool HasLocales { get { return _locale > Locale.English; } }

        public string LocalePosfix { get { return Locales[Locale]; } }

        #endregion
    }
}