namespace RoboTx.Api
{
    /// <summary>
    /// Writes text to a 16x2 or 16x4 I2C LCD display.
    /// </summary>
    public sealed class DisplayLcd
    {
        readonly private RobotIO _robotIO;
        private volatile string _lastText = null;
        private volatile int _lastCol = -1;
        private volatile int _lastRow = -1;
        private DateTime _lastRequestTime = DateTime.MinValue;

        internal DisplayLcd(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Clears the display.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Clear()
        {
            _robotIO.CheckSerialState();

            DisplayLcdMessage msg = new DisplayLcdMessage(_robotIO.StreamWriter);
            msg.Clear();
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Displays text starting at a specific location of the LCD display. Causes the LCD
        /// backlight to switch on. The backlight may switch off after a predefined duration in the firmware.
        /// </summary>
        /// <param name="col">The column (zero based) from which to display text.</param>
        /// <param name="row">The row (zero based) from which to display text.</param>
        /// <param name="text">The text to display.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void PrintAt(int col, int row, string text)
        {
            col = Math.Clamp(col, 0, 15);
            row = Math.Clamp(row, 0, 3);
            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            if (_lastCol != col || _lastRow != row || string.Compare(_lastText, text) != 0
                || DateTime.Now.Subtract(_lastRequestTime).TotalSeconds > .523)
            {
                _lastCol = col;
                _lastRow = row;
                _lastText = text;
                _lastRequestTime = DateTime.Now;

                _robotIO.CheckSerialState();

                DisplayLcdMessage msg = new DisplayLcdMessage(_robotIO.StreamWriter);
                msg.PrintAt(col, row, text);
                _robotIO.MessageSender.EnQueueMessage(msg);
            }
        }

        /// <summary>
        /// Switches off the LCD backlight.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Sleep()
        {
            _robotIO.CheckSerialState();

            DisplayLcdMessage msg = new DisplayLcdMessage(_robotIO.StreamWriter);
            msg.Sleep();
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Switches on the LCD backlight. The backlight may switch off after a predefined duration in the firmware.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void WakeUp()
        {
            _robotIO.CheckSerialState();

            DisplayLcdMessage msg = new DisplayLcdMessage(_robotIO.StreamWriter);
            msg.WakeUp();
            _robotIO.MessageSender.EnQueueMessage(msg);
        }
    }
}
