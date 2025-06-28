namespace RoboTx.Api
{
    /// <summary>
    /// Uses sonar to calculate distance by sending a ping and measuring the time lapsed before receiving the echo.
    /// The maximum distance that can be measured is 165 Centimetres.
    /// Sonar works best to detect objects with hard surfaces that reflect sound well. The sonar sensor
    /// may detect a closer object off to the side instead of a farther object straight ahead.
    /// The Arduino pins assigned for the sonar module are configured in the firmware settings.
    /// </summary>
    public sealed class Sonar
    {
        private readonly RobotIO _robotIO;
        volatile private int _distance;
        object _lockobj = new object();

        internal Sonar(RobotIO robotIO)
        {
            _robotIO = robotIO;
            _distance = -1;
        }

        /// <summary>
        /// Sends one ping using a sonar module.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Ping()
        {
            _robotIO.CheckSerialState();

            SonarMessage msg = new SonarMessage(_robotIO.StreamWriter);
            _robotIO.MessageSender.EnQueueMessage(msg, true);
        }

        /// <summary>
        /// Clears the current distance value.
        /// </summary>
        public void Clear()
        {
            _distance = -1;
        }

        /// <summary>
        /// Returns true if a distance value has been calculated from the sonar echo. Returns false if no echo was received in time.
        /// </summary>
        public bool DistanceAcquired
        {
            get { return _distance != -1; }
        }


        internal void SetDistance(int distance)
        {
            lock (_lockobj)
            {
                _distance = distance;
            }
        }

        /// <summary>
        /// Gets the distance calculated from the sonar echo after calling Ping().
        /// A value of -1 indicates no echo was received in time. Note that a call to this method
        /// will reset the distance to -1. Therefore, the caller should store the
        /// value returned in a variable.
        /// </summary>
        /// <returns>Distance calculated as a result of an echo received after a sonar ping.</returns>
        public int GetDistance()
        {
            int val = _distance;
            lock (_lockobj)
            {
                if (_distance > -1)
                {
                    _distance = -1;
                }
            }
            return val;
        }
    }
}
