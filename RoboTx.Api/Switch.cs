namespace RoboTx.Api
{
    /// <summary>
    /// Sets a digital output to on or off. The Arduino digital pin and values that represent on and off
    /// are configured in the firmware settings.
    /// </summary>
    public sealed class Switch
    {
        private readonly RobotIO _robotIO;
        private readonly int _switchNumber;
        private static readonly Object _lock = new Object();

        private volatile bool _lastState = false;
        private DateTime _lastRequestTime = DateTime.MinValue;
        private volatile bool _isOn = false;

        internal Switch(RobotIO robotIO, int switchNumber)
        {
            if (switchNumber < 1 || switchNumber > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(switchNumber));
            }
            _robotIO = robotIO;
            _switchNumber = switchNumber;
        }

        /// <summary>
        /// Switches on the digital output.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void On()
        {
            SetState(true);
        }

        /// <summary>
        /// Returns true if the switch is set to On, false otherwise.
        /// </summary>
        public bool IsOn { get => _isOn; private set => _isOn = value;  }

        /// <summary>
        /// Switches off the digital output.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Off()
        {
            SetState(false);
        }

        private void SetState(bool stateOn)
        {
            lock (_lock)
            {
                _isOn = stateOn;

                if (_lastState != stateOn || DateTime.Now.Subtract(_lastRequestTime).TotalSeconds > .521)
                {
                    _lastState = stateOn;
                    _lastRequestTime = DateTime.Now;

                    _robotIO.CheckSerialState();
                    SwitchMessage msg = new SwitchMessage(_robotIO.StreamWriter);
                    msg.SetState(_switchNumber, stateOn);
                    _robotIO.MessageSender.EnQueueMessage(msg);
                }
            }
        }
    }
}
