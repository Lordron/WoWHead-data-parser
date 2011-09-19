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

            progressBar.Value = (int) start;
            progressBar.Minimum = (int)start;
            progressBar.Maximum = (int)end;
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

        public void WorkerProgressChanged(Worker worker, int val)
        {
            progressLabel.Text = val.ToString();
            progressBar.Value = val;
            /*if (val == progressBar.Maximum)
            {
                if (worker.Pages.Count > 0)
                    _parser.Parse(worker.Pages);
            }*/
        }
    }
}
