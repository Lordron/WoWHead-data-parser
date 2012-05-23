using System.Windows.Forms;

namespace WoWHeadParser.Messages
{
    public enum MessageType : byte
    {
        None,
        MultipleTypeBigger,
        MultipleTypeEqual,
        AbortQuestion,
        ExitQuestion,
        WelfListEmpty,
        WelfFileNotFound,
        Max,
    }

    public class MessageText
    {
        public string Text;
        public MessageBoxButtons Button;
        public MessageBoxIcon Icon;

        public MessageText(string message, MessageBoxButtons button, MessageBoxIcon icon)
        {
            Text = message;
            Button = button;
            Icon = icon;
        }

        public MessageText(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }

        public MessageText(string message)
            : this(message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        {
        }

        public MessageText(string message, MessageBoxButtons button)
            : this(message, button, MessageBoxIcon.Exclamation)
        {
        }

        public MessageText(string message, MessageBoxIcon icon)
            : this(message, MessageBoxButtons.OK, icon)
        {
        }

        public DialogResult ShowMessage(string caption, params object[] args)
        {
            return MessageBox.Show(string.Format(Text, args), caption, Button, Icon);
        }
    }
}