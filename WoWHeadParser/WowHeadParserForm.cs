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
        private List<Type> _type = new List<Type>();
        private WebClient _client = new WebClient { Encoding = Encoding.UTF8 };
        private List<string> _datas = new List<string>();

        public WoWHeadParserForm()
        {
            InitializeComponent();
            Initial();
        }

        public void Initial()
        {
            uint count = 0;
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in Types)
            {
                if (type.IsSubclassOf(typeof(Parser)))
                {
                    parserBox.Items.Add(type);
                    _type.Add(type);
                    ++count;
                }
            }
            if (count == 0)
                startButton.Enabled = false;

            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCompletedAction);
        }

        void DownloadStringCompletedAction(object sender, DownloadStringCompletedEventArgs e)
        {
            _datas.Add(e.Result);
            ++progressBar.Value;
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            if (parserBox.SelectedIndex == -1)
                throw new NotImplementedException(@"You should select something first!");

            int start = (int)rangeStart.Value;
            int end = (int)rangeEnd.Value;

            if (start > end)
                throw new NotImplementedException(@"Starting value can not be bigger than endind value!");

            if (start == end)
                throw new NotImplementedException(@"Starting value can not be equal ending value!");

            if (start == 1 && end == 1)
                throw new NotImplementedException(@"Starting and ending value can not be equal '1'!");

            progressBar.Maximum = end;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
            startButton.Enabled = false;

            Parser parser = (Parser)Activator.CreateInstance(_type[parserBox.SelectedIndex]);

            if (parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            string baseAddress = string.Format("http://{0}{1}", localeBox.SelectedItem, parser.Adress);

            for (int i = start; i < end; ++i)
            {
                Task task = Download(string.Format("{0}{1}", baseAddress, i), string.Format("{0}.txt", i));
                task.Wait();
            }

            parser.Parse(_datas);
            startButton.Enabled = true;
        }

        public async Task Download(string address, string file)
        {
            await _client.DownloadStringTaskAsync(address);
        }
    }
}
