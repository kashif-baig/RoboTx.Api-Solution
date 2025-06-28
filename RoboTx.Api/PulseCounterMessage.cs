using Messaging;

namespace RoboTx.Api
{
    internal class PulseCounterMessage : SerializableMessage, IDeserializableMessage
    {
        internal const string MessageType = "P";
        private readonly PulseCounter _pulseCounter;
        private const string CMD_DISABLE = "F";
        private const string CMD_ENABLE = "T";
        volatile private int _timeoutMs = 200;
        volatile private int _trigger = 0;
        volatile private string _cmd = CMD_DISABLE;

        /// <summary>
        /// Constructor used for serializing.
        /// </summary>
        /// <param name="sw"></param>
        internal PulseCounterMessage(StreamWriter sw) : base(sw)
        {

        }

        /// <summary>
        /// Constructor used for deserializing.
        /// </summary>
        /// <param name="pulseCounter"></param>
        internal PulseCounterMessage(PulseCounter pulseCounter)
        {
            _pulseCounter = pulseCounter;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        public void Disable()
        {
            _cmd = CMD_DISABLE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <param name="triggerEdge"></param>
        /// <exception cref="ArgumentOutOfRangeException">timeoutMs is out of range.</exception>
        public void Enable(int timeoutMs, int triggerEdge)
        {
            if (timeoutMs < 250)
            {
                throw new ArgumentOutOfRangeException(nameof(timeoutMs));
            }
            if (timeoutMs > 10000)
            {
                throw new ArgumentOutOfRangeException(nameof(timeoutMs));
            }
            _timeoutMs = timeoutMs;
            _trigger = triggerEdge;
            _cmd = CMD_ENABLE;
        }

        public void OnBeginDeserialize()
        {

        }

        public void OnDeserializeProperty(int propertyIndex, string propertyValue)
        {
            switch (propertyIndex)
            {
                case 0:
                    int period;

                    if (int.TryParse(propertyValue, out period))
                    {
                        _pulseCounter.Period = period;
                    }
                    break;
            }
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete)
            {
            }
        }

        protected override void OnSerialize()
        {
            if (_cmd == CMD_ENABLE)
            {
                SerializeProperty(_timeoutMs.ToString());
                SerializeProperty(_trigger.ToString());
            }
            else
            {
                SerializeProperty(CMD_DISABLE);
            }
        }
    }
}
