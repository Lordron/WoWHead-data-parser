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
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Worker _worker;
        private List<Type> _parsers;
        private List<ParserType> _parserTypes;

        private CultureInfo _currentCulture;

        private const string WelfFolder = "EntryList";

        #region Language

        private List<string> _language = new List<string>
        {
            "en-US",
            "ru-RU",
        };

        #endregion

        public WoWHeadParserForm()
        {
            InitializeComponent();

            RichTextBoxWriter.Instance.OutputBox = consoleBox;

            _parsers = new List<Type>((int)ParserType.Max);
            _parserTypes = new List<ParserType>((int)ParserType.Max);
            _worker = new Worker(WorkerPageDownloaded);
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
            Reload();

            #endregion

            #region DB File loading

            DBFileLoader.Initial();

            #endregion

            #region Parsers loading

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(PageParser)))
                    continue;

                ParserAttribute[] attributes = type.GetCustomAttributes(typeof(ParserAttribute), true) as ParserAttribute[];
                if (attributes == null)
                    throw new InvalidOperationException(); // Each parsers should be marked with this attribute

                foreach (ParserAttribute attribute in attributes)
                {
                    ParserType parserType = attribute.Type;
                    parserBox.Items.Add(GetNameByParserType(parserType));
                    _parserTypes.Add(parserType);
                }

                _parsers.Add(type);
            }

            int index = Settings.Default.LastParser;
            parserBox.SelectedIndex = index < parserBox.Items.Count  ? index : 0;

            #endregion

            #region Locale loading

            localeBox.SetEnumValues<Locale>(Settings.Default.LastLocale);

            #endregion

            #region Welf files loading

            LoadWelfFiles();

            #endregion
        }

        private void ParserBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = parserBox.SelectedIndex;
            welfBox.SelectedItem = _parserTypes[index].ToString().ToLower();

            subparsersListBox.Items.Clear();
            Type subParsers = _parsers[index].GetNestedType("SubParsers");
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
            ConstructorInfo cInfo = _parsers[parserBox.SelectedIndex].GetConstructor(new[] { typeof(Locale), typeof(int) });
            if (cInfo == null)
                return;

            int flags = GetSubparsers();
            PageParser parser = (PageParser)cInfo.Invoke(new [] { localeBox.SelectedItem, flags });
            if (parser == null)
                return;

            _worker.Parser(parser);

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

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
                        List<uint> entries = GetEntriesList();
                        numericUpDown.Maximum = progressBar.Maximum = entries.Count;
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

            backgroundWorker.RunWorkerAsync(type);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ParsingType type = (ParsingType)e.Argument;
            _worker.Start(type, Settings.Default.DataCompression);
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

        private void WELFCreatorMenuClick(object sender, EventArgs e)
        {
            new WelfCreator(_currentCulture).ShowDialog();
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
            this.ThreadSafeBegin(x => LoadWelfFiles());
        }

        private void LanguageMenuItemClick(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null)
                return;

            string selectedCulture = item.Text;
            if (_currentCulture.Equals(selectedCulture))
                return;

            foreach (MenuItem menu in languageMenuItem.MenuItems)
            {
                menu.Checked = false;
            }

            item.Checked = true;

            Settings.Default.Culture = selectedCulture;
            _currentCulture = new CultureInfo(selectedCulture, true);

            Reload();
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

        private void Reload()
        {
            Reload(_currentCulture);

            int index = parserBox.SelectedIndex;

            parserBox.Items.Clear();

            foreach (ParserType type in _parserTypes)
            {
                parserBox.Items.Add(GetNameByParserType(type));
            }

            parserBox.SelectedIndex = index;
        }

        #region Parsers names

        private string GetNameByParserType(ParserType type)
        {
            switch (type)
            {
                case ParserType.Page:
                    return Resources.PageParser;
                case ParserType.Item:
                    return Resources.ItemParser;
                case ParserType.Npc:
                    return Resources.NpcDataParser;
                case ParserType.NpcLocale:
                    return Resources.NpcLocaleParser;
                case ParserType.QuestData:
                    return Resources.QuestDataParser;
                case ParserType.QuestLocale:
                    return Resources.QuestLocaleParser;
                case ParserType.Trainer:
                    return Resources.TrainerParser;
                case ParserType.Vendor:
                    return Resources.VendorParser;
            }
            return string.Empty;
        }

        #endregion

        private void LoadWelfFiles()
        {
            welfBox.Items.Clear();

            DirectoryInfo info = new DirectoryInfo(WelfFolder);
            FileInfo[] files = info.GetFiles("*.welf");
            foreach(FileInfo file in files)
            {
                welfBox.Items.Add(file.Name.Replace(file.Extension, string.Empty));
            }

            if (files.Length > 0)
                welfBox.SelectedIndex = 0;
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

        private List<uint> GetEntriesList()
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
                    List<uint> entries = new List<uint>(count);
                    for (int i = 0; i < count; ++i)
                    {
                        uint entry = reader.ReadUInt32();
                        entries.Add(entry);
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