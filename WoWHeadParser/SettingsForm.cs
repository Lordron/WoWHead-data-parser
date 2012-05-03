using System;
using System.Windows.Forms;
using Sql;
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
                if (type > SqlQueryType.None && type < SqlQueryType.Max)
                    sqlQueryTypeBox.Items.Add(type);
            }
            sqlQueryTypeBox.SelectedItem = (SqlQueryType) Settings.Default.QueryType;

            foreach (SortOrder sort in Enum.GetValues(typeof(SortOrder)))
            {
                sortDirectionBox.Items.Add(sort);
            }
            sortDirectionBox.SelectedItem = (SortOrder) Settings.Default.SortOrder;
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            Settings.Default.AllowEmptyValues = allowNullValCheckBox.Checked;
            Settings.Default.AppendDeleteQuery = appendDeleteQueryCheckBox.Checked;
            Settings.Default.QueryType = sqlQueryTypeBox.SelectedIndex + 1;
            Settings.Default.SortOrder = sortDirectionBox.SelectedIndex;
            Settings.Default.Save();

            Close();
        }

        private void CancleButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
