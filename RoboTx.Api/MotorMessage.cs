using Messaging;

namespace RoboTx.Api
{
    internal enum MotorCommand
    {
        NotSet = 0,
        Drive,
        DriveForDuration,
        DriveAndOverrideAcceleration,
        StopAcceleration,
        SetAccel,

    }

    internal class MotorMessage : SerializableMessage
    {
        private const string MessageType = "M";
        private const string MR_CMD_SET_ACCEL = "A";
        private const string MR_CMD_OVERRIDE_ACCEL = "O";
        private const string MR_CMD_FOR_DURATION = "T";

        volatile private int _motorNumber;
        volatile private byte _speedPercent;
        volatile private int _accel;
        volatile private int _durationMs;

        volatile MotorCommand _cmd;

        public MotorMessage(StreamWriter sw) : base(sw)
        {
        }

        private void Drive(int motorNumber, int speedPercent)
        {
            Drive(motorNumber, speedPercent, false);
        }

        public void Drive(int motorNumber, int speedPercent, int durationMs)
        {
            _cmd = MotorCommand.DriveForDuration;
            _motorNumber = motorNumber;
            _speedPercent = (byte)speedPercent;
            _durationMs = durationMs;
        }

        public void Drive(int motorNumber, int speedPercent, bool overrideAccel)
        {
            _cmd = overrideAccel ? MotorCommand.DriveAndOverrideAcceleration : MotorCommand.Drive;
            _motorNumber = motorNumber;
            _speedPercent = (byte)speedPercent;
        }

        public void SetAcceleration(int motorNumber, int acceleration)
        {
            _cmd = MotorCommand.SetAccel;
            _motorNumber = motorNumber;
            _accel = acceleration;
        }

        public void StopAccelerating(int motorNumber)
        {
            _cmd = MotorCommand.StopAcceleration;
            _motorNumber = motorNumber;
        }

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            SerializeProperty(_motorNumber.ToString());

            switch (_cmd)
            {
                case MotorCommand.Drive:
                    SerializeProperty(Convert.ToHexString(new[] { _speedPercent }));
                    break;
                case MotorCommand.DriveForDuration:
                    SerializeProperty(Convert.ToHexString(new[] { _speedPercent }));
                    SerializeProperty(MR_CMD_FOR_DURATION);
                    SerializeProperty(_durationMs.ToString("X4"));
                    break;
                case MotorCommand.DriveAndOverrideAcceleration:
                    SerializeProperty(Convert.ToHexString(new[] { _speedPercent }));
                    SerializeProperty(MR_CMD_OVERRIDE_ACCEL);
                    break;
                case MotorCommand.StopAcceleration:
                    SerializeProperty("");
                    SerializeProperty(MR_CMD_OVERRIDE_ACCEL);
                    break;
                case MotorCommand.SetAccel:
                    SerializeProperty(_accel.ToString("X4"));
                    SerializeProperty(MR_CMD_SET_ACCEL);
                    break;
            }
        }
    }
}
