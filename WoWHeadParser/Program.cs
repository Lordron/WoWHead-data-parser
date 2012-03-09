using System;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]

namespace WoWHeadParser
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.SetOut(RichTextBoxWriter.Instance);
            Application.Run(new WoWHeadParserForm());
            RichTextBoxWriter.Instance.Close();
        }
    }
}