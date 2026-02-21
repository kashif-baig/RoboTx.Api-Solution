
namespace RoboTx.Api
{
    /// <summary>
    /// Base class for any sensor that continuously reports values, after being enabled.
    /// Once the values are available, the state of the sensor is set to ready.
    /// </summary>
    public abstract class ContinuousSensor
    {
        private volatile bool _isReady = false;
        private volatile bool _isEnabled = false;

        /// <summary>
        /// Returns true if the sensor values are available for reading.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return _isReady;
            }
        }

        /// <summary>
        /// Returns true if the sensor has been enabled. 
        /// </summary>
        public bool IsEnabled
        { get { return _isEnabled; } }


        /// <summary>
        /// Use to indicate that the sensor values have been received.
        /// </summary>
        internal void SetReady()
        {
            if (_isEnabled)
            {
                _isReady = true;
            }
        }

        /// <summary>
        /// Use to enable the sensor and start reporting values.
        /// </summary>
        public virtual void Enable()
        {
            _isEnabled = true;
        }

        /// <summary>
        /// Use to disable the sensor and stop reporting values.
        /// </summary>
        public virtual void Disable()
        {
            _isEnabled = false;
            _isReady = false;
        }
    }
}
