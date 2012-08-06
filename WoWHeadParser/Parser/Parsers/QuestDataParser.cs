using System;
using System.Text.RegularExpressions;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Quest)]
    internal class QuestDataParser : PageParser
    {
        private SubParsers parsers;

        public QuestDataParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "quests?filter=cr=30:30;crs=1:4;crv={0}:{1}";
            this.MaxCount = 32000;

            parsers = (SubParsers)flags;
        }

        private const string pattern = @"<div id=""lknlksndgg-(?<type>\w+)"" style=""display: none"">(?<text>.+)\n";
        private Regex dataRegex = new Regex(pattern);

        public override void Parse(string page, uint id)
        {
            /*SqlBuilder builder;
            if (HasLocales)
            {
                builder = new SqlBuilder("quest_template", "id");
                builder.SetFieldsNames("RequestItemsText", "OfferRewardText");
            }
            else
            {
                builder = new SqlBuilder("locales_quest");
                builder.SetFieldsNames(string.Format("RequestItemsText_{0}", LocalePosfix), string.Format("OfferRewardText_{0}", LocalePosfix));
            }

            MatchCollection matches = dataRegex.Matches(page);
            foreach (Match item in matches)
            {
                GroupCollection groups = item.Groups;
                string type = groups["type"].Value ?? string.Empty;
                string text = groups["text"].Value ?? string.Empty;

                switch (type.ToLower())
                {
                    case "progress":
                        if (parsers.HasFlag(SubParsers.RequestItemsText))
                            builder.AppendFieldsValue(id, text.HTMLEscapeSumbols());
                        break;
                    case "completion":
                        if (parsers.HasFlag(SubParsers.OfferRewardText))
                            builder.AppendFieldsValue(id, text.HTMLEscapeSumbols());
                        break;
                }
            }

            return new PageItem(id, builder.ToString());*/
        }

        [Flags]
        public enum SubParsers : uint
        {
            RequestItemsText = 0x0001,
            OfferRewardText = 0x0002,
        }
    }
}