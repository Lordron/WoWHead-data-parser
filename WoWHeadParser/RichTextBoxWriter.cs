using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public class RichTextBoxWriter : StreamWriter
    {
        public static RichTextBoxWriter Instance = new RichTextBoxWriter();

        public RichTextBox OutputBox;

        public RichTextBoxWriter()
                : base(new FileStream(Assembly.GetExecutingAssembly().GetName().Name + ".log", FileMode.Create))
        {
            AutoFlush = true;

            if (base.BaseStream.Position != 0)
            {
                base.WriteLine();
                base.WriteLine();
            }
        }

        // Must Implement Methods

        public override Encoding Encoding { get { return Encoding.UTF8; } }

        public override void WriteLine(string value)
        {
            _Write(value + Environment.NewLine);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            _Write(String.Format(format, arg) + Environment.NewLine);
        }

        public override void WriteLine()
        {
            _Write(Environment.NewLine);
        }

        public override void Write(string value)
        {
            _Write(value);
        }

        public override void Write(string format, params object[] arg)
        {
            _Write(String.Format(format, arg));
        }

        // Generic Write Method

        private void _Write(string text)
        {
            if (!OutputBox.IsDisposed)
            {
                OutputBox.ThreadSafeBegin(x =>
                    {
                        x.AppendText(text);
                        x.ScrollToCaret();
                    });
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    base.Write(string.Format("[{0:yyyy.MM.dd HH:mm:ss.ffff}] {1}", DateTime.Now, text));
                }
                catch
                {
                }
            }
        }
    }
}