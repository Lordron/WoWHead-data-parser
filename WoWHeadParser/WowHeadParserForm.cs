using System;
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
            bool single = (tabControl1.SelectedIndex == 0);
            string locale = (string)localeBox.SelectedItem;

            if (parserBox.SelectedItem == null)
                throw new NotImplementedException(@"You should select something first!");

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);

            if (!single)
            {
                int startValue = (int)rangeStart.Value;
                int endValue = (int)rangeEnd.Value;

                if (startValue > endValue)
                    throw new NotImplementedException(@"Starting value can not be bigger than ending value!");

                if (startValue == endValue)
                    throw new NotImplementedException(@"Starting value can not be equal ending value!");

                if (startValue == 1 && endValue == 1)
                    throw new NotImplementedException(@"Starting and ending values can not be equal '1'!");

                startButton.Enabled = false;
                stopButton.Enabled = true;
                progressBar.Visible = true;
                progressBar.Value = startValue;
                progressBar.Minimum = startValue;
                progressBar.Maximum = endValue;

                _worker = new Worker(startValue, endValue, address, backgroundWorker);
            }
            else
            {
                int value = (int)valueBox.Value;
                if (value < 1)
                    throw new NotImplementedException(@"Value can not be smaller than '1'!");

                _worker = new Worker(value, address, backgroundWorker);
            }

            progressLabel.Text = "Downloading...";
            if (_worker != null)
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
    }
}
