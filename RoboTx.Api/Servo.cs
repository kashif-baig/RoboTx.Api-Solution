namespace RoboTx.Api
{
    /// <summary>
    /// Configures the range and the position setting of a servo motor.
    /// The Arduino pins assigned for servo motors are configured in the firmware settings.
    /// </summary>
    public sealed class Servo
    {
        private readonly RobotIO _robotIO;
        private readonly ServoRange _servoRange = new ServoRange();
        private readonly int _servoNumber;
        private volatile float _position;
        private volatile int _maxSpeed;

        internal Servo(RobotIO robotIO, int servoNumber)
        {
            ValidateServoNumber(servoNumber);
            _robotIO = robotIO;
            _servoNumber = servoNumber;
            _position = 93;
            _maxSpeed = 50;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSpeed"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal void SetSpeedLimit(int maxSpeed)
        {
            if (maxSpeed < 1 || maxSpeed > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSpeed));
            }
            _maxSpeed = maxSpeed;
            SetSpeed(maxSpeed);
        }

        /// <summary>
        /// Sets the servo angle lower and upper limits to prevent the servo travelling beyond physical boundaries.
        /// </summary>
        /// <param name="angleLowerLimit">An angle &gt;= 0 and &lt; angleUpperLimit </param>
        /// <param name="angleUpperLimit">An angle &gt; angleLowerLimit and &lt;= servo max angle</param>
        /// <exception cref="ArgumentOutOfRangeException">Either angleLowerLimit or angleUpperLimit is out of range.</exception>
        internal void SetAngleLimits(int angleLowerLimit, int angleUpperLimit)
        {
            if (angleLowerLimit < 0 || angleLowerLimit > _servoRange.MaxAngle)
            {
                throw new ArgumentOutOfRangeException(nameof(angleLowerLimit));
            }
            if (angleUpperLimit < angleLowerLimit || angleUpperLimit > _servoRange.MaxAngle)
            {
                throw new ArgumentOutOfRangeException(nameof(angleLowerLimit));
            }
            _servoRange.LowerLimit = angleLowerLimit;
            _servoRange.UpperLimit = angleUpperLimit;
        }


        /// <summary>
        /// Gets the servo range settings.
        /// </summary>
        public ServoRange Range { get => _servoRange; }


        /// <summary>
        /// Sets the range of the servo motor in terms of its maximum angle to ensure correct positioning.
        /// </summary>
        /// <param name="angleRange">Maximum angle of the servo. Must be between 180 and 360 degrees inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException">angleRange is out of range.</exception>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        internal void SetServoType(int angleRange)
        {
            SetServoType(angleRange, 544, 2400);
        }


        /// <summary>
        /// Sets the range of specified servo motor in terms of minimum and maximum pulse width to ensure correct positioning.
        /// </summary>
        /// <param name="maxAngle">Maximum angle of the servo. Must be between 180 and 360 degrees inclusive.</param>
        /// <param name="minPulseWidth">Minimum pulse width expressed as milliseconds. Must be between 500 and 1250 inclusive.</param>
        /// <param name="maxPulseWidth">Maximum pulse width expressed as milliseconds Must be between minPulseWidth and 2500 inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException">Either angleRange, minPulseWidth or maxPulseWidth are out of range.</exception>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        internal void SetServoType(int maxAngle, int minPulseWidth, int maxPulseWidth)
        {
            if (maxAngle < 180 || maxAngle > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(maxAngle));
            }

            if (minPulseWidth < 500 || minPulseWidth > 1250)
            {
                throw new ArgumentOutOfRangeException(nameof(minPulseWidth));
            }
            if (maxPulseWidth < minPulseWidth || maxPulseWidth > 2500)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPulseWidth));
            }
            _servoRange.MaxAngle = maxAngle;
            _servoRange.MinPulseWidth = minPulseWidth;
            _servoRange.MaxPulseWidth = maxPulseWidth;

            _robotIO.CheckSerialState();
            ServoMessage msg = new ServoMessage(_robotIO.StreamWriter);
            msg.SetRange(_servoNumber, minPulseWidth, maxPulseWidth);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Sets the speed with which the servo moves to a specified position. The actual speed and range of speed
        /// will depend on the type of servo used. By default the servo is set to move at maximum speed.
        /// </summary>
        /// <param name="speedStep">A value starting from 1 (slowest) to 50 (fastest) that correlates with servo speed.</param>
        /// <exception cref="ArgumentOutOfRangeException">speedStep is out of range.</exception>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void SetSpeed(int speedStep)
        {
            if (speedStep < 1 || speedStep > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(speedStep));
            }
            _robotIO.CheckSerialState();

            ServoMessage msg = new ServoMessage(_robotIO.StreamWriter);
            msg.SetSpeedStep(_servoNumber, Utility.Constrain(speedStep, 1, _maxSpeed));
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Gets the current position angle of the servo that was set using <see cref="SetPosition(float)"/>.
        /// The value returned is not guaranteed to reflect the actual physical position of the servo, since it can take time for the
        /// servo to move to a given position, or the servo may have been manually re-positioned
        /// whilst in a stopped state.
        /// </summary>
        public float Position { get => _position;}

        /// <summary>
        /// Sets the position angle of a specified servo motor.
        /// </summary>
        /// <param name="angle">The position angle to rotate to. Must be between 0 and maximum angle range set by method SetRange().
        /// The angle value is constrained to the limits specified using <see cref="SetAngleLimits(int, int)"/> </param>
        /// <exception cref="ArgumentOutOfRangeException">angle is out of range.</exception>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void SetPosition(float angle)
        {
            if (angle < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(angle));
            }

            if (angle > _servoRange.MaxAngle)
            {
                throw new ArgumentOutOfRangeException(nameof(angle));
            }

            _robotIO.CheckSerialState();
            ServoMessage msg = new ServoMessage(_robotIO.StreamWriter);

            angle = Utility.Constrain(angle, _servoRange.LowerLimit, _servoRange.UpperLimit);
            _position = angle;

            msg.SetPulseWidth(_servoNumber, Map(angle, 0, _servoRange.MaxAngle,
                                        _servoRange.MinPulseWidth, _servoRange.MaxPulseWidth));

            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Stops the specified servo so it no longer maintains its position.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Stop()
        {
            _robotIO.CheckSerialState();

            ServoMessage msg = new ServoMessage(_robotIO.StreamWriter);
            msg.Stop(_servoNumber);

            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        private static void ValidateServoNumber(int servoNumber)
        {
            if (servoNumber < 1 || servoNumber > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(servoNumber));
            }
        }

        private static int Map(float value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            if (value < fromLow) value = fromLow;
            else if (value > fromHigh) value = fromHigh;
            return (int)((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow + .5f);
        }
    }
}
