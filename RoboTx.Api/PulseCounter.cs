namespace RoboTx.Api
{
    /// <summary>
    /// Configures and calculates period of input pulses on Arduino pin A2.
    /// </summary>
    public sealed class PulseCounter
    {
        readonly private RobotIO _robotIO;
        private volatile int _pulsePeriod;

        internal PulseCounter(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Initializes and enables the pulse counter. Used for measuring pulses applied to input pin A2. Max pulse frequency 500hz.
        /// Inputs on A2 will not be registered as events whilst pulse counting is enabled.
        /// </summary>
        /// <param name="timeoutMs">The number of milliseconds to wait for a pulse before resetting pulse period to 0.
        /// Must be between 250 and 10000 inclusive.</param>
        /// <param name="triggerEdge">Trigger counter on either rising (1) or falling (0) edge of pulse signal.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeoutMs is out of range.</exception>
        public void Enable(int timeoutMs, int triggerEdge)
        {
            _robotIO.CheckSerialState();

            PulseCounterMessage msg = new PulseCounterMessage(_robotIO.StreamWriter);
            msg.Enable(timeoutMs, triggerEdge == 0 ? 0 : 1);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Disables the pulse counter.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Disable()
        {
            _robotIO.CheckSerialState();

            PulseCounterMessage msg = new PulseCounterMessage(_robotIO.StreamWriter);
            msg.Disable();
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Gets the pulse period (in milliseconds) reported by the pulse counter.
        /// Pulse counter must be <see cref="Enable(int, int)">enabled</see> before values become available.
        /// </summary>
        public int Period { get => _pulsePeriod; internal set => _pulsePeriod = value; }
    }
}
