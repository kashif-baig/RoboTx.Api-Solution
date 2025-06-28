using Messaging;

namespace RoboTx.Api
{
    internal enum TriggerCommand
    {
        None,
        Run,
        SetOffPeriod,
    }

    internal class TriggerMessage : SerializableMessage
    {
        private const string MessageType = "Z";
        private const string CMD_RUN = "B";
        private const string CMD_SET_OFF_PERIOD = "F";

        volatile TriggerCommand _cmd;

        volatile int _onPeriod = 20;
        volatile int _offPeriod = 0;
        volatile int _cycles = 1;
        volatile int _loopCycles = 1;
        volatile int _loopDelayPeriod = 0;

        public TriggerMessage(StreamWriter sw) : base(sw)
        {

        }

        public void Clear()
        {
            RunPattern(0);
        }

        public void RunPattern(int onPeriod = 20, int offPeriod = 0, int cycles = 1, int loopCycles = 1 /* 0=indefinitely */, int loopDelayPeriod = 0)
        {
            _cmd = TriggerCommand.Run;

            _onPeriod = Utility.Constrain(onPeriod, 0, 500);
            _offPeriod = Utility.Constrain(offPeriod, 0, 500);
            _cycles = Utility.Constrain(cycles, 0, 255);
            _loopCycles = Utility.Constrain(loopCycles, 0, 999);
            _loopDelayPeriod = Utility.Constrain(loopDelayPeriod, 0, 500);
        }

        public void SetOffPeriod(int offPeriod)
        {
            _cmd = TriggerCommand.SetOffPeriod;
            _offPeriod = offPeriod;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            switch (_cmd)
            {
                case TriggerCommand.Run:
                    SerializeProperty(CMD_RUN);
                    SerializeProperty(_onPeriod.ToString());
                    SerializeProperty(_offPeriod.ToString());
                    SerializeProperty(_cycles.ToString());
                    SerializeProperty(_loopCycles.ToString());
                    SerializeProperty(_loopDelayPeriod.ToString());
                    break;
                case TriggerCommand.SetOffPeriod:
                    SerializeProperty(CMD_SET_OFF_PERIOD);
                    SerializeProperty("");
                    SerializeProperty(_offPeriod.ToString());
                    break;
            }
        }
    }
}
