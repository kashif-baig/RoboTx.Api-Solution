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

        private DateTime _durationStartTime = DateTime.MinValue;
        private volatile float _duration = 0;
        private volatile bool _durationIsEnabled = false;

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
            _durationIsEnabled = false;
            _duration = 0;
        }

        /// <summary>
        /// Switches on the digital output for the specified duration in seconds.
        /// </summary>
        /// <param name="duration">Duration in seconds. Value must be between 0 and 600 inclusive.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void OnForDuration(float duration)
        {
            ValidateDuration(duration);

            _duration = duration;
            _durationStartTime = DateTime.Now;

            if (duration > 0)
            {
                SetState(true);
                _durationIsEnabled = true;
            }
            else
            {
                SetState(false);
                _durationIsEnabled = false;
            }
        }

        /// <summary>
        /// Returns true if a previously set duration in seconds for the output has lapsed.
        /// Returns false otherwise. See <see cref="OnForDuration(float)"/>.
        /// </summary>
        public bool DurationLapsed
        {
            get
            {
                if (_duration == 0 || DateTime.Now.Subtract(_durationStartTime).TotalSeconds >= _duration)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns true if the switch duration is enabled. The enabled state is
        /// set to true when OnForDuration() is called with a non-zero duration value.
        /// It is set to false when On() is called.
        /// </summary>
        internal bool DurationIsEnabled
        {
            get { return _durationIsEnabled; }
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
            _durationIsEnabled = false;
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

        private void ValidateDuration(float duration)
        {
            if (duration < 0 || duration > 600)
            {
                throw new ArgumentOutOfRangeException($"{nameof(duration)}");
            }
        }
    }
}
