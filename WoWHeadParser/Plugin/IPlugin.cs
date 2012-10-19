using System.Globalization;

namespace WoWHeadParser.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        void Run(CultureInfo cultureInfo);
    }
}
