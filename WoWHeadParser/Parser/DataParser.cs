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

        public List<PageItem> Items = new List<PageItem>(2048);

        public bool TryParse(string page, uint id)
        {
            try
            {
                PageItem item = Parse(page, id);
                if (item == null)
                    return false;

                Items.Add(item);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in parsing items # {0}: {1}", id, e);
                return false;
            }
        }

        public void Sort()
        {
            SortOrder sortOrder = (SortOrder)Settings.Default.SortOrder;
            if (sortOrder > SortOrder.None)
                Items.Sort(new PageItemComparer(sortOrder));
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder(Items.Count * 2048);

            string preParse = PreParse().TrimStart();
            if (!string.IsNullOrEmpty(preParse))
                content.Append(preParse);

            for (int i = 0; i < Items.Count; ++i)
            {
                content.Append(Items[i].ToString());
            }
            return content.ToString();
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

        public string LocalePosfix { get { return Locales[Locale]; } }

        #endregion
    }
}