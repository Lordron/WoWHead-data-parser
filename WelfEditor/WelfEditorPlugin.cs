using System.Globalization;
using WoWHeadParser.Plugin;

namespace WelfEditor
{
    public class WelfEditorPlugin : IPlugin
    {
        public void Run(CultureInfo cultureInfo)
        {
            new WelfEditorForm().ShowDialog();
        }

        public string Name { get { return "Welf Editor"; } }
    }
}
