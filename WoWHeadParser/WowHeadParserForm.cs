using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Parser _parser = null;
        private Worker _worker = null;
        private List<uint> _entries = null;

        public WoWHeadParserForm()
        {
            InitializeComponent();
            Initial();
        }

        public void Initial()
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in Types)
            {
                if (type.IsSubclassOf(typeof(Parser)))
                {
                    parserBox.Items.Add(type);
                }
            }
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            ParsingType type = (ParsingType)tabControl1.SelectedIndex;
            string locale = (string)localeBox.SelectedItem;

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new ArgumentNullException("Parser");

            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);

            switch (type)
            {
                case ParsingType.TypeSingle:
                    {
                        int value = (int)valueBox.Value;
                        if (value < 1)
                            throw new ArgumentOutOfRangeException(@"Value", @"Value can not be smaller than '1'!");

                        _worker = new Worker(value, address, backgroundWorker);
                        break;
                    }
                case ParsingType.TypeList:
                    {
                        if (_entries.Count == -1)
                            throw new NotImplementedException("Entries list is empty!");

                        startButton.Enabled = false;
                        stopButton.Enabled = true;
                        progressBar.Visible = true;
                        progressBar.Value = 1;
                        progressBar.Minimum = 1;
                        progressBar.Maximum = _entries.Count;

                        _worker = new Worker(_entries, address, backgroundWorker);
                        break;
                    }
                case ParsingType.TypeMultiple:
                    {
                        int startValue = (int)rangeStart.Value;
                        int endValue = (int)rangeEnd.Value;

                        if (startValue > endValue)
                            throw new ArgumentOutOfRangeException(@"StartValue", @"Starting value can not be bigger than ending value!");

                        if (startValue == endValue)
                            throw new NotImplementedException(@"Starting value can not be equal ending value!");

                        startButton.Enabled = false;
                        stopButton.Enabled = true;
                        progressBar.Visible = true;
                        progressBar.Value = startValue;
                        progressBar.Minimum = startValue;
                        progressBar.Maximum = endValue;

                        _worker = new Worker(startValue, endValue, address, backgroundWorker);
                        break;
                    }
                default:
                    throw new NotImplementedException(string.Format("Unsupported type: {0}", type));
            }

            progressLabel.Text = "Downloading...";
            if (_worker == null)
                throw new ArgumentNullException("Worker");

            _worker.Start();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            if (parserBox.SelectedItem == null)
            {
                startButton.Enabled = false;
                stopButton.Enabled = false;
                return;
            }

            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new Action<int>((i) => progressBar.Value += i), e.ProgressPercentage);
            else
                progressBar.Value += e.ProgressPercentage;
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_worker == null)
                throw new ArgumentNullException("Worker");

            startButton.Enabled = true;
            stopButton.Enabled = false;

            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                progressLabel.Text = "Parsing...";
                using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile()))
                {
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

        private void StopButtonClick(object sender, EventArgs e)
        {
            _worker.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            progressLabel.Text = "Abort...";
        }

        private void openEntryListDataButton_Click(object sender, EventArgs e)
        {
            _entries = new List<uint>();

            if (openDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamReader reader = new StreamReader(openDialog.FileName))
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

                    entryCountLabel.Text = string.Format("Entry count: {0}", _entries.Count);
                }
            }
        }
    }
}
