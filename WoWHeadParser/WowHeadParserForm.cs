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
        private DateTime _startTime;
        private Parser _parser = null;
        private Worker _worker = null;
        private List<uint> _entries = null;
        private WelfCreator _creator = null;

        private Dictionary<MessageType, Message> _message = new Dictionary<MessageType, Message>
        {
            {MessageType.MultipleTypeBigger, new Message("Starting value can not be bigger than ending value!")},
            {MessageType.MultipleTypeEqual, new Message("Starting value can not be equal ending value!")},
            {MessageType.WelfListEmpty, new Message("Entries list is empty!")},
            {MessageType.AbortQuestion, new Message("Do you really want to stop ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
            {MessageType.ExitQuestion, new Message("Do you really want to quit WoWHead Parser ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)},
        };

        public WoWHeadParserForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            _entries = new List<uint>();

            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in Types)
            {
                if (type.IsSubclassOf(typeof(Parser)))
                    parserBox.Items.Add(type);
            }

            LoadWelfFiles();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            startButton.Enabled = true;
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            abortButton.Enabled = true;
            settingsBox.Enabled = startButton.Enabled = false;

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);

            string locale = (string)localeBox.SelectedItem;
            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);

            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        uint value = (uint)valueBox.Value;
                        _worker = new Worker(value, address);
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        progressBar.Maximum = _entries.Count;
                        numericUpDown.Maximum = _entries.Count;
                        _worker = new Worker(_entries, address);
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

                        progressBar.Maximum = (int)(endValue - startValue);
                        numericUpDown.Maximum = (int)(endValue - startValue);
                        _worker = new Worker(startValue, endValue, address);
                        break;
                    }
                default:
                    return;
            }

            _worker.PageDownloaded += new Worker.OnPageDownloaded(WorkerPageDownloaded);
            _worker.Disposed += new Worker.OnDisposed(WorkerDisposed);

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
                progressBar.BeginInvoke(new Action<int>(i => progressBar.Value += i), 1);
            else
                progressBar.Value++;

            if (numericUpDown.InvokeRequired)
                numericUpDown.BeginInvoke(new Action<int>(i => numericUpDown.Value += i), 1);
            else
                numericUpDown.Value++;
        }

        void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DateTime now = DateTime.Now;

            if (_worker.Pages.Count > 0)
            {
                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile(), Encoding.UTF8))
                    {
                        stream.WriteLine(@"-- Dump of {0} ({1}), Total object count: {2}", now, now - _startTime, _worker.Pages.Count);
                        foreach (Block block in _worker.Pages)
                        {
                            string content = _parser.Parse(block);
                            if (!string.IsNullOrEmpty(content))
                                stream.Write(content);
                        }
                    }
                }
            }

            _worker.Dispose();

            progressLabel.Text = "Complete!";
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            DialogResult result = ShowMessageBox(MessageType.AbortQuestion);
            if (result != DialogResult.Yes)
                return;

            backgroundWorker.Dispose();
            _worker.Dispose();

            progressLabel.Text = "Aborted";
        }

        private void WorkerDisposed()
        {
            progressBar.Value = 0;
            numericUpDown.Value = 0;
            abortButton.Enabled = false;
            settingsBox.Enabled = startButton.Enabled = true;
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

        private void WELFCreatorToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_creator == null || _creator.IsDisposed)
                _creator = new WelfCreator();
            if (!_creator.Visible)
                _creator.Show(this);
        }

        private void ReloadWelfFilesToolStripMenuItemClick(object sender, EventArgs e)
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

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
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
            if (type >= MessageType.Max)
                return DialogResult.None;

            Message message = _message[type];
            string msg = string.Format(CultureInfo.InvariantCulture, message.Message, args);
            return MessageBox.Show(msg, @"WoWHead Parser", message.Button, message.Icon);
        }
    }
}
