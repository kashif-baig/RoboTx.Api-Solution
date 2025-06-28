namespace RoboTx.Api
{
    /// <summary>
    /// A digital trigger for repeating pulse cycle patterns.
    /// Use for an audio or visual alert (e.g. active beeper) using repeating cycle patterns.
    /// The Arduino digital pin and values that represent on and off are configured in the firmware settings.
    /// </summary>
    public sealed class Trigger
    {
        private readonly RobotIO _robotIO;
        private volatile bool _activeIndefinitely = false;
        private DateTime _lapsedTime = DateTime.MinValue;

        internal Trigger(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        /// <summary>
        /// Returns true if the trigger is on or running a pattern cycle. Returns false otherwise.
        /// </summary>
        public bool IsActive { get { return _activeIndefinitely || DateTime.Now <= _lapsedTime; } }


        /// <summary>
        /// Stops the trigger if it is on or running a pattern cycle.
        /// </summary>
        public void Off()
        {
            RunPattern(0, 0, 1);
        }

        /// <summary>
        /// Switches on the trigger for an indefinite period.
        /// </summary>
        public void On()
        {
            RunPattern(50, 0, 1, 0, 0);
        }

        /// <summary>
        /// Trigger a short pulse.
        /// </summary>
        public void Pulse()
        {
            RunPattern(50, 0, 1);
        }

        /// <summary>
        /// Trigger a pulse for a specified period in milliseconds.
        /// </summary>
        /// <param name="onPeriod">On for specified milliseconds.</param>
        public void Pulse(int onPeriod)
        {
            RunPattern(onPeriod, 0, 1);
        }

        /// <summary>
        /// Pulses the trigger indefinitely with a continous sequence of on/off periods.
        /// </summary>
        /// <param name="onPeriod">On for specified milliseconds.</param>
        /// <param name="offPeriod">Off for specified milliseconds.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Repeat(int onPeriod, int offPeriod)
        {
            RunPattern(onPeriod, offPeriod, 1, 0, 0);
        }

        /// <summary>
        /// Runs a repeating pattern of digital pulses.
        /// </summary>
        /// <param name="onPeriod">On for specified milliseconds.</param>
        /// <param name="offPeriod">Off for specified milliseconds.</param>
        /// <param name="cycles">Repeat the pulse/off cycle for specified number of times.</param>
        /// <param name="loopCycles">The number of times to loop (repeat) the pulse/off/cycle. 0=indefinite.</param>
        /// <param name="loopDelayPeriod">Interval between loops in millisecionds.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void RunPattern(int onPeriod, int offPeriod, int cycles, int loopCycles = 1 /* 0=indefinitely */, int loopDelayPeriod = 0)
        {
            _robotIO.CheckSerialState();

            TriggerMessage msg = new TriggerMessage(_robotIO.StreamWriter);
            msg.RunPattern(onPeriod / 10, offPeriod / 10, cycles, loopCycles, loopDelayPeriod / 10);

            _robotIO.MessageSender.EnQueueMessage(msg);

            _activeIndefinitely = loopCycles == 0;
            _lapsedTime = DateTime.Now.AddMilliseconds(((((onPeriod + offPeriod) * cycles) + loopDelayPeriod) * loopCycles) -loopDelayPeriod);
        }

        /// <summary>
        /// Sets the off period between pulses whilst the trigger is cyclng through a repeat pattern.
        /// </summary>
        /// <param name="offPeriod">Off period in milliseconds.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void SetOffPeriod(int offPeriod)
        {
            _robotIO.CheckSerialState();

            TriggerMessage msg = new TriggerMessage(_robotIO.StreamWriter);
            msg.SetOffPeriod(offPeriod / 10);

            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Sets the off period between pulses whilst the trigger is cyclng through a repeat pattern.
        /// </summary>
        /// <param name="offPeriod">Off period in milliseconds.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void SetOffPeriod(float offPeriod)
        {
            SetOffPeriod((int)offPeriod);
        }
    }
}
