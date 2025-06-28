using Messaging;

namespace RoboTx.Api
{
    internal enum DisplayLcdCommand
    {
        None,
        Clear,
        Print,
        Sleep,
        WakeUp
    }

    internal class DisplayLcdMessage : SerializableMessage
    {
        private const string MessageType = "D";
        private const string CMD_CLEAR = "C";
        private const string CMD_PRINT = "P";
        private const string CMD_WAKEUP = "W";
        private const string CMD_SLEEP = "S";

        volatile private DisplayLcdCommand _cmd;
        volatile private string _displayText = string.Empty;
        volatile private int _col = 0;
        volatile private int _row = 0;

        public DisplayLcdMessage(StreamWriter sw) : base(sw)
        {

        }

        public void Clear()
        {
            _cmd = DisplayLcdCommand.Clear;
        }

        public void PrintAt(int col, int row, string text)
        {
            _col = Math.Clamp(col, 0, 15);
            _row = Math.Clamp(row, 0, 3);
            _displayText = text.Substring(0, Math.Min(text.Length, 16));
            _cmd = DisplayLcdCommand.Print;
        }

        public void Sleep()
        {
            _cmd = DisplayLcdCommand.Sleep;
        }

        public void WakeUp()
        {
            _cmd = DisplayLcdCommand.WakeUp;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            switch (_cmd)
            {
                case DisplayLcdCommand.None:
                    break;
                case DisplayLcdCommand.Clear:
                    SerializeProperty(CMD_CLEAR);
                    break;
                case DisplayLcdCommand.Print:
                    SerializeProperty(CMD_PRINT);
                    SerializeProperty(_col.ToString());
                    SerializeProperty(_row.ToString());
                    SerializeProperty(_displayText);
                    break;
                case DisplayLcdCommand.Sleep:
                    SerializeProperty(CMD_SLEEP);
                    break;
                case DisplayLcdCommand.WakeUp:
                    SerializeProperty(CMD_WAKEUP);
                    break;
                default:
                    break;
            }
        }
    }
}
