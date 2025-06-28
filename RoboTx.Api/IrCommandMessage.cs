using Messaging;
using System.Globalization;


namespace RoboTx.Api
{
    internal class IrCommandMessage : IDeserializableMessage
    {
        public const string MessageType = "I";

        private readonly RobotIO _robotIO;
        private int _irCommand = -1;

        public IrCommandMessage(RobotIO robotIO)
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
            switch (propertyIndex)
            {
                case 0:
                    _irCommand = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
            }

        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete)
            {
                if (_irCommand >= 0)
                {
                    _robotIO.QueueIrCommand(_irCommand);
                }
            }
        }
    }
}
