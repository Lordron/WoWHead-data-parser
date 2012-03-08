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
        private DateTime _startTime;
        private List<uint> _entries;

        private List<Type> _types;

        private Dictionary<MessageType, Message> _message = new Dictionary<MessageType, Message>
        {
            {MessageType.MultipleTypeBigger, new Message(@"Starting value can not be bigger than ending value!")},
            {MessageType.MultipleTypeEqual, new Message(@"Starting value can not be equal ending value!")},
            {MessageType.WelfListEmpty, new Message(@"Entries list is empty!")},
            {MessageType.UnsupportedParsingType, new Message(@"Unsupported parsing type: {0}!")},
            {MessageType.AbortQuestion, new Message(@"Do you really want to stop ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
            {MessageType.ExitQuestion, new Message(@"Do you really want to quit WoWHead Parser ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
        };

        public WoWHeadParserForm()
        {
            InitializeComponent();

            _worker = new Worker();
            {
                _worker.PageDownloaded += new Worker.OnPageDownloaded(WorkerPageDownloaded);
                _worker.Finished += new Worker.OnFinished(WorkerFinished);
            }

            _types = new List<Type>();
            _entries = new List<uint>();
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

                _types.Add(type);
            }

            LoadWelfFiles();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            startButton.Enabled = true;
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            Parser parser = (Parser)Activator.CreateInstance(_types[parserBox.SelectedIndex]);

            string locale = (string)localeBox.SelectedItem;
            string address = string.Format("http://{0}{1}", (string.IsNullOrWhiteSpace(locale) ? "www." : locale), parser.Address);

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

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
                default:
                    ShowMessageBox(MessageType.UnsupportedParsingType, type);
                    return;
            }

            abortButton.Enabled = true;
            settingsBox.Enabled = startButton.Enabled = false;
            numericUpDown.Value = progressBar.Value = 0;
            progressLabel.Text = "Downloading...";

            _startTime = DateTime.Now;
            
            backgroundWorker.RunWorkerAsync(type);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ParsingType type = (ParsingType)e.Argument;
            _worker.Start(type);
        }

        private void WorkerPageDownloaded()
        {
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new Action(() => ++progressBar.Value));
            else
                ++progressBar.Value;

            if (numericUpDown.InvokeRequired)
                numericUpDown.BeginInvoke(new Action(() => ++numericUpDown.Value));
            else
                ++numericUpDown.Value;
        }

        void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DateTime now = DateTime.Now;

            if (!_worker.Empty)
            {
                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile(), Encoding.UTF8))
                    {
                        Parser parser = (Parser)Activator.CreateInstance(_types[parserBox.SelectedIndex]);
                        stream.WriteLine(@"-- Dump of {0} ({1}), Total object count: {2}", now, now - _startTime, _worker.Pages.Count);
                        while(!_worker.Empty)
                        {
                            Block block = _worker.Pages.Dequeue();
                            string content = parser.Parse(block);
                            if (!string.IsNullOrEmpty(content))
                                stream.Write(content);
                        }
                    }
                }
            }

            _worker.Finish();
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (ShowMessageBox(MessageType.AbortQuestion) != DialogResult.Yes)
                return;

            _worker.Stop();
            backgroundWorker.CancelAsync();

            progressLabel.Text = "Aborting...";
        }

        private void WorkerFinished()
        {
            numericUpDown.Value = progressBar.Value = 0;
            abortButton.Enabled = false;
            settingsBox.Enabled = startButton.Enabled = true;

            progressLabel.Text = "Finished!";
        }

        private void WelfBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            using (StreamReader reader = new StreamReader(Path.Combine("EntryList", (string)welfBox.SelectedItem)))
            {
                _entries.Clear();

                string str = reader.ReadToEnd();
                string[] values = str.Split(',');
                foreach (string value in values)
                {
                    uint val;
                    if (uint.TryParse(value, out val))
                    {
                        if (!_entries.Contains(val))
                            _entries.Add(val);
                    }
                }
            }

            entryCountLabel.Text = string.Format("Entries count: {0}", _entries.Count);

            if (_entries.Count == -1)
                ShowMessageBox(MessageType.WelfListEmpty);
        }

        private void WELFCreatorMenuClick(object sender, EventArgs e)
        {
            new WelfCreator().Show();
        }

        private void ReloadWelfFilesMenuClick(object sender, EventArgs e)
        {
            LoadWelfFiles();
        }

        private void LoadWelfFiles()
        {
            welfBox.Items.Clear();

            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            FileInfo[] Files = info.GetFiles("*.welf", SearchOption.AllDirectories);
            foreach (FileInfo file in Files)
            {
                welfBox.Items.Add(file.Name);
            }
        }

        private void ExitMenuClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_worker != null)
                _worker.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = ShowMessageBox(MessageType.ExitQuestion);
            e.Cancel = (result == DialogResult.No);
        }

        private DialogResult ShowMessageBox(MessageType type, params object[] args)
        {
            if (!_message.ContainsKey(type))
                return DialogResult.None;

            Message message = _message[type];
            string msg = string.Format(CultureInfo.InvariantCulture, message.Text, args);
            return MessageBox.Show(msg, @"WoWHead Parser", message.Button, message.Icon);
        }
    }
}
