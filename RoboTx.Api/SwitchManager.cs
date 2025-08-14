namespace RoboTx.Api
{
    internal class SwitchManager
    {
        readonly RobotIO _robotIO;
        readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        readonly List<Switch> _switches = new List<Switch>();

        public SwitchManager(RobotIO robotIO)
        {
            _robotIO = robotIO;
            _switches.AddRange(new Switch[] { _robotIO.Switch1, _robotIO.Switch2, _robotIO.Switch3, _robotIO.Switch4 });
        }

        /// <summary>
        /// Manages the On duration of switches.
        /// </summary>
        public void ManageSwitches()
        {
            while (!_cancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (_robotIO.IsConnected)
                    {
                        foreach (var theSwitch in _switches)
                        {
                            if (theSwitch.DurationIsEnabled && theSwitch.IsOn && theSwitch.DurationLapsed)
                            {
                                theSwitch.Off();
                            }
                        }
                    }
                }
                catch
                {

                }
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// Cancels the task.
        /// </summary>
        public void Cancel()
        {
            _cancelTokenSource.Cancel();
        }
    }
}
