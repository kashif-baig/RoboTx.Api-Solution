using Messaging;

namespace RoboTx.Api
{
    internal partial class DeserializableMessageFactory : DeserializableMessageFactoryBase, IDeserializableMessageFactory
    {
        public DeserializableMessageFactory(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }


    }
}
