using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class WelfCreator : Form
    {
        public WelfCreator()
        {
            InitializeComponent();
            Initial();
        }

        private WebClient _client;
        private int _count;
        private List<string> _ids;

        private void Initial()
        {
            _client = new WebClient() { Encoding = Encoding.UTF8 };
            _ids = new List<string>();
            _count = 0;
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            StringBuilder content = new StringBuilder();
            string address = addressBox.Text;

            if (string.IsNullOrEmpty(address))
            {
                welDataBox.AppendText("<ERROR> Address box can not be empty!\n\n");
                return;
            }

            string page = _client.DownloadString(address);
            Regex pattern = new Regex("\"id\":([0-9]+),", RegexOptions.Multiline);
            MatchCollection matches = pattern.Matches(page);

            foreach (Match match in matches)
            {
                if (!_ids.Contains(match.Groups[1].Value))
                    _ids.Add(match.Groups[1].Value);
            }
            _count += matches.Count;

            foreach (string id in _ids)
            {
                content.AppendLine(id);
            }

            countLabel.Text = string.Format("Count: {0}", _count);

            if (!saveButton.Enabled)
                saveButton.Enabled = true;

            welDataBox.AppendText(content.ToString());
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    for(int i = 0; i < _ids.Count; ++i)
                    {
                        writer.Write("{0}{1}", _ids[i], (i < _ids.Count - 1 ? "," : string.Empty));
                    }
                }
            }
        }
    }
}
