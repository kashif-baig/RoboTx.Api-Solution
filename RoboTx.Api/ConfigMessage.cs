using Messaging;

namespace RoboTx.Api
{
    internal class ConfigMessage : SerializableMessage
    {
        public const string MessageType = "CFG";
        private bool? _inputsinverted = null;
        private bool? _inputEventsEnabled = null;
        private string _inputPins = string.Empty;
        private bool? _analogInputsEnabled = null;

        public ConfigMessage(StreamWriter sw) : base(sw)
        {

        }

        public void InvertDigitalInputs(bool inverted, params int[] inputPins)
        {
            _inputsinverted = inverted;

            var pinArray = "00000".ToArray();

            for (int i = 0; i < inputPins.Length; i++)
            {
                pinArray[pinArray.Length - inputPins[i] - 1] = '1';
            }
            _inputPins = new string(pinArray);
        }

        public void EnableInputEvents(bool enabled, params int[] inputPins)
        {
            _inputEventsEnabled = enabled;

            var pinArray = "00000".ToArray();

            for (int i = 0; i < inputPins.Length; i++)
            {
                pinArray[pinArray.Length - inputPins[i] - 1] = '1';
            }
            _inputPins = new string(pinArray);
        }

        public void EnableAnalogInputs(bool enabled, params int[] inputPins)
        {
            _analogInputsEnabled = enabled;

            var pinArray = "00000000".ToArray();

            for (int i = 0; i < inputPins.Length; i++)
            {
                pinArray[pinArray.Length - inputPins[i] - 1] = '1';
            }
            _inputPins = new string(pinArray);
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            if (_inputsinverted != null)
            {
                SerializeProperty("InpInv");
                SerializeProperty(_inputsinverted.Value ? "T" : "F");
                SerializeProperty(_inputPins);
            }
            else if (_inputEventsEnabled != null)
            {
                SerializeProperty("InpEvt");
                SerializeProperty(_inputEventsEnabled.Value ? "T" : "F");
                SerializeProperty(_inputPins);
            }
            else if (_analogInputsEnabled != null)
            {
                SerializeProperty("InpAnlg");
                SerializeProperty(_analogInputsEnabled.Value ? "T" : "F");
                SerializeProperty(_inputPins);
            }
        }
    }
}
