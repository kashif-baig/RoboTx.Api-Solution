namespace RoboTx.Api
{
    /// <summary>
    /// Configuration options for DC motors.
    /// </summary>
    public class MotorConfig
    {
        internal MotorConfig()
        {
            
        }

        /// <summary>
        /// Sets speed multiplier for motor. A negative value will reverse the motor direction.
        /// Allows for fine tuning of the motor speed to match the speed of the other motor.
        /// </summary>
        /// <param name="speedMultiplier">A value between -2 and +2 inclusive</param>
        /// <param name="motor">The motor that will be affected.</param>
        /// <exception cref="ArgumentNullException">motor is null.</exception>
        /// /// <exception cref="ArgumentOutOfRangeException">speedMultiplier is out of range.</exception>
        public void SetSpeedMultiplier(float speedMultiplier, Motor motor)
        {
            if (motor == null)
            {
                throw new ArgumentNullException(nameof(motor));
            }
            motor.SetSpeedMultiplier(speedMultiplier);
        }

        /// <summary>
        /// Sets the reverse and forward speed limits of a motor.
        /// </summary>
        /// <param name="maxReverseSpeed">A value between -100 and 0 inclusive.</param>
        /// <param name="maxForwardSpeed">A value between 1 and 100 inclusinve.</param>
        /// <param name="motor">The motor that will be affected.</param>
        /// <exception cref="ArgumentNullException">motor is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxReverseSpeed or maxForwardSpeed is out of range.</exception>
        public void SetSpeedLimits(int maxReverseSpeed, int maxForwardSpeed, Motor motor)
        {
            if (motor == null)
            {
                throw new ArgumentNullException(nameof(motor));
            }
            motor.SetSpeedLimits(maxReverseSpeed, maxForwardSpeed);
        }
    }
}
