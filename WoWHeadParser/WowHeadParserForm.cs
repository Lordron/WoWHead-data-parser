using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WoWHeadParser.DBFileStorage;
using WoWHeadParser.Messages;
using WoWHeadParser.Parser;
using WoWHeadParser.Plugin;
using WoWHeadParser.Properties;
using WoWHeadParser.Serialization;
using System.Linq;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private const string WelfFolder = "EntryList";
        private const string WelfExtension = "*.json";
        private const string DllExtension = "*.Plugin.dll";
        private const string ParserFile = "Parsers.json";

        private const uint MaxIdCountPerRequest = 200;

        private static string[] s_languages = new string[]
        {
            "en-US",
            "ru-RU",
        };

        private Worker m_worker;
        private CultureInfo m_currentCulture;
        private ParserData m_data;

        public WoWHeadParserForm()
        {
            InitializeComponent();

            RichTextBoxWriter.Instance.OutputBox = consoleBox;
        }

        protected override void OnLoad(EventArgs e)
        {
            #region Language loading

            string currentLanguage = Settings.Default.Culture;
            foreach (string language in s_languages)
            {
                MenuItem item = new MenuItem(language, LanguageMenuItemClick) { Checked = currentLanguage.Equals(language) };
                languageMenuItem.MenuItems.Add(item);
            }

            m_data = SerializationHelper.SerializeFile<ParserData>(ParserFile);
            m_currentCulture = new CultureInfo(currentLanguage, true);

            ReloadUILanguage();

            #endregion

            #region Files loading

            DBFileLoader.Initial();
            LoadWelfFiles();

            #endregion

            #region Parsers loading

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(PageParser)))
                    continue;

                ParserAttribute attribute = type.GetCustomAttribute<ParserAttribute>(true);
                foreach (ParserData.Parser parser in m_data.Data)
                {
                    if (parser.ParserType != attribute.ParserType)
                        continue;

                    parser.Type = type;
                    break;
                }
            }

            #endregion

            #region Locale loading

            parserBox.SelectIndex(Settings.Default.LastParser);
            localeBox.SetEnumValues<Locale>(Settings.Default.LastLocale);

            #endregion

            #region Plugins loading

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

            #endregion
        }

        private void PluginMenuItemClick(object sender, EventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            IPlugin plugin = (IPlugin)item.Tag;
            plugin.Run(m_currentCulture);
        }

        private void ParserBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ParserData.Parser data = m_data.Data[parserBox.SelectedIndex];
            welfBox.SelectedItem = data.ParserType.ToString().ToLower();
       
            subparsersListBox.Items.Clear();
            Type subParsers = data.Type.GetNestedType("SubParsers");
            if (subParsers != null)
            {
                foreach (Enum val in Enum.GetValues(subParsers))
                {
                    subparsersListBox.Items.Add(val, true);
                }
            }
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            ParserData.Parser data = m_data.Data[parserBox.SelectedIndex];
            ConstructorInfo cInfo = data.Type.GetConstructor(new[] { typeof(Locale), typeof(int) });
            if (cInfo == null)
                return;

            int flags = GetSubparsers();
            PageParser parser = (PageParser)cInfo.Invoke(new [] { localeBox.SelectedItem, flags });
            if (parser == null)
                throw new InvalidOperationException("parser");

            parser.Parser = data;

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            m_worker = new Worker(type, parser, WorkerPageDownloaded);

            Worker.ParserValue value = default(Worker.ParserValue);
            switch (type)
            {
                case ParsingType.TypeBySingleValue:
                    {
                        value.Id = (uint)valueBox.Value;
                        break;
                    }
                case ParsingType.TypeByList:
                    {
                        value.Array = GetEntriesList().Data;
                        numericUpDown.Maximum = progressBar.Maximum = value.Array.Length;
                        break;
                    }
                case ParsingType.TypeByMultipleValue:
                    {
                        value.Start = (uint)rangeStart.Value;
                        value.End = (uint)rangeEnd.Value;

                        if (value.Start > value.End)
                        {
                            ShowMessageBox(MessageType.MultipleTypeBigger);
                            return;
                        }

                        if (value.Start == value.End)
                        {
                            ShowMessageBox(MessageType.MultipleTypeEqual);
                            return;
                        }

                        numericUpDown.Maximum = progressBar.Maximum = (int)(value.End - value.Start) + 1;
                        break;
                    }
                case ParsingType.TypeByWoWHeadFilter:
                    {
                        value.Maximum = (data.CountLimit / MaxIdCountPerRequest);
                        numericUpDown.Maximum = progressBar.Maximum = (int)value.Maximum + 1;
                        break;
                    }
                default:
                    return;
            }

            m_worker.SetValue(value);

            abortButton.Enabled = true;
            subparsersListBox.Enabled = settingsBox.Enabled = startButton.Enabled = false;
            numericUpDown.Value = progressBar.Value = 0;
            SetLabelText(Resources.Label_Working);

            Requests.Compress = Settings.Default.DataCompression;

            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            m_worker.Start();
        }

        private void WorkerPageDownloaded(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(_ =>
                {
                    ++progressBar.Value;
                    ++numericUpDown.Value;
                });
        }

        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("Error! {0}", e.Error);
            }
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter stream = new StreamWriter(saveDialog.FileName, Settings.Default.Append, Encoding.UTF8))
                {
                    stream.Write(m_worker);
                }
            }

            abortButton.Enabled = false;
            subparsersListBox.Enabled = settingsBox.Enabled = startButton.Enabled = true;
            numericUpDown.Value = progressBar.Value = 0;

            m_worker.Reset();

            SetLabelText(Resources.Label_Complete);
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (ShowMessageBox(MessageType.AbortQuestion) != DialogResult.Yes)
                return;

            m_worker.Stop();
            abortButton.Enabled = false;
            SetLabelText(Resources.Label_Abort);
        }

        private void WelfBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            entryCountLabel.Text = string.Format(Resources.EntryCountLabel, GetEntriesList().Count);
        }

        private void OptionsMenuItemClick(object sender, EventArgs e)
        {
            new SettingsForm(m_currentCulture).ShowDialog();
        }

        private void AboutMenuItemClick(object sender, EventArgs e)
        {
            new AboutForm(m_currentCulture).ShowDialog();
        }

        private void ReloadWelfFilesButtonClick(object sender, EventArgs e)
        {
            LoadWelfFiles();
        }

        private void LanguageMenuItemClick(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null)
                return;

            CultureInfo selectedCulture = new CultureInfo(item.Text, true);
            if (m_currentCulture.Equals(selectedCulture))
                return;
            
            foreach (MenuItem menu in languageMenuItem.MenuItems)
            {
                menu.Checked = menu == item;
            }

            Settings.Default.Culture = item.Text;
            m_currentCulture = selectedCulture;

            ReloadUILanguage();
        }

        private void ExitMenuClick(object sender, EventArgs e)
        {
            Application.Exit();
            Save();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = ShowMessageBox(MessageType.ExitQuestion);
            e.Cancel = (result == DialogResult.No);
        }

        protected override void OnClosed(EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            Settings.Default.LastLocale = localeBox.SelectedIndex;
            Settings.Default.LastParser = parserBox.SelectedIndex;
            Settings.Default.Save();
        }

        private void SetLabelText(string text)
        {
            progressLabel.ThreadSafeBegin(x => x.Text = text);
        }

        private void ReloadUILanguage()
        {
            ReloadUILanguage(m_currentCulture);

            int selectedIndex = parserBox.SelectedIndex;

            parserBox.Items.Clear();

            foreach (ParserData.Parser data in m_data.Data)
            {
                parserBox.Items.Add(GetNameByParserType(data.ParserType));
            }

            parserBox.SelectedIndex = selectedIndex;
        }

        #region Parsers names

        private string GetNameByParserType(ParserType Type)
        {
            switch (Type)
            {
                case ParserType.Page:
                    return Resources.PageParser;
                case ParserType.Item:
                    return Resources.ItemParser;
                case ParserType.Npc:
                    return Resources.NpcDataParser;
                case ParserType.NpcLocale:
                    return Resources.NpcLocaleParser;
                case ParserType.Quest:
                    return Resources.QuestDataParser;
                case ParserType.QuestLocale:
                    return Resources.QuestLocaleParser;
                case ParserType.Trainer:
                    return Resources.TrainerParser;
                case ParserType.Vendor:
                    return Resources.VendorParser;
                default:
                    throw new InvalidOperationException(Type.ToString());
            }
        }

        #endregion

        private void LoadWelfFiles()
        {
            welfBox.Items.Clear();

            string[] files = Directory.GetFiles(WelfFolder, WelfExtension, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                welfBox.Items.Add(fileName);
            }

            if (welfBox.Items.Count > 0)
            welfBox.SelectIndex(0);
        }

        private DialogResult ShowMessageBox(MessageType type, params object[] args)
        {
            return MessageManager.ShowMessage(type, Text, args);
        }

        private int GetSubparsers()
        {
            int mask = 0;
            for (int i = 0; i < subparsersListBox.Items.Count; ++i)
            {
                if (subparsersListBox.GetItemChecked(i))
                    mask += 1 << i;
            }

            return mask;
        }

        private Entries GetEntriesList()
        {
            string path = string.Format("{0}\\{1}.json", WelfFolder, welfBox.SelectedItem);
            if (!File.Exists(path))
            {
                ShowMessageBox(MessageType.WelfFileNotFound, path);
                return null;
            }

            try
            {
                return SerializationHelper.SerializeFile<Entries>(path);
            }
            catch (Exception exception)
            {
                Console.WriteLine(Resources.Error_while_loading_welf_file, path, exception.Message);
                return null;
            }
        }
    }
}