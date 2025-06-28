using Messaging;

namespace RoboTx.Api
{
    internal class SwitchMessage : SerializableMessage
    {
        private const string MessageType = "SW";
        volatile private int _switchNumber;
        volatile private bool _state;

        public SwitchMessage(StreamWriter sw) : base(sw)
        {

        }

        public void SetState(int switchNumber, bool state)
        {
            _switchNumber = switchNumber;
            _state = state;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            SerializeProperty(_switchNumber.ToString());
            SerializeProperty(_state ? "T" : "F");
        }
    }
}
