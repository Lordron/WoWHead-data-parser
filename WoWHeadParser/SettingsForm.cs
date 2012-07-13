using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Sql;
using WoWHeadParser.Properties;

namespace WoWHeadParser
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            sqlQueryTypeBox.SetEnumValues<SqlQueryType>(Settings.Default.QueryType);
            sortDirectionBox.SetEnumValues<SortOrder>(Settings.Default.SortOrder);
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            Settings.Default.AllowEmptyValues = allowNullValCheckBox.Checked;
            Settings.Default.AppendDeleteQuery = appendDeleteQueryCheckBox.Checked;
            Settings.Default.QueryType = (SqlQueryType)sqlQueryTypeBox.SelectedItem;
            Settings.Default.SortOrder = (SortOrder)sortDirectionBox.SelectedItem;
            Settings.Default.Append = appendSqlCheckBox.Checked;
            Settings.Default.WithoutHeader = withoutHeaderCheckBox.Checked;
            Settings.Default.DataCompression = dataCompressionCheckBox.Checked;
            Settings.Default.Save();

            Close();
        }

        private void CancleButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}