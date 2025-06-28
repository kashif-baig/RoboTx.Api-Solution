using Messaging;
using System.Diagnostics;

namespace RoboTx.Api
{
    internal class DebugMessage : IDeserializableMessage
    {
        public const string MessageType = "DB";

        public DebugMessage()
        {
            //_robotIO = robotIO;
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
            Debug.WriteLine(propertyValue);
        }

        public void OnEndDeserialize(bool messageComplete)
        {
        }
    }
}
