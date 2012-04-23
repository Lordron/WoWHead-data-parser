using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Worker _worker;
        private List<uint> _entries;
        private List<Parser> _parsers;

        #region Messages
        private Dictionary<MessageType, Message> _message = new Dictionary<MessageType, Message>
        {
            {MessageType.MultipleTypeBigger, new Message(@"Starting value can not be bigger than ending value!")},
            {MessageType.MultipleTypeEqual, new Message(@"Starting value can not be equal ending value!")},
            {MessageType.WelfListEmpty, new Message(@"Entries list is empty!")},
            {MessageType.WelfFileNotFound, new Message(@"File {0} not found!")},
            {MessageType.AbortQuestion, new Message(@"Do you really want to stop ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
            {MessageType.ExitQuestion, new Message(@"Do you really want to quit WoWHead Parser ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
        };
        #endregion

        #region Locales
        private Dictionary<Locale, string> _locales = new Dictionary<Locale, string>
        {
            {Locale.Old, "old."},
            {Locale.English, "www."},
            {Locale.Russia, "ru."},
            {Locale.Germany, "de."},
            {Locale.France, "fr."},
            {Locale.Spain, "es."},
            {Locale.Portugal, "pt."},
        };
        #endregion

        public WoWHeadParserForm()
        {
            InitializeComponent();

            RichTextBoxWriter.Instance.OutputBox = consoleBox;

            _entries = new List<uint>();
            _parsers = new List<Parser>();

            _worker = new Worker();
            _worker.PageDownloadingComplete += WorkerPageDownloaded;

            new DB2Loader();
        }

        protected override void OnLoad(EventArgs e)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(Parser)))
                    continue;

                Parser parser = (Parser)Activator.CreateInstance(type);
                parserBox.Items.Add(parser.Name);

                _parsers.Add(parser);
            }

            foreach (Locale locale in Enum.GetValues(typeof(Locale)))
            {
                if (locale != Locale.Portugal)
                    localeBox.Items.Add(locale);
            }

            parserBox.SelectedIndex = 0;
            localeBox.SelectedItem = Locale.English;

            LoadWelfFiles();
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            Parser parser = _parsers[parserBox.SelectedIndex];
            parser.Locale = (Locale)localeBox.SelectedItem;

            string locale = _locales[parser.Locale];
            string address = string.Format("http://{0}{1}", locale, parser.Address);

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            _worker.Parser(parser);

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        uint value = (uint)valueBox.Value;
                        _worker.SetValue(value, address);
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        numericUpDown.Maximum = progressBar.Maximum = _entries.Count;
                        _worker.SetValue(_entries, address);
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
                        _worker.SetValue(startValue, endValue, address);
                        break;
                    }
                case ParsingType.TypeWoWHead:
                    {
                        int maxValue = (parser.MaxCount / 200);
                        _worker.SetValue((uint)maxValue, address);
                        numericUpDown.Maximum = progressBar.Maximum = maxValue + 1;
                        break;
                    }
                default:
                    return;
            }

            abortButton.Enabled = true;
            settingsBox.Enabled = startButton.Enabled = false;
            numericUpDown.Value = progressBar.Value = 0;
            SetLabelText(@"Downloading...");

            backgroundWorker.RunWorkerAsync(type);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ParsingType type = (ParsingType)e.Argument;
            _worker.Start(type);
        }

        private void WorkerPageDownloaded()
        {
            progressBar.ThreadSafeBegin(x => ++x.Value);
            numericUpDown.ThreadSafeBegin(x => ++x.Value);
        }

        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile(), Encoding.UTF8))
                {
                    stream.Write(_worker.ToString());
                }
            }

            abortButton.Enabled = false;
            settingsBox.Enabled = startButton.Enabled = true;
            numericUpDown.Value = progressBar.Value = 0;

            _worker.Reset();

            SetLabelText(@"Finished!");
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
            string fileName = Path.Combine("EntryList", (string)welfBox.SelectedItem);

            if (!File.Exists(fileName))
            {
                ShowMessageBox(MessageType.WelfFileNotFound, fileName);
                return;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                _entries.Clear();

                string str = reader.ReadToEnd();
                string[] values = str.Split(',');
                foreach (string value in values)
                {
                    uint val;
                    if (!uint.TryParse(value, out val))
                        continue;

                    if (!_entries.Contains(val))
                        _entries.Add(val);
                }
            }

            entryCountLabel.Text = string.Format("Entries count: {0}", _entries.Count);

            if (_entries.Count == -1)
                ShowMessageBox(MessageType.WelfListEmpty);
        }

        private void WELFCreatorMenuClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => new WelfCreator().Show());
        }

        private void ReloadWelfFilesMenuClick(object sender, EventArgs e)
        {
            LoadWelfFiles();
        }

        private void ExitMenuClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = ShowMessageBox(MessageType.ExitQuestion);
            e.Cancel = (result == DialogResult.No);
        }

        private void SetLabelText(string text)
        {
            progressLabel.ThreadSafeBegin(x => x.Text = text);
        }

        private void LoadWelfFiles()
        {
            welfBox.Items.Clear();

            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            FileInfo[] files = info.GetFiles("*.welf", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                welfBox.Items.Add(file.Name);
            }

            if (welfBox.Items.Count > 0)
                welfBox.SelectedIndex = 0;
        }

        private DialogResult ShowMessageBox(MessageType type, params object[] args)
        {
            if (!_message.ContainsKey(type))
                return DialogResult.None;

            Message message = _message[type];
            string msg = string.Format(CultureInfo.InvariantCulture, message.Text, args);
            return MessageBox.Show(msg, @"WoWHead Parser", message.Button, message.Icon);
        }

        private void OptionsMenuItemClick(object sender, EventArgs e)
        {
            this.ThreadSafeBegin(x => new SettingsForm().ShowDialog());
        }
    }
}