using Messaging;
using System.Globalization;

namespace RoboTx.Api
{
    internal class ColourMessage : SerializableMessage, IDeserializableMessage
    {
        private const string CMD_DISABLE = "F";
        private const string CMD_ENABLE = "T";

        internal const string MessageType = "L";
        private ColourSensor _colour;
        private bool _enabled = false;

        private int _red;
        private int _green;
        private int _blue;
        private int _clear;
        private int _integrationTime;
        private int _gain;

        /// <summary>
        /// Constructor used for serializing.
        /// </summary>
        /// <param name="sw"></param>
        internal ColourMessage(StreamWriter sw) : base(sw)
        {

        }

        /// <summary>
        /// Constructor used for deserializing.
        /// </summary>
        /// <param name="colour"></param>
        internal ColourMessage(ColourSensor colour)
        {
            _colour = colour;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        public void Enable(int integrationTime, int gain)
        {
            _integrationTime = Math.Clamp(integrationTime, 0, 5);
            _gain = Math.Clamp(gain, 0, 3);
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void OnBeginDeserialize()
        {

        }

        public void OnDeserializeProperty(int propertyIndex, string propertyValue)
        {
            switch (propertyIndex)
            {
                case 0:
                    _red = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 1:
                    _green = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 2:
                    _blue = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 3:
                    _clear = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
            }
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete)
            {
                _colour.SetRGBC(_red, _green, _blue, _clear);
            }
        }

        protected override void OnSerialize()
        {
            SerializeProperty(_enabled ? CMD_ENABLE : CMD_DISABLE);
            SerializeProperty(_integrationTime.ToString());
            SerializeProperty(_gain.ToString());
        }
    }
}
