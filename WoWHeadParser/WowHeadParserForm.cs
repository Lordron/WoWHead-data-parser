using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WoWHeadParser.DBFileStorage;
using WoWHeadParser.Parser;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Worker _worker;
        private List<uint> _entries;
        private List<DataParser> _parsers;

        private const string WelfFolder = "EntryList";

        #region Messages

        private Dictionary<MessageType, Message> _message = new Dictionary<MessageType, Message>
        {
            {MessageType.WelfFileNotFound, new Message(@"File {0} not found!")},
            {MessageType.WelfListEmpty, new Message(@"Entries list ({0}) is empty!")},
            {MessageType.MultipleTypeEqual, new Message(@"Starting value can not be equal ending value!")},
            {MessageType.MultipleTypeBigger, new Message(@"Starting value can not be bigger than ending value!")},
            {MessageType.AbortQuestion, new Message(@"Do you really want to stop ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
            {MessageType.ExitQuestion, new Message(@"Do you really want to quit WoWHead Parser ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
        };

        #endregion

        public WoWHeadParserForm()
        {
            InitializeComponent();

            RichTextBoxWriter.Instance.OutputBox = consoleBox;

            _entries = new List<uint>(1024);
            _parsers = new List<DataParser>(16);

            _worker = new Worker();
            _worker.PageDownloadingComplete += WorkerPageDownloaded;

            DBFileLoader.Initial();
        }

        protected override void OnLoad(EventArgs e)
        {
            #region Prepare

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
                Console.WriteLine("Error while loading last parser from settings! Last parser index < parsers count!");

            int lastLanguage = Settings.Default.LastLanguage;
            if (lastLanguage < localeBox.Items.Count)
                localeBox.SelectedIndex = lastLanguage;
            else
                Console.WriteLine("Error while loading last language from settings! Last language index < parsers count!");

            #endregion

            LoadWelfFiles();
        }

        private void LocaleBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.LastLanguage = localeBox.SelectedIndex;
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
            SetLabelText(@"Parsing...");

            backgroundWorker.RunWorkerAsync(type);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ParsingType type = (ParsingType)e.Argument;
            _worker.Start(type);
        }

        private void WorkerPageDownloaded()
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

            SetLabelText(@"Complete!");
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (ShowMessageBox(MessageType.AbortQuestion) != DialogResult.Yes)
                return;

            _worker.Stop();
            SetLabelText(@"Aborting...");
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
                Console.WriteLine("Error while loading welf file {0} - {1}", path, exception.Message);
                return;
            }

            if (_entries.Count == -1)
                ShowMessageBox(MessageType.WelfListEmpty, path);

            entryCountLabel.Text = string.Format("Entries count: {0}", _entries.Count);
        }

        private void WELFCreatorMenuClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => new WelfCreator().Show());
        }

        private void ReloadWelfFilesButtonClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => LoadWelfFiles());
        }

        private void AboutMenuItemClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => new AboutForm().ShowDialog());
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
            if (!_message.ContainsKey(type))
                return DialogResult.None;

            Message message = _message[type];
            string msg = string.Format(CultureInfo.InvariantCulture, message.Text, args);

            Console.WriteLine(msg);

            return MessageBox.Show(msg, @"WoWHead Parser", message.Button, message.Icon);
        }

        private void OptionsMenuItemClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => new SettingsForm().ShowDialog());
        }
    }
}