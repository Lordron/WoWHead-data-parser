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

            Parser parser = (Parser)Activator.CreateInstance((Type)parserBox.SelectedItem);
            if (parser == null)
                throw new NotImplementedException(@"Parser object is NULL!");

            progressBar.Value = (int) start;
            progressBar.Minimum = (int)start;
            progressBar.Maximum = (int)end;
            Worker worker = new Worker(parser, start, end, localeBox.SelectedItem, count, progressBar);
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
    }
}
