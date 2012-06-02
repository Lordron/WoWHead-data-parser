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
        WelfFileNotFound,
        Max,
    }

    public class MessageText
    {
        public string Text;
        public MessageBoxButtons Button;
        public MessageBoxIcon Icon;

        public MessageText(string message, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Exclamation)
        {
            Text = message;
            Button = button;
            Icon = icon;
        }

        public MessageText(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }

        public DialogResult ShowMessage(string caption, params object[] args)
        {
            return MessageBox.Show(string.Format(Text, args), caption, Button, Icon);
        }
    }
}