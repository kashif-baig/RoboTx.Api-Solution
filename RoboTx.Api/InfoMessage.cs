using Messaging;

namespace RoboTx.Api
{
    internal class InfoMessage : IDeserializableMessage
    {
        internal const string MessageType = "RTx";
        private string _robotId = string.Empty;
        readonly private RobotIO _robotIO;

        public InfoMessage(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        public string GetMessageType()
        {
            return MessageType;
        }

        public void OnBeginDeserialize()
        {

        }

        public void OnDeserializeProperty(int propertyIndex, string propertyValue)
        {
            _robotId = propertyValue;
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (!string.IsNullOrEmpty(_robotId))
            {
                _robotIO.RobotId = _robotId;
            }
        }
    }
}
