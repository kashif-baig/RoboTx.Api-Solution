namespace RoboTx.Api
{
    /// <summary>
    /// Writes a string to a 4 digit 7 segment LED display.
    /// The Arduino pins assigned for the display are configured in the firmware settings.
    /// </summary>
    public sealed class DisplayLed
    {
        readonly private RobotIO _robotIO;
        private const string ERR_STRING = "   E";
        private volatile string _lastText = null;
        private DateTime _lastRequestTime = DateTime.MinValue;

        private static readonly object _lock = new object();

        internal DisplayLed(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Writes a short string to the 7 segment LED display. String will be truncated if it does not fit the display.
        /// </summary>
        /// <param name="text">The string to display, which will be truncated if it does not fit the display.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Write(string text)
        {
            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            lock (_lock)
            {
                if (string.Compare(text, _lastText) != 0 || DateTime.Now.Subtract(_lastRequestTime).TotalSeconds > .503)
                {
                    _lastText = text;
                    _lastRequestTime = DateTime.Now;

                    _robotIO.CheckSerialState();

                    DisplayLedMessage msg = new DisplayLedMessage(_robotIO.StreamWriter);
                    msg.Write(text);

                    _robotIO.MessageSender.EnQueueMessage(msg);
                }
            }
        }

        /// <summary>
        /// Writes an integer value to the 7 segment display.
        /// </summary>
        /// <param name="value">The value to display. Will be show as 'E' if its length does not fit the display.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Write(int value)
        {
            string valueStr = value.ToString().PadLeft(4);
            Write(valueStr.Length <= 4 ? valueStr : ERR_STRING);
        }

        /// <summary>
        /// Writes a double value to the 7 segment display.
        /// </summary>
        /// <param name="value">The value to display. Will be show as 'E' if its length does not fit the display.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Write(double value)
        {
            string valueStr = value.ToString();

            if (valueStr.Contains("."))
            {
                valueStr = valueStr.PadLeft(5);
            }
            else
            {
                valueStr = valueStr.PadLeft(4);
            }
            Write(valueStr.Replace(".", "").Length <= 4 ? valueStr : ERR_STRING);
        }

        /// <summary>
        /// Writes a float value to the 7 segment display.
        /// </summary>
        /// <param name="value">The value to display. Will be show as 'E' if its length does not fit the display.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Write(float value)
        {
            string valueStr = value.ToString();

            if (valueStr.Contains("."))
            {
                valueStr = valueStr.PadLeft(5);
            }
            else
            {
                valueStr = valueStr.PadLeft(4);
            }
            Write(valueStr.Replace(".", "").Length <= 4 ? valueStr : ERR_STRING);
        }
    }
}
