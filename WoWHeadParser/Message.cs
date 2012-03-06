using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public enum MessageType
    {
        None,
        MultipleTypeBigger,
        MultipleTypeEqual,
        AbortQuestion,
        ExitQuestion,
        WelfListEmpty,
        UnsupportedParsingType,
        Max,
    }

    public class Message
    {
        public MessageBoxButtons Button;
        public MessageBoxIcon Icon;
        public string Text;

        public Message(string message, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Exclamation)
        {
            Text = message;
            Button = button;
            Icon = icon;
        }
    }
}
