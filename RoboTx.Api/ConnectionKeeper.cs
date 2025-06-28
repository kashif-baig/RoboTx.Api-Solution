namespace RoboTx.Api
{
    internal class ConnectionKeeper
    {
        readonly RobotIO _robotIO;
        readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        public ConnectionKeeper(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Task to signal connection alive to Arduino.
        /// </summary>
        public void KeepAliveTask()
        {
            while (!_cancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (_robotIO.IsConnected)
                    {
                        ConnectionMessage connMsg = new ConnectionMessage(_robotIO.StreamWriter);
                        connMsg.KeepAlive();
                        connMsg.Serialize();
                    }
                }
                catch
                {

                }
                Thread.Sleep(211);
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
