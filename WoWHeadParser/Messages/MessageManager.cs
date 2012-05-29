using System.Windows.Forms;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Messages
{
    public static class MessageManager
    {
        public static MessageText GetMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.MultipleTypeEqual:
                    return new MessageText(Resources.MultipleTypeEqual);
                case MessageType.MultipleTypeBigger:
                    return new MessageText(Resources.MultipleTypeBigger);
                case MessageType.WelfFileNotFound:
                    return new MessageText(Resources.WelfFileNotFound, MessageBoxIcon.Error);
                case MessageType.AbortQuestion:
                    return new MessageText(Resources.AbortQuestion, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                case MessageType.ExitQuestion:
                    return new MessageText(Resources.ExitQuestion, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            return new MessageText("Unsupported message type {0}", type);
        }
    }
}
