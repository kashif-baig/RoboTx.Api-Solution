namespace RoboTx.Api
{
    /// <summary>
    /// Configuration options for servo motors.
    /// </summary>
    public class ServoConfig
    {
        internal ServoConfig()
        {
            
        }

        /// <summary>
        /// Sets the servo angle lower and upper limits to prevent the servo travelling beyond physical boundaries.
        /// </summary>
        /// <param name="angleLowerLimit">An angle &gt;= 0 and &lt; angleUpperLimit </param>
        /// <param name="angleUpperLimit">An angle &gt; angleLowerLimit and &lt;= servo max angle</param>
        /// <param name="servo">The servo that will be affected.</param>
        /// <exception cref="ArgumentNullException">servo is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Either angleLowerLimit or angleUpperLimit is out of range.</exception>
        public void SetAngleLimits(int angleLowerLimit, int angleUpperLimit, Servo servo)
        {
            if (servo == null)
            {
                throw new ArgumentNullException(nameof(servo));
            }
            servo.SetAngleLimits(angleLowerLimit, angleUpperLimit);
        }

        /// <summary>
        /// Sets the maximum speed of the servos.
        /// </summary>
        /// <param name="maxSpeed">Maximum speed of the servo whose value is between 1 and 50 inclusive.</param>
        /// <param name="servo">The servo that will be affected.</param>
        /// <exception cref="ArgumentNullException">servo is null or has null elements.</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxSpeed is out of range.</exception>
        public void SetSpeedLimit(int maxSpeed, params Servo[] servo)
        {
            if (servo == null || servo.Length == 0)
            {
                throw new ArgumentNullException(nameof(servo));
            }
            for (int i = 0; i < servo.Length; i++)
            {
                if (servo[i] == null)
                {
                    throw new ArgumentNullException(nameof(servo));
                }
                servo[i].SetSpeedLimit(maxSpeed);
            }
        }

        /// <summary>
        /// Sets the type of specified servo motor in terms of minimum and maximum pulse width to ensure correct positioning.
        /// </summary>
        /// <param name="maxAngle">Maximum angle of the servo. Must be between 180 and 360 degrees inclusive.</param>
        /// <param name="minPulseWidth">Minimum pulse width expressed as milliseconds. Must be between 500 and 1250 inclusive.</param>
        /// <param name="maxPulseWidth">Maximum pulse width expressed as milliseconds Must be between minPulseWidth and 2500 inclusive.</param>
        /// <param name="servo">The servo that will be affected.</param>
        /// <exception cref="ArgumentNullException">servo is null or has null elements.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Either angleRange, minPulseWidth or maxPulseWidth are out of range.</exception>
        public void SetType(int maxAngle, int minPulseWidth, int maxPulseWidth, params Servo[] servo)
        {
            if (servo == null || servo.Length == 0)
            {
                throw new ArgumentNullException(nameof(servo));
            }
            for (int i = 0; i < servo.Length; i++)
            {
                if (servo[i] == null)
                {
                    throw new ArgumentNullException(nameof(servo));
                }
                servo[i].SetServoType(maxAngle, minPulseWidth, maxPulseWidth);
            }
        }
    }
}
