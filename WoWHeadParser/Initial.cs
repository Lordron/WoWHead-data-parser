using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WoWHeadParser.Parser;
using WoWHeadParser.Plugin;
using WoWHeadParser.Properties;
using WoWHeadParser.Serialization;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        [AutoInitial(Order = 0)]
        private void InitialLanguage()
        {
            string currentLanguage = Settings.Default.Culture;
            foreach (string language in s_languages)
            {
                MenuItem item = new MenuItem(language, LanguageMenuItemClick) { Checked = currentLanguage.Equals(language) };
                languageMenuItem.MenuItems.Add(item);
            }

            m_data = SerializationHelper.SerializeFile<ParserData>(ParserFile);
            m_currentCulture = new CultureInfo(currentLanguage, true);

            ReloadUILanguage();
        }

        [AutoInitial(Order = 3)]
        private void InitialParsers()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(PageParser)))
                    continue;

                ParserAttribute attribute = type.GetCustomAttribute<ParserAttribute>(true);
                if (attribute == null)
                    continue;

                ConstructorInfo cInfo = type.GetConstructor(new[] { typeof(Locale), typeof(int) });
                if (cInfo == null)
                    continue;

                PageParser parser = (PageParser)cInfo.Invoke(new object [] { Locale.English, 0 });
                if (parser == null)
                    throw new InvalidOperationException("parser");

                if (m_parsers.ContainsKey(attribute.ParserType))
                {
                    Console.WriteLine("There is another parser for ParserType.{0}", attribute.ParserType);
                    continue;
                }

                m_parsers[attribute.ParserType] = parser;
            }
        }

        [AutoInitial(Order = 4)]
        private void InitialLocale()
        {
            parserBox.SelectIndex(Settings.Default.LastParser);
            localeBox.SetEnumValues<Locale>(Settings.Default.LastLocale);
        }
        [AutoInitial(Order = 5)]

        private void InitialPlugins()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), DllExtension, SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                Assembly assembly = Assembly.LoadFile(file);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetInterface(typeof(IPlugin).Name, true) == null)
                        continue;

                    IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                    if (plugin == null)
                        continue;

                    MenuItem item = new MenuItem(plugin.Name, PluginMenuItemClick) { Tag = plugin };
                    editMenuItem.MenuItems.Add(item);
                }
            }
        }
    }
}
