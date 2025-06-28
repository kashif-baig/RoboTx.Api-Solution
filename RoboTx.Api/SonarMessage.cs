using Messaging;

namespace RoboTx.Api
{
    internal class SonarMessage : SerializableMessage, IDeserializableMessage
    {
        internal const string MessageType = "R";
        private Sonar _sonar;
        private int _distance = 0;
        private bool _distanceIsValid = false;

        internal SonarMessage(StreamWriter sw) : base(sw)
        {

        }
        public SonarMessage(Sonar sonar)
        {
            _sonar = sonar;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        public void OnBeginDeserialize()
        {
            // Intended to be empty.
        }

        public void OnDeserializeProperty(int propertyIndex, string propertyValue)
        {
            switch (propertyIndex)
            {
                case 0:
                    _distanceIsValid = int.TryParse(propertyValue, out _distance);

                    break;
            }
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete && _distanceIsValid)
            {
                _sonar.SetDistance(_distance);
            }
        }

        protected override void OnSerialize()
        {
            // Intended to be empty.
        }
    }
}
