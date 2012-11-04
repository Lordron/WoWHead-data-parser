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

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Worker _worker;
        private Dictionary<int, KeyValuePair<ParserType, Type>> _parsers = new Dictionary<int, KeyValuePair<ParserType, Type>>((int)ParserType.Max);

        private CultureInfo _currentCulture;

        private const string WelfFolder = "EntryList";
        private const string WelfExtension = "*.welf";
        private const string DllExtension = "*.Plugin.dll";

        #region Language

        private string[] _language = new string[]
        {
            "en-US",
            "ru-RU",
        };

        #endregion

        public WoWHeadParserForm()
        {
            InitializeComponent();

            RichTextBoxWriter.Instance.OutputBox = consoleBox;
        }

        protected override void OnLoad(EventArgs e)
        {
            #region Language loading

            string stringCulture = Settings.Default.Culture;
            foreach (string language in _language)
            {
                MenuItem item = new MenuItem(language, LanguageMenuItemClick) { Checked = stringCulture.Equals(language) };
                languageMenuItem.MenuItems.Add(item);
            }

            _currentCulture = new CultureInfo(stringCulture, true);

            ReloadUILanguage();

            #endregion

            #region Files loading

            DBFileLoader.Initial();
            LoadWelfFiles();

            #endregion

            #region Parsers loading

            int loadedParsers = 0;
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(PageParser)))
                    continue;

                ParserAttribute[] attributes = type.GetCustomAttributes(typeof(ParserAttribute), true) as ParserAttribute[];
                if (attributes == null || attributes.Length < 1)
                    throw new InvalidOperationException(); // Each parsers should be marked with this attribute

                parserBox.Items.Add(GetNameByParserType(attributes[0].Type));

                _parsers.Add(loadedParsers++, new KeyValuePair<ParserType, Type>(attributes[0].Type, type));
            }

            parserBox.SelectIndex(Settings.Default.LastParser);

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
            plugin.Run(_currentCulture);
        }

        private void ParserBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = parserBox.SelectedIndex;
            welfBox.SelectedItem = _parsers[selectedIndex].Key.ToString().ToLower();
       
            subparsersListBox.Items.Clear();
            Type subParsers = _parsers[selectedIndex].Value.GetNestedType("SubParsers");
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
            ConstructorInfo cInfo = _parsers[parserBox.SelectedIndex].Value.GetConstructor(new[] { typeof(Locale), typeof(int) });
            if (cInfo == null)
                return;

            int flags = GetSubparsers();
            PageParser parser = (PageParser)cInfo.Invoke(new [] { localeBox.SelectedItem, flags });
            if (parser == null)
                throw new InvalidOperationException("parser");

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            _worker = new Worker(type, parser, WorkerPageDownloaded);

            switch (type)
            {
                case ParsingType.TypeBySingleValue:
                    {
                        uint value = (uint)valueBox.Value;
                        _worker.SetValue(value);
                        break;
                    }
                case ParsingType.TypeByList:
                    {
                        uint[] entries = GetEntriesList();
                        numericUpDown.Maximum = progressBar.Maximum = entries.Length;
                        _worker.SetValue(entries);
                        break;
                    }
                case ParsingType.TypeByMultipleValue:
                    {
                        uint startValue = (uint)rangeStart.Value;
                        uint endValue = (uint)rangeEnd.Value;

                        if (startValue > endValue)
                        {
                            ShowMessageBox(MessageType.MultipleTypeBigger);
                            return;
                        }

                        if (startValue == endValue)
                        {
                            ShowMessageBox(MessageType.MultipleTypeEqual);
                            return;
                        }

                        numericUpDown.Maximum = progressBar.Maximum = (int)(endValue - startValue) + 1;
                        _worker.SetValue(startValue, endValue);
                        break;
                    }
                case ParsingType.TypeByWoWHeadFilter:
                    {
                        int maxValue = (parser.MaxCount / 200);
                        numericUpDown.Maximum = progressBar.Maximum = maxValue + 1;
                        _worker.SetValue((uint)maxValue);
                        break;
                    }
                default:
                    return;
            }

            abortButton.Enabled = true;
            subparsersListBox.Enabled = settingsBox.Enabled = startButton.Enabled = false;
            numericUpDown.Value = progressBar.Value = 0;
            SetLabelText(Resources.Label_Working);

            Requests.Compress = Settings.Default.DataCompression;

            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            _worker.Start();
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
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter stream = new StreamWriter(saveDialog.FileName, Settings.Default.Append, Encoding.UTF8))
                {
                    stream.Write(_worker);
                }
            }

            abortButton.Enabled = false;
            subparsersListBox.Enabled = settingsBox.Enabled = startButton.Enabled = true;
            numericUpDown.Value = progressBar.Value = 0;

            _worker.Reset();

            SetLabelText(Resources.Label_Complete);
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (ShowMessageBox(MessageType.AbortQuestion) != DialogResult.Yes)
                return;

            _worker.Stop();
            abortButton.Enabled = false;
            SetLabelText(Resources.Label_Abort);
        }

        private void WelfBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string path = string.Format("{0}\\{1}.welf", WelfFolder, welfBox.SelectedItem);
            if (!File.Exists(path))
            {
                ShowMessageBox(MessageType.WelfFileNotFound, path);
                return;
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                entryCountLabel.Text = string.Format(Resources.EntryCountLabel, count);
            }
        }

        private void OptionsMenuItemClick(object sender, EventArgs e)
        {
            new SettingsForm(_currentCulture).ShowDialog();
        }

        private void AboutMenuItemClick(object sender, EventArgs e)
        {
            new AboutForm(_currentCulture).ShowDialog();
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
            if (_currentCulture.Equals(selectedCulture))
                return;
            
            foreach (MenuItem menu in languageMenuItem.MenuItems)
            {
                menu.Checked = menu == item;
            }

            Settings.Default.Culture = item.Text;
            _currentCulture = selectedCulture;

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
            ReloadUILanguage(_currentCulture);

            int selectedIndex = parserBox.SelectedIndex;

            parserBox.Items.Clear();

            foreach (KeyValuePair<int, KeyValuePair<ParserType, Type>> kvp in _parsers)
            {
                parserBox.Items.Add(GetNameByParserType(kvp.Value.Key));
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

        private uint[] GetEntriesList()
        {
            string path = string.Format("{0}\\{1}.welf", WelfFolder, welfBox.SelectedItem);
            if (!File.Exists(path))
            {
                ShowMessageBox(MessageType.WelfFileNotFound, path);
                return null;
            }

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    uint[] entries = new uint[count];

                    for (int i = 0; i < count; ++i)
                    {
                        uint entry = reader.ReadUInt32();
                        entries[i] = entry;
                    }

                    return entries;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(Resources.Error_while_loading_welf_file, path, exception.Message);
                return null;
            }
        }
    }
}