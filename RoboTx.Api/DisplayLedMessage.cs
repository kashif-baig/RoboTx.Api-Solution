using Messaging;

namespace RoboTx.Api
{
    internal enum DisplayLedCommand
    {
        None,
        Write,
        BlinkOn,
        BlinkOff,
        Brightness
    }

    internal class DisplayLedMessage : SerializableMessage
    {
        private const string MessageType = "D7";
        private const string CMD_WRITE = "W";
        private const string CMD_BLINK = "B";
        private const string CMD_BRIGHTNESS = "L";

        volatile private DisplayLedCommand _cmd;
        volatile private string _displayText = string.Empty;
        volatile private string _blinkDigits = string.Empty;
        volatile private int _brightnessValue = 2;

        internal DisplayLedMessage(StreamWriter sw) : base(sw)
        {
        }

        public void Write(string text)
        {
            _cmd = DisplayLedCommand.Write;
            if (text.Length > 8)
            {
                _displayText = text.Substring(0, 8);
            }
            else
            {
                _displayText = text;
            }
        }

        //public void Blink(bool on, params int[] digits)
        //{
        //    var digitArray = "0000".ToArray();

        //    if (digits == null || digits.Length == 0)
        //    {
        //        throw new ArgumentException($"Parameter {digits} not supplied.");
        //    }
        //    for (int i = 0; i < digits.Length; i++)
        //    {
        //        if (digits[i] < 1 || digits[i] > 4)
        //        {
        //            throw new ArgumentOutOfRangeException($"Parameter {digits} out of range.");
        //        }
        //        digitArray[digits[i] - 1] = '1';
        //    }
        //    _cmd = on ? Display7SegCommand.BlinkOn : Display7SegCommand.BlinkOff;

        //    _blinkDigits = new string(digitArray);

        //}

        //public void SetBrightness(int value)
        //{
        //    if (value < 0 || value > 3)
        //    {
        //        throw new ArgumentException($"'{nameof(value)}' out of range.", nameof(value));
        //    }

        //    _cmd = Display7SegCommand.Brightness;
        //    _brightnessValue = value;
        //}

        public override string GetMessageType()
        {
            return MessageType;
        }

        protected override void OnSerialize()
        {
            switch (_cmd)
            {
                case DisplayLedCommand.Write:
                    SerializeProperty(CMD_WRITE);
                    SerializeProperty(_displayText);
                    break;
                case DisplayLedCommand.BlinkOn:
                case DisplayLedCommand.BlinkOff:
                    SerializeProperty(CMD_BLINK);
                    SerializeProperty(_blinkDigits);
                    SerializeProperty(_cmd == DisplayLedCommand.BlinkOn ? "1" : "0");
                    break;
                case DisplayLedCommand.Brightness:
                    SerializeProperty(CMD_BRIGHTNESS);
                    SerializeProperty("");
                    SerializeProperty(_brightnessValue.ToString());
                    break;
            }
        }
    }
}
