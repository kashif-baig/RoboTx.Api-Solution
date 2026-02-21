using Messaging;

namespace RoboTx.Api
{
    internal abstract class DeserializableMessageFactoryBase
    {
        protected RobotIO _robotIO;

        public virtual IDeserializableMessage GetMessageDeserializer(string messageType)
        {
            if (messageType == ButtonMessage.MessageType)
            {
                return new ButtonMessage(_robotIO);
            }
            if (messageType == AnalogMessage.MessageType)
            {
                return new AnalogMessage(_robotIO.Analog);
            }
            if (messageType == SonarMessage.MessageType)
            {
                return new SonarMessage(_robotIO.Sonar);
            }
            if (messageType == IrCommandMessage.MessageType)
            {
                return new IrCommandMessage(_robotIO);
            }
            if (messageType == DebugMessage.MessageType)
            {
                return new DebugMessage();
            }
            if (messageType == InfoMessage.MessageType)
            {
                return new InfoMessage(_robotIO);
            }
            return null;
        }
    }
}
