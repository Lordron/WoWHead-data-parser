using System;
using System.Collections.Generic;
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
        public delegate void DownloaderProgressHandler(Worker worker);

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
            uint start = (uint)rangeStart.Value;
            uint end = (uint)rangeEnd.Value;
            uint count = (uint)threadCount.Value;

            if (parserBox.SelectedItem == null)
                throw new NotImplementedException(@"You should select something first!");

            if (start > end)
                throw new NotImplementedException(@"Starting value can not be bigger than endind value!");

            if (start == end)
                throw new NotImplementedException(@"Starting value can not be equal ending value!");

            if (start == 1 && end == 1)
                throw new NotImplementedException(@"Starting and ending value can not be equal '1'!");

            _parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (_parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            startButton.Enabled = false;

            progressBar.Value = (int) start;
            progressBar.Minimum = (int)start;
            progressBar.Maximum = (int)end + 1;
            Worker worker = new Worker(start, end, localeBox.SelectedItem, _parser.Address, count);
            worker.OnProgressChanged += new DownloaderProgressHandler(WorkerProgressChanged);
            worker.Start();
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

        public void WorkerProgressChanged(Worker worker)
        {
            if (progressBar.InvokeRequired)
                progressBar.BeginInvoke(new Action<int>((i) => ++progressBar.Value), 0);

            if (progressLabel.InvokeRequired)
                progressLabel.BeginInvoke(new Action<string>((s) => progressLabel.Text = s), progressBar.Value.ToString());
            if (progressBar.Value == progressBar.Maximum)
            {
                if (progressBar.InvokeRequired)
                    progressBar.Invoke(new Action<bool>((b) => progressBar.Visible = b), true);
                if (progressLabel.InvokeRequired)
                    progressLabel.Invoke(new Action<bool>((b) => progressLabel.Visible = b), true);

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                using (StreamWriter stream = new StreamWriter(saveDialog.OpenFile()))
                {
                    foreach(Block block in worker.Pages)
                    {
                        stream.Write(_parser.Parse(block.Page, block.Entry));
                    }
                }
            }
        }
    }
}
