using Messaging;
using System.Globalization;

namespace RoboTx.Api
{
    internal class ButtonMessage : IDeserializableMessage
    {
        public const string MessageType = "B";

        private int _buttonValue = 0;
        private readonly RobotIO _robotIO;

        public ButtonMessage(RobotIO robotIO)
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
            //Debug.WriteLine($"Property Index:Value {propertyIndex}:{propertyValue}");
            _buttonValue = int.Parse(propertyValue, NumberStyles.HexNumber);
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete && _buttonValue > 0 && _buttonValue < 256)
            {
                _robotIO.Digital.QueueInputEvent(_buttonValue);
            }
        }
    }
}
