using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        private Parser _parser = null;
        private Worker _worker = null;

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
            int startValue = (int)rangeStart.Value;
            int endValue = (int)rangeEnd.Value;
            int _threadCount = (int)threadCountBox.Value;
            string locale = (string)localeBox.SelectedItem;

            if (parserBox.SelectedItem == null)
                throw new NotImplementedException(@"You should select something first!");

            if (startValue > endValue)
                throw new NotImplementedException(@"Starting value can not be bigger than ending value!");

            if (startValue == endValue)
                throw new NotImplementedException(@"Starting value can not be equal ending value!");

            if (startValue == 1 && endValue == 1)
                throw new NotImplementedException(@"Starting and ending values can not be equal '1'!");

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            startButton.Enabled = false;
            stopButton.Enabled = true;
            progressBar.Value = startValue;
            progressBar.Minimum = startValue;
            progressBar.Maximum = endValue;
            progressLabel.Text = "Downloading...";

            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);
            _worker = new Worker(startValue, endValue, _threadCount, address, backgroundWorker);
            _worker.Start();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            if (parserBox.SelectedItem == null)
            {
                startButton.Enabled = false;
                stopButton.Enabled = true;
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
                throw new NotImplementedException(@"Worker object is NULL!");

            startButton.Enabled = true;
            stopButton.Enabled = false;

            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                progressLabel.Text = "Parsing...";
                using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile()))
                {
                    foreach (Block block in _worker.Pages)
                    {
                        stream.Write(_parser.Parse(block.Page, block.Entry));
                    }
                }
            }

            progressLabel.Text = "Complete!";
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            foreach (Thread thread in _worker.Threads)
                thread.Abort();

            startButton.Enabled = true;
            stopButton.Enabled = false;
            progressLabel.Text = "Abort...";
        }
    }
}
