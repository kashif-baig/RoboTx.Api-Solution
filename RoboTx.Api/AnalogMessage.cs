using Messaging;
using System.Globalization;

namespace RoboTx.Api
{
    internal class AnalogMessage : SerializableMessage, IDeserializableMessage
    {
        internal const string MessageType = "A";
        private readonly Analog _analog;
        private const string CMD_SET_SAMPLE_RATE = "S";

        volatile private bool _setSampleRate = false;
        volatile int _sampleRate;

        private int _A0 = -1;
        private int _A1 = -1;
        private int _A2 = -1;
        private int _A3 = -1;
        private int _A4 = -1;
        private int _A5 = -1;
        private int _A6 = -1;
        private int _A7 = -1;

        internal AnalogMessage(StreamWriter sw) : base(sw)
        {

        }

        public AnalogMessage(Analog analog)
        {
            _analog = analog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sampleRateHz"></param>
        /// <exception cref="ArgumentOutOfRangeException">sampleRateHz has value less than 1 or greater than 50.</exception>
        public void SetSampleRate(int sampleRateHz)
        {
            if (sampleRateHz < 1 || sampleRateHz > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(sampleRateHz));
            }
            _sampleRate = sampleRateHz;
            _setSampleRate = true;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            if (_setSampleRate)
            {
                SerializeProperty(CMD_SET_SAMPLE_RATE);
                SerializeProperty(_sampleRate.ToString());
            }
        }

        public void OnBeginDeserialize()
        {

        }

        public void OnDeserializeProperty(int propertyIndex, string propertyValue)
        {
            switch (propertyIndex)
            {
                case 0:
                    _A0 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 1:
                    _A1 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 2:
                    _A2 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 3:
                    _A3 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 4:
                    _A4 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 5:
                    _A5 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 6:
                    _A6 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
                case 7:
                    _A7 = int.Parse(propertyValue, NumberStyles.HexNumber);
                    break;
            }
        }

        public void OnEndDeserialize(bool messageComplete)
        {
            if (messageComplete)
            {
                _analog.A0.Value = _A0 > -1 ? _A0 : _analog.A0.Value;
                _analog.A1.Value = _A1 > -1 ? _A1 : _analog.A1.Value;
                _analog.A2.Value = _A2 > -1 ? _A2 : _analog.A2.Value;
                _analog.A3.Value = _A3 > -1 ? _A3 : _analog.A3.Value;
                _analog.A4.Value = _A4 > -1 ? _A4 : _analog.A4.Value;
                _analog.A5.Value = _A5 > -1 ? _A5 : _analog.A5.Value;
                _analog.A6.Value = _A6 > -1 ? _A6 : _analog.A6.Value;
                _analog.A7.Value = _A7 > -1 ? _A7 : _analog.A7.Value;
            }
        }
    }
}
