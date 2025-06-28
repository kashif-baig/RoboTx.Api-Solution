namespace RoboTx.Api
{
    /// <summary>
    /// Informs the connected state of the computer with the Arduino.
    /// </summary>
    public class ConnectionState
    {
        private readonly RobotIO _robotIO;
        internal ConnectionState(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Returns true if the method <see cref="RobotIO.NotifyClosing"/> was previously called.
        /// Returns false otherwise. This property is for monitoring by background threads
        /// which should exit gracefully when the property returns true.
        /// </summary>
        public bool IsClosing { get => _robotIO.IsClosing; }

        /// <summary>
        /// Returns true if the serial port is connected to the Arduino, false otherwise.
        /// </summary>
        public bool IsConnected { get => _robotIO.IsConnected; }
    }
}
