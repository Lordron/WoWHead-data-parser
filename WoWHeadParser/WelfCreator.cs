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
        private List<uint> _ids;

        public WelfCreator()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            _ids = new List<uint>();
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            _ids.Clear();

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };
            string page = client.DownloadString(addressBox.Text);

            Regex pattern = new Regex("\"id\":([0-9]+),", RegexOptions.Multiline);
            MatchCollection matches = pattern.Matches(page);

            foreach (Match match in matches)
            {
                string id = match.Groups[1].Value;
                uint val;
                if (!uint.TryParse(id, out val))
                    continue;

                if (!_ids.Contains(val))
                    _ids.Add(val);
            }

            numericUpDown.Value = _ids.Count;

            StringBuilder content = new StringBuilder();
            {
                content.AppendFormat(@" -- Dump of {0}, Total ids count: {1}", DateTime.Now, _ids.Count);

                for (int i = 0; i < _ids.Count; ++i)
                {
                    content.AppendFormat("{0}{1}", _ids[i], (i < _ids.Count - 1 ? "," : string.Empty));
                }

                welDataBox.Text = content.ToString();
            }

            if (!saveButton.Enabled)
                saveButton.Enabled = true;
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (_ids.Count <= 0)
                return;

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

            saveButton.Enabled = false;
            startButton.Enabled = true;
        }

        private void AddressBoxTextChanged(object sender, EventArgs e)
        {
            startButton.Enabled = !string.IsNullOrWhiteSpace(addressBox.Text);
        }
    }
}
