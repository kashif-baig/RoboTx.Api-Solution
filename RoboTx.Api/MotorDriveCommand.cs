namespace RoboTx.Api
{
    /// <summary>
    /// Used for keeping track of motor commands for ensuring repetetive commands are not seriaiized at high frequency.
    /// </summary>
    internal class MotorDriveCommand
    {
        private DateTime _requestTime;

        private volatile int _speed;
        private volatile bool _overrideAcceleration;

        public MotorDriveCommand()
        {
            _speed = int.MaxValue;
            _requestTime = DateTime.MinValue;
            _overrideAcceleration = false;
        }

        public bool ParametersAreFresh(int speed, bool overrideAcceleration)
        {
            if (_speed != speed || _overrideAcceleration != overrideAcceleration || DateTime.Now.Subtract(_requestTime).TotalSeconds > 0.25)
            {
                _speed = speed;
                _overrideAcceleration = overrideAcceleration;
                _requestTime = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
