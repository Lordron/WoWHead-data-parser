using System.Globalization;
using WoWHeadParser.Plugin;

namespace WelfCreator
{
    public class WelfCreatorPlugin : IPlugin
    {
        public void Run(CultureInfo cultureInfo)
        {
            new WelfCreator(cultureInfo).ShowDialog();
        }

        public string Name { get { return "Welf Creator"; } }
    }
}
