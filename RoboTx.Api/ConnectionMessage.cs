using Messaging;

namespace RoboTx.Api
{
    internal enum ConnectionCommand
    {
        None,
        Open,
        Close,
        KeepAlive
    }

    /// <summary>
    /// Sends connection related information to the Arduino.
    /// </summary>
    internal class ConnectionMessage : SerializableMessage
    {
        public const string MessageType = "C";
        private const string CR_CMD_OPEN = "O";
        private const string CR_CMD_CLOSE = "C";
        private const string CR_CMD_KEEP_ALIVE = "A";

        volatile ConnectionCommand _cmd;

        public ConnectionMessage(StreamWriter sw) : base(sw)
        {

        }

        /// <summary>
        /// Signal to the Arduino that the client has opened the connection.
        /// </summary>
        public void Open()
        {
            _cmd = ConnectionCommand.Open;
        }

        /// <summary>
        /// Signal to the Arduino that the client is closing the connection.
        /// </summary>
        public void Close()
        {
            _cmd = ConnectionCommand.Close;
        }

        public void KeepAlive()
        {
            _cmd = ConnectionCommand.KeepAlive;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            switch (_cmd)
            {
                case ConnectionCommand.Open:
                    SerializeProperty(CR_CMD_OPEN);
                    break;
                case ConnectionCommand.Close:
                    SerializeProperty(CR_CMD_CLOSE);
                    break;
                case ConnectionCommand.KeepAlive:
                    SerializeProperty(CR_CMD_KEEP_ALIVE);
                    break;
            }
        }
    }
}
