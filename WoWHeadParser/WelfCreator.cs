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

        private void StartButtonClick(object sender, EventArgs e)
        {
            _ids = new List<uint>();

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };
            string page = client.DownloadString(addressBox.Text);

            if (string.IsNullOrWhiteSpace(page))
                return;

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
                content.AppendFormat(@" -- Dump of {0}, Total ids count: {1}", DateTime.Now, _ids.Count).AppendLine();

                for (int i = 0; i < _ids.Count; ++i)
                {
                    content.AppendFormat("{0},", _ids[i]);
                }

                welDataBox.Text = content.ToString().TrimEnd(',');
            }

            if (!saveButton.Enabled)
                saveButton.Enabled = true;
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (_ids.Count <= 0)
                return;

            saveButton.Enabled = false;
            startButton.Enabled = true;

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
            {
                for (int i = 0; i < _ids.Count; ++i)
                {
                    writer.Write("{0}{1}", _ids[i], (i < _ids.Count - 1 ? "," : string.Empty));
                }
            }
        }

        private void AddressBoxTextChanged(object sender, EventArgs e)
        {
            startButton.Enabled = !string.IsNullOrWhiteSpace(addressBox.Text);
        }
    }
}
