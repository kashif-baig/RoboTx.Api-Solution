namespace RoboTx.Api
{
    /// <summary>
    /// Configures the type of motor driver connected to the Arduino.
    /// </summary>
    public sealed class Config
    {
        private readonly RobotIO _robotIO;

        internal Config(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        ///// <summary>
        ///// Works with L298 breakout board which allows control of 2 motors using 3 Arduino
        ///// pins per motor. One pin for PWM, two pins for motor direction.
        ///// </summary>
        ///// <exception cref="IOException">Serial port is in error state or not open.</exception>
        //internal void UseMotorDriverL298()
        //{
        //    _robotIO.CheckSerialState();
        //    ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
        //    msg.UseMotorDriverL298();
        //    _robotIO.MessageSender.EnQueueMessage(msg);
        //}

        ///// <summary>
        ///// Use for a motor controller which allows 2 motors to be driven using 2 Arduino
        ///// pins per motor. Uses either pin for PWM.
        ///// </summary>
        ///// <exception cref="IOException">Serial port is in error state or not open.</exception>
        //internal void UseMotorDriverInIn()
        //{
        //    _robotIO.CheckSerialState();
        //    ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
        //    msg.UseMotorDriverInIn();
        //    _robotIO.MessageSender.EnQueueMessage(msg);
        //}

        ///// <summary>
        ///// Works with motor controller which allows 2 motors to be driven using 2 Arduino
        ///// pins per motor. One pin is for direction, the other for PWM.
        ///// </summary>
        ///// <param name="useEnable">Set to true to use an Arduino pin as motor master enable.</param>
        ///// <exception cref="IOException">Serial port is in error state or not open.</exception>
        //internal void UseMotorDriverDirPwm(bool useEnable)
        //{
        //    _robotIO.CheckSerialState();
        //    ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
        //    msg.UseMotorDriver2DirPwm(useEnable);
        //    _robotIO.MessageSender.EnQueueMessage(msg);
        //}

        ///// <summary>
        ///// Works with motor controller which allows 2 motors to be driven using 2 Arduino
        ///// pins per motor. One pin is for direction, the other for PWM.
        ///// </summary>
        //internal void UseMotorDriverDirPwm()
        //{
        //    _robotIO.CheckSerialState();
        //    ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
        //    msg.UseMotorDriverDirPwm();
        //    _robotIO.MessageSender.EnQueueMessage(msg);
        //}
    }
}
