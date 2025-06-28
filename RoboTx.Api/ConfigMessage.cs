using Messaging;

namespace RoboTx.Api
{
    internal enum MotorDriverType
    {
        NotSet,
        MotorInIn,
        MotorDirPwm,
        L298
    }

    internal class ConfigMessage : SerializableMessage
    {
        public const string MessageType = "CFG";
        private bool? _displayIsInverse = null;
        private bool? _displayIsRTL = null;
        private bool? _inputsinverted = null;
        private bool? _beeperInverted = null;
        MotorDriverType _motorDriverType = MotorDriverType.NotSet;
        private bool? _motorUseEnabled = null;
        private bool? _inputEventsEnabled = null;
        private string _inputPins = string.Empty;
        private bool? _analogInputsEnabled = null;

        public ConfigMessage(StreamWriter sw) : base(sw)
        {

        }

        public void SetDisplayInverse(bool isInverse)
        {
            _displayIsInverse = isInverse;
        }

        public void SetDisplayRTL(bool isRTL)
        {
            _displayIsRTL = isRTL;
        }

        public void SetBeeperInverted(bool beeperInverted)
        {
            _beeperInverted = beeperInverted;
        }

        public void UseMotorDriverL298()
        {
            _motorDriverType = MotorDriverType.L298;
        }

        public void UseMotorDriverInIn()
        {
            _motorDriverType = MotorDriverType.MotorInIn;
        }

        public void UseMotorDriverDirPwm()
        {
            _motorDriverType = MotorDriverType.MotorDirPwm;
        }

        public void UseMotorDriver2DirPwm(bool useEnable)
        {
            _motorDriverType = MotorDriverType.MotorDirPwm;
            _motorUseEnabled = useEnable;
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
            if (_displayIsInverse != null)
            {
                SerializeProperty(_displayIsInverse.Value ? "7SegInv" : "7SegNorm");
            }
            else if (_displayIsRTL != null)
            {
                SerializeProperty(_displayIsRTL.Value ? "7SegRTL" : "7SegLTR");
            }
            else if (_inputsinverted != null)
            {
                //SerializeProperty(_inputsinverted.Value ? "InpInv" : "InpNorm");
                SerializeProperty("InpInv");
                SerializeProperty(_inputsinverted.Value ? "T" : "F");
                SerializeProperty(_inputPins);
            }
            else if (_beeperInverted != null)
            {
                SerializeProperty(_beeperInverted.Value ? "BprInv" : "BprNorm");
            }
            else if (_motorDriverType == MotorDriverType.MotorInIn)
            {
                SerializeProperty("InIn");
                //if (_motorUseEnabled.HasValue)
                //{
                //    SerializeProperty(_motorUseEnabled.Value ? "T" : "F");
                //}
            }
            else if (_motorDriverType == MotorDriverType.MotorDirPwm)
            {
                SerializeProperty("DirPwm");
                if (_motorUseEnabled.HasValue)
                {
                    SerializeProperty(_motorUseEnabled.Value ? "T" : "F");
                }
            }
            else if (_motorDriverType == MotorDriverType.L298)
            {
                SerializeProperty("L298");
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
