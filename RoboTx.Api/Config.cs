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
    }
}
