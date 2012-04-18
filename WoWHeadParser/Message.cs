using System.Windows.Forms;

namespace WoWHeadParser
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

    public struct Message
    {
        public string Text;
        public MessageBoxButtons Button;
        public MessageBoxIcon Icon;

        public Message(string message, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Exclamation)
        {
            Text = message;
            Button = button;
            Icon = icon;
        }
    }
}