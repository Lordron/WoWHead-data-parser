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
        private ObjectType _type;

        private Dictionary<Locale, string> _locales = new Dictionary<Locale, string>
        {
            {Locale.English, "www"},
            {Locale.Russia, "ru"},
            {Locale.Germany, "de"},
            {Locale.France, "fr"},
            {Locale.Spain, "es"},
            {Locale.Portugal, "pt"},
        };

        private Dictionary<ObjectType, string> _types = new Dictionary<ObjectType, string>
        {
            {ObjectType.TypeNpc, "npcs"},
            {ObjectType.TypeGameObject, "objects"},
            {ObjectType.TypeQuest, "quests"},
        };

        public WelfCreator()
        {
            InitializeComponent();

            foreach (Locale locale in Enum.GetValues(typeof(Locale)))
            {
                languageBox.Items.Add(locale);
            }

            languageBox.SelectedItem = Locale.English;

            foreach (ObjectType locale in Enum.GetValues(typeof(ObjectType)))
            {
                typeBox.Items.Add(locale);
            }

            typeBox.SelectedItem = ObjectType.TypeNpc;

            _ids = new List<uint>();

        }

        private void TypeBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _type = (ObjectType)typeBox.SelectedItem;

            startButton.Enabled = true;
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            Locale locale = (Locale)languageBox.SelectedItem;

            int maxCount = 0;
            int filterId = 0;
            switch (_type)
            {
                case ObjectType.TypeNpc:
                    filterId = 37;
                    maxCount = 59000;
                    break;
                case ObjectType.TypeGameObject:
                    filterId = 15;
                    maxCount = 220000;
                    break;
                case ObjectType.TypeQuest:
                    filterId = 30;
                    maxCount = 31000;
                    break;
            }

            stateLabel.Text = "Working...";

            startButton.Enabled = false;
            progressBar.Minimum = progressBar.Value = 0;
            progressBar.Maximum = maxCount / 200;

            string address = string.Format("http://{0}.wowhead.com/{1}?filter=cr={2}:{2};crs=2:4;", _locales[locale], _types[_type], filterId); // + crv=0:100

            backgroundWorker.RunWorkerAsync(address);
        }

        private void BackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string baseAddress = (string)e.Argument;

            int maxCount = progressBar.Maximum;

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };

            for (int i = 0; i < maxCount; ++i) // maximum 200 entries per list
            {
                string address = string.Format("{0}crv={1}:{2}", baseAddress, (i * 200), ((i + 1)*200));

                string page = client.DownloadString(address);

                Regex pattern = new Regex("\"id\":([0-9]+),", RegexOptions.Multiline);
                MatchCollection matches = pattern.Matches(page);

                int count = 0;
                foreach (Match match in matches)
                {
                    string id = match.Groups[1].Value;

                    uint val;
                    if (!uint.TryParse(id, out val))
                        continue;

                    if (_ids.Contains(val))
                        continue;

                    _ids.Add(val);
                    ++count;
                }

                numericUpDown.Value += count;

                if (progressBar.InvokeRequired)
                    progressBar.BeginInvoke(new Action(() => ++progressBar.Value));
                else
                    ++progressBar.Value;
            }
        }

        void BackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!saveButton.Enabled)
                saveButton.Enabled = true;

            progressBar.Value = 0;
            stateLabel.Text = "Complete!";
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            startButton.Enabled = true;

            if (_ids.Count <= 0)
                return;

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
    }

    public enum ObjectType
    {
        TypeNpc,
        TypeGameObject,
        TypeQuest,
    }
}