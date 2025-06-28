using Messaging;

namespace RoboTx.Api
{
    internal class MyMessageDeserializerFactory : IDeserializableMessageFactory
    {
        private RobotIO _robotIO;

        public MyMessageDeserializerFactory(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        public IDeserializableMessage GetMessageDeserializer(string messageType)
        {
            if (messageType == ButtonMessage.MessageType)
            {
                return new ButtonMessage(_robotIO);
            }
            if (messageType == AnalogMessage.MessageType)
            {
                return new AnalogMessage(_robotIO.Analog);
            }
            if (messageType == PulseCounterMessage.MessageType)
            {
                return new PulseCounterMessage(_robotIO.PulseCounter);
            }
            if (messageType == ColourMessage.MessageType)
            {
                return new ColourMessage(_robotIO.ColourSensor);
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
