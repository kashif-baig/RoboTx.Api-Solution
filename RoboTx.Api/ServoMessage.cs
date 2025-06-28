using Messaging;

namespace RoboTx.Api
{
    internal enum ServoCommand
    {
        None = 0,
        Stop,
        SetRange,
        SetSpeedStep
    }

    internal class ServoMessage : SerializableMessage
    {
        private const string MessageType = "V";
        private const string CMD_STOP = "S";
        private const string CMD_RANGE = "R";
        private const string CMD_SPEEDSTEP = "P";

        private volatile ServoCommand _servoCmd;

        private volatile int _speedStep = 0;
        private volatile int _pulseWidth = 0;
        private volatile int _servoNumber = 1;
        //private volatile bool _stop = false;

        //private volatile bool _setRange = false;
        private volatile int _minPulseWidth = 0;
        private volatile int _maxPulseWidth = 0;

        public ServoMessage(StreamWriter sw) : base(sw)
        {

        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        public void SetSpeedStep(int servoNumber, int speedStep)
        {
            _servoNumber = servoNumber;
            _speedStep  = Utility.Constrain(speedStep, 1, 50);
            _servoCmd = ServoCommand.SetSpeedStep;
        }

        public void SetRange(int servoNumber, int min, int max)
        {
            //ValidateServoNumber(servoNumber);
            _servoNumber = servoNumber;
            _minPulseWidth = min;
            _maxPulseWidth = max;
            //_setRange = true;
            _servoCmd = ServoCommand.SetRange;
        }

        public void SetPulseWidth(int servoNumber, int pulseWidth)
        {
            //ValidateServoNumber(servoNumber);
            _servoNumber = servoNumber;
            _pulseWidth = pulseWidth;

        }

        public void Stop(int servoNumber)
        {
            //ValidateServoNumber(servoNumber);
            _servoNumber = servoNumber;
            //_stop = true;
            _servoCmd = ServoCommand.Stop;
        }

        protected override void OnSerialize()
        {
            SerializeProperty(_servoNumber.ToString());
            if (_servoCmd == ServoCommand.Stop)
            {
                SerializeProperty(CMD_STOP);
            }
            else if (_servoCmd == ServoCommand.SetRange)
            {
                SerializeProperty(CMD_RANGE);
                SerializeProperty(_minPulseWidth.ToString("x"));
                SerializeProperty(_maxPulseWidth.ToString("x"));
            }
            else if (_servoCmd == ServoCommand.SetSpeedStep)
            {
                SerializeProperty(CMD_SPEEDSTEP);
                SerializeProperty(_speedStep.ToString("x"));
            }
            else
            {
                SerializeProperty(_pulseWidth.ToString("x"));
            }

        }

        //private static void ValidateServoNumber(int servoNumber)
        //{
        //    if (servoNumber < 1 || servoNumber > 4)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(servoNumber));
        //    }
        //}
    }
}
