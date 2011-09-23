using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class WoWHeadParserForm : Form
    {
        protected Parser _parser = null;
        protected Worker _worker = null;
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
            int threadCount = (int)threadCountBox.Value;
            string locale = (string)localeBox.SelectedItem;

            if (parserBox.SelectedItem == null)
                throw new NotImplementedException(@"You should select something first!");

            if (startValue > endValue)
                throw new NotImplementedException(@"Starting value can not be bigger than ending value!");

            if (startValue == endValue)
                throw new NotImplementedException(@"Starting value can not be equal ending value!");

            if (startValue == 1 && endValue == 1)
                throw new NotImplementedException(@"Starting and ending value can not be equal '1'!");

            if (threadCount > endValue)
                throw new NotImplementedException(@"Thread count value can not be bigger than ending value!");

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            startButton.Enabled = false;
            progressBar.Value = startValue;
            progressBar.Minimum = startValue;
            progressBar.Maximum = endValue + threadCount;

            string address = string.Format("http://{0}{1}", (string.IsNullOrEmpty(locale) ? "www." : locale), _parser.Address);
            _worker = new Worker(startValue, endValue, threadCount, address, backgroundWorker);
            _worker.Start();
        }

        public void ParserIndexChanged(object sender, EventArgs e)
        {
            if (parserBox.SelectedItem == null)
            {
                startButton.Enabled = false;
                return;
            }

            startButton.Enabled = true;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new Action<int>((i) => progressBar.Value += i), e.ProgressPercentage);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_worker == null)
                throw new NotImplementedException(@"Worker object is NULL!");

            progressBar.Visible = true;
            progressLabel.Visible = true;
            startButton.Enabled = true;

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
                return;

            using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile()))
            {
                foreach (Block block in _worker.Pages)
                {
                    stream.Write(_parser.Parse(block.Page, block.Entry));
                }
            }
        }
    }
}
