using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private List<uint> _entries;
        private List<DataParser> _parsers;

        private string _currentCulture;

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

            _entries = new List<uint>(1024);
            _parsers = new List<DataParser>(32);
            _worker = new Worker(WorkerPageDownloaded);

            DBFileLoader.Initial();
        }

        protected override void OnLoad(EventArgs e)
        {
            #region Parsers loading

            Type typofParser = typeof(DataParser);
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typofParser))
                    continue;

                DataParser parser = Activator.CreateInstance(type) as DataParser;
                if (parser == null)
                    continue;

                parserBox.Items.Add(parser.Name);
                parser.Prepare();

                _parsers.Add(parser);
            }

            #endregion

            #region Language loading

            foreach (string language in _language)
            {
                MenuItem item = new MenuItem(language, LanguageMenuItemClick);
                languageMenuItem.MenuItems.Add(item);
            }


            _currentCulture = Settings.Default.Culture;
            Reload(_currentCulture);

            #endregion

            #region Locale loading

            foreach (Locale locale in Enum.GetValues(typeof(Locale)))
            {
                if (locale != Locale.Portugal)
                    localeBox.Items.Add(locale);
            }

            #endregion

            #region Load from settings

            int lastParser = Settings.Default.LastParser;
            if (lastParser < parserBox.Items.Count)
                parserBox.SelectedIndex = lastParser;
            else
                Console.WriteLine(Resources.Error_while_loading_last_parser);

            int lastLocale = Settings.Default.LastLocale;
            if (lastLocale < localeBox.Items.Count)
                localeBox.SelectedIndex = lastLocale;
            else
                Console.WriteLine(Resources.Error_while_loading_last_locale);

            #endregion

            LoadWelfFiles();
        }

        private void LocaleBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.LastLocale = localeBox.SelectedIndex;
        }

        private void ParserBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.LastParser = parserBox.SelectedIndex;
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            DataParser parser = _parsers[parserBox.SelectedIndex];
            parser.Locale = (Locale)localeBox.SelectedItem;

            _worker.Parser(parser);

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        uint value = (uint)valueBox.Value;
                        _worker.SetValue(value);
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        numericUpDown.Maximum = progressBar.Maximum = _entries.Count;
                        _worker.SetValue(_entries);
                        break;
                    }
                case ParsingType.TypeMultiple:
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
                case ParsingType.TypeWoWHead:
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
            settingsBox.Enabled = startButton.Enabled = false;
            numericUpDown.Value = progressBar.Value = 0;
            SetLabelText(Resources.Label_Working);

            backgroundWorker.RunWorkerAsync(type);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ParsingType type = (ParsingType)e.Argument;
            _worker.Start(type);
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
            settingsBox.Enabled = startButton.Enabled = true;
            numericUpDown.Value = progressBar.Value = 0;

            _worker.Reset();

            SetLabelText(Resources.Label_Complete);
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (ShowMessageBox(MessageType.AbortQuestion) != DialogResult.Yes)
                return;

            _worker.Stop();
            SetLabelText(Resources.Label_Abort);
        }

        private void WelfBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string path = string.Format("{0}\\{1}", WelfFolder, welfBox.SelectedItem);

            if (!File.Exists(path))
            {
                ShowMessageBox(MessageType.WelfFileNotFound, path);
                return;
            }

            _entries.Clear();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string text = reader.ReadToEnd();
                    string[] values = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < values.Length; ++i)
                    {
                        string s = values[i];

                        uint value;
                        if (!uint.TryParse(s, out value))
                            continue;

                        _entries.Add(value);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(Resources.Error_while_loading_welf_file, path, exception.Message);
                return;
            }

            if (_entries.Count == -1)
                ShowMessageBox(MessageType.WelfListEmpty, path);

            entryCountLabel.Text = string.Format(Resources.EntryCountLabel, _entries.Count);
        }

        private void WELFCreatorMenuClick(object sender, EventArgs e)
        {
            new WelfCreator(_currentCulture).ShowDialog();
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

            _currentCulture = Settings.Default.Culture = selectedCulture;

            Reload(_currentCulture);
        }

        private void AboutMenuItemClick(object sender, EventArgs e)
        {
            new AboutForm(_currentCulture).ShowDialog();
        }

        private void ExitMenuClick(object sender, EventArgs e)
        {
            Application.Exit();
            Settings.Default.Save();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = ShowMessageBox(MessageType.ExitQuestion);
            e.Cancel = (result == DialogResult.No);
        }

        protected override void OnClosed(EventArgs e)
        {
            Settings.Default.Save();
        }

        private void SetLabelText(string text)
        {
            progressLabel.ThreadSafeBegin(x => x.Text = text);
        }

        private void LoadWelfFiles()
        {
            welfBox.Items.Clear();

            DirectoryInfo info = new DirectoryInfo(WelfFolder);
            FileInfo[] files = info.GetFiles("*.welf");
            foreach(FileInfo file in files)
            {
                welfBox.Items.Add(file.Name);
            }

            if (files.Length > 0)
                welfBox.SelectedIndex = 0;
        }

        private DialogResult ShowMessageBox(MessageType type, params object[] args)
        {
            MessageText message = MessageManager.GetMessage(type);
            return message.ShowMessage(Text, args);
        }

        private void OptionsMenuItemClick(object sender, EventArgs e)
        {
            new SettingsForm(_currentCulture).ShowDialog();
        }
    }
}