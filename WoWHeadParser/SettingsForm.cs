using System;
using System.Windows.Forms;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            foreach (SqlQueryType type in Enum.GetValues(typeof(SqlQueryType)))
            {
                string stype = type.ToString().Replace("Type", "");
                sqlQueryTypeBox.Items.Add(stype);
            }
            sqlQueryTypeBox.SelectedItem = ((SqlQueryType)Settings.Default.QueryType).ToString().Replace("Type", "");
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            Settings.Default.AllowEmptyValues = allowNullValCheckBox.Checked;
            Settings.Default.AppendDeleteQuery = appendDeleteQueryCheckBox.Checked;
            Settings.Default.QueryType = sqlQueryTypeBox.SelectedIndex;
            Settings.Default.Save();

            Close();
        }

        private void CancleButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
