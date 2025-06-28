namespace RoboTx.Api
{
    /// <summary>
    /// Controls speed, acceleration, duration and direction of a motor.
    /// The Arduino pins assigned for motors are configured in the firmware settings.
    /// </summary>
    public sealed class Motor
    {
        private readonly RobotIO _robotIO;
        private readonly int _motorNumber;
        private readonly MotorDriveCommand _motorDriveCmd = new MotorDriveCommand();
        private volatile float _speedMultiplier;
        private DateTime _durationStartTime = DateTime.MinValue;
        private volatile float _duration = 0;
        private volatile int _maxFwdSpeed = 100;
        private volatile int _maxRevSpeed = -100;

        private static readonly Object _lock = new Object();

        internal Motor(RobotIO robotIO, int motorNumber)
        {
            ValidateMotor(motorNumber);
            _robotIO = robotIO;
            _motorNumber = motorNumber;
            _speedMultiplier = 1;
        }

        /// <summary>
        /// Returns true if a previously set duration for driving the motor has lapsed.
        /// Returns false otherwise. See <see cref="DriveForDuration(float, float)"/>.
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
        /// Sets the reverse and forward speed limits of a motor.
        /// </summary>
        /// <param name="maxReverseSpeed">A value between -100 and 0 inclusive.</param>
        /// <param name="maxForwardSpeed">A value between 1 and 100 inclusinve.</param>
        /// <exception cref="ArgumentNullException">motor is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxReverseSpeed or maxForwardSpeed is out of range.</exception>
        internal void SetSpeedLimits(int maxReverseSpeed, int maxForwardSpeed)
        {
            if (maxReverseSpeed > 0 || maxReverseSpeed < -100)
            {
                throw new ArgumentOutOfRangeException(nameof(maxReverseSpeed));
            }
            if (maxForwardSpeed <=0 || maxForwardSpeed > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(maxForwardSpeed));
            }
            _maxFwdSpeed = maxForwardSpeed;
            _maxRevSpeed = maxReverseSpeed;
        }

        /// <summary>
        /// Sets speed multiplier for motor. A negative value will reverse the motor direction.
        /// Allows for fine tuning of the motor speed to match the speed of the other motor.
        /// </summary>
        /// <param name="speedMultiplier">A value between -2 and +2 inclusive</param>
        /// <exception cref="ArgumentOutOfRangeException">speedMultiplier is out of range.</exception>
        internal void SetSpeedMultiplier(float speedMultiplier)
        {
            if (speedMultiplier < -2 || speedMultiplier > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(speedMultiplier));
            }
            _speedMultiplier = speedMultiplier;
        }

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse) and overrides any previously set acceleration.
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void DriveNoAccel(int speedPercent)
        {
            Drive(speedPercent, true);
        }

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse) and overrides any previously set acceleration.
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void DriveNoAccel(float speedPercent)
        {
            Drive(ToInt(speedPercent), true);
        }

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse) and overrides any previously set acceleration.
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <param name="overrideAcceleration">Setting to true overrides a previously set acceleration value.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        internal void Drive(int speedPercent, bool overrideAcceleration)
        {
            _duration = 0;
            speedPercent = Utility.Constrain(ToInt(speedPercent * _speedMultiplier), _maxRevSpeed, _maxFwdSpeed);
            lock (_lock)
            {
                if (_motorDriveCmd.ParametersAreFresh(speedPercent, overrideAcceleration))
                {
                    _robotIO.CheckSerialState();
                    MotorMessage msg = new MotorMessage(_robotIO.StreamWriter);
                    msg.Drive(_motorNumber, speedPercent, overrideAcceleration);
                    _robotIO.MessageSender.EnQueueMessage(msg);
                }
            }
        }

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse) for a specified duration.
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <param name="duration">Duration (in seconds) for which motor is driven.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Duration is out of range.</exception>
        public void DriveForDuration(int speedPercent, float duration)
        {
            ValidateDuration(duration);

            speedPercent = Utility.Constrain(ToInt(speedPercent * _speedMultiplier), _maxRevSpeed, _maxFwdSpeed);

            _duration = duration;
            _durationStartTime = DateTime.Now;

            _robotIO.CheckSerialState();
            MotorMessage msg = new MotorMessage(_robotIO.StreamWriter);
            msg.Drive(_motorNumber, speedPercent, (int)(duration * 1000 + .5));
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse) for a specified duration.
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <param name="duration">Duration (in seconds) for which motor is driven.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Duration is out of range.</exception>
        public void DriveForDuration(float speedPercent, float duration)
        {
            DriveForDuration(ToInt(speedPercent), duration);
        }


        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse).
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Drive(int speedPercent)
        {
            Drive(speedPercent, false);
        }

        ///// <summary>
        ///// Drives the motor at a percentage of its maximum speed (either forward or reverse) and overrides any previously set acceleration.
        ///// </summary>
        ///// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        ///// <param name="overrideAcceleration">If set to true, drives motor at specified speed immediately. Otherwise
        ///// motor accelerates to specified speed using previously set acceleration value.</param>
        ///// <exception cref="IOException">Serial port is in error state or not open.</exception>
        //public void Drive(float speedPercent, bool overrideAcceleration)
        //{
        //    Drive(ToInt(speedPercent), overrideAcceleration);
        //}

        /// <summary>
        /// Drives the motor at a percentage of its maximum speed (either forward or reverse).
        /// </summary>
        /// <param name="speedPercent">Percentage of motor's maximum speed, e.g. -50 or +90.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Drive(float speedPercent)
        {
            Drive(ToInt(speedPercent), false);
        }

        /// <summary>
        /// Configures motor acceleration by specifying the time (in seconds) it takes to reach maximum speed from stationary position.
        /// </summary>
        /// <param name="timeToMaxSpeed">Time (in seconds) to reach maximum speed from stationary. Minimum value is 0, maximum is 10.</param>
        /// <exception cref="ArgumentOutOfRangeException">timeToMaxSpeed is out of range.</exception>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void SetAcceleration(float timeToMaxSpeed)
        {
            if (timeToMaxSpeed < 0 || timeToMaxSpeed > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(timeToMaxSpeed));
            }
            _robotIO.CheckSerialState();

            if (timeToMaxSpeed < 0.05f)
            {
                timeToMaxSpeed = 0;
            }

            int acceleration = timeToMaxSpeed == 0 ? 0 : (int)(12800 / (timeToMaxSpeed * 20) + .5);

            MotorMessage msg = new MotorMessage(_robotIO.StreamWriter);
            msg.SetAcceleration(_motorNumber, acceleration);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// If motor is accelerating, holds the motor at the current speed.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void StopAccelerating()
        {
            _robotIO.CheckSerialState();
            MotorMessage msg = new MotorMessage(_robotIO.StreamWriter);
            msg.StopAccelerating(_motorNumber);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        private void ValidateMotor(int motorNumber)
        {
            if (motorNumber < 1 || motorNumber > 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(motorNumber)}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <exception cref="ArgumentOutOfRangeException">duration is out of range.</exception>
        private void ValidateDuration(float duration)
        {
            if (duration < 0 || duration > 30)
            {
                throw new ArgumentOutOfRangeException($"{nameof(duration)}");
            }
        }

        /// <summary>
        /// Rounds float to the nearest whole integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int ToInt(float value)
        {
            if (value < 0)
            {
                return (int)(value - 0.5);
            }
            return (int)(value + 0.5);
        }
    }
}
