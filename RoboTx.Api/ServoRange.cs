namespace RoboTx.Api
{
    /// <summary>
    /// Maintains the range settings for a servo.
    /// </summary>
    public class ServoRange
    {
        private volatile int _maxAngle;
        private volatile int _minPulseWidth;
        private volatile int _maxPulseWidth;
        private volatile int _lowerLimit;
        private volatile int _upperLimit;

        /// <summary>
        /// Gets the maximum physical angle of the servo.
        /// </summary>
        public int MaxAngle { get => _maxAngle; internal set { _maxAngle = value; _upperLimit = value; } }
        internal int MinPulseWidth { get => _minPulseWidth; set => _minPulseWidth = value; }
        internal int MaxPulseWidth { get => _maxPulseWidth; set => _maxPulseWidth = value; }
        /// <summary>
        /// Gets the angle lower limit set for the servo.
        /// </summary>
        public int LowerLimit { get => _lowerLimit; internal set => _lowerLimit = value; }
        /// <summary>
        /// Gets the angle upper limit set for the servo.
        /// </summary>
        public int UpperLimit { get => _upperLimit; internal set => _upperLimit = value; }

        internal ServoRange()
        {
            MaxAngle = 180;
            LowerLimit = 0;
            MinPulseWidth = 544;
            MaxPulseWidth = 2400;
        }
    }
}
