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

        public WoWHeadParserForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in Types)
            {
                if (type.IsSubclassOf(typeof(Parser)))
                    parserBox.Items.Add(type);
            }

            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            FileInfo[] Files = info.GetFiles("*.welf", SearchOption.AllDirectories);
            foreach (FileInfo file in Files)
            {
                welfBox.Items.Add(file.Name);
            }
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            abortButton.Enabled = true;

            progressBar.Value = 0;

            // Starting work on a different thread to prevent MainForm freezing
            backgroundWorker.RunWorkerAsync();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            if (parserBox.SelectedItem == null)
            {
                startButton.Enabled = false;
                abortButton.Enabled = false;
                return;
            }

            startButton.Enabled = true;
            abortButton.Enabled = false;
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            string locale = (string)localeBox.SelectedItem;
            ParsingType type = (ParsingType)parsingControl.SelectedIndex;

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new ArgumentNullException();

            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);

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

                        _worker = new Worker(_entries, address);
                        break;
                    }
                case ParsingType.TypeMultiple:
                    {
                        uint startValue = (uint)rangeStart.Value;
                        uint endValue = (uint)rangeEnd.Value;

                        if (startValue > endValue)
                            throw new ArgumentOutOfRangeException(@"Starting value can not be bigger than ending value!");

                        if (startValue == endValue)
                            throw new ArgumentOutOfRangeException(@"Starting value can not be equal ending value!");

                        progressBar.Maximum = (int)(endValue - startValue);

                        _worker = new Worker(startValue, endValue, address);
                        break;
                    }
                default:
                    throw new NotImplementedException(string.Format(@"Unsupported type: {0}", type));
            }

            progressLabel.Text = "Downloading...";

            _startTime = DateTime.Now;
            _worker.Start(backgroundWorker);
        }

        private void BackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new Action<int>(i => progressBar.Value += i), e.ProgressPercentage);
            else
                progressBar.Value += e.ProgressPercentage;
        }

        void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startButton.Enabled = true;
            abortButton.Enabled = false;
            DateTime now = DateTime.Now;

            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                progressLabel.Text = "Parsing...";

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

            progressLabel.Text = "Complete!";
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            DialogResult result = ShowQuestionMessageBox("Do you really want to stop ?");
            if (result != DialogResult.OK)
                return;

            _worker.Stop();
            startButton.Enabled = true;
            abortButton.Enabled = false;
            progressLabel.Text = "Abort...";
        }

        private void WelfBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _entries = new List<uint>();

            if (welfBox.SelectedItem == null)
            {
                startButton.Enabled = false;
                abortButton.Enabled = false;
                return;
            }

            using (StreamReader reader = new StreamReader(Path.Combine("EntryList", (string)welfBox.SelectedItem)))
            {
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

            entryCountLabel.Text = string.Format("Entry count: {0}", _entries.Count);

            if (_entries.Count == -1)
                throw new NotImplementedException(@"Entries list is empty!");
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
                _worker.Stop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = ShowQuestionMessageBox("Do you really want to quit WoWHead Parser ?");
            e.Cancel = (result == DialogResult.Cancel);
        }

        private static DialogResult ShowQuestionMessageBox(string format, params object[] args)
        {
            string msg = string.Format(CultureInfo.InvariantCulture, format, args);
            return MessageBox.Show(msg, @"WoWHead Parser", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
    }
}
