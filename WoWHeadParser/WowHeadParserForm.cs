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
        }

        public void StartButtonClick(object sender, EventArgs e)
        {
            if (parserBox.SelectedIndex == -1)
                throw new NotImplementedException("You should select something first!");

            progressBar1.Maximum = (int)rangeEnd.Value;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;

            Parser parser = (Parser)Activator.CreateInstance(_type[parserBox.SelectedIndex]);

            string baseAddress = string.Format("http://{0}{1}", localeBox.SelectedItem, parser.Adress);

            for (decimal i = rangeStart.Value; i < rangeEnd.Value; ++i)
            {
                Task task = Download(string.Format("{0}{1}", baseAddress, i), string.Format("{0}.txt", i));
                task.Wait();
            }
        }

        public async Task Download(string address, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                string page = await _client.DownloadStringTaskAsync(address);
                _datas.Add(page);
            }
        }
    }
}
