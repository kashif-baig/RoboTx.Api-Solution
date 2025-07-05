using System.Diagnostics;

namespace RoboTx.Api
{
    /// <summary>
    /// Configures and reports digital inputs via Arduino pins defined in the firmware profile. By default
    /// the input pins are A0 to A4, but can be overridden for a specific profile using firmware macro DIGITAL_INPUT_PINS.
    /// Digital inputs are identified by their index position. I.e. index 0 by default corresponds to A0, index 1 to A1 etc.
    /// Also reports received IR commands sent by an IR remote control.
    /// </summary>
    public sealed class Digital
    {
        readonly private RobotIO _robotIO;
        readonly Queue<int> _inputEventsReceivedQueue;
        readonly bool[] _inputState = { false, false, false, false, false };

        internal Digital(RobotIO robotIO)
        {
            _robotIO = robotIO;
            _inputEventsReceivedQueue = new Queue<int>(25);

            IN0 = new DigitalInput(this, 0);
            IN1 = new DigitalInput(this, 1);
            IN2 = new DigitalInput(this, 2);
            IN3 = new DigitalInput(this, 3);
            IN4 = new DigitalInput(this, 4);
        }

        internal bool[] InputState { get => _inputState; }

        /// <summary>
        /// Gets the digital reading for input 0 whose value is either true or false.
        /// The corresponding pin must be <see cref="EnableInputs(int[])">enabled</see> for digital input before values become available.
        /// </summary>
        public DigitalInput IN0 { get; }
        /// <summary>
        /// Gets the digital reading for input 1 whose value is either true or false.
        /// The corresponding pin must be <see cref="EnableInputs(int[])">enabled</see> for digital input before values become available.
        /// </summary>
        public DigitalInput IN1 { get; }
        /// <summary>
        /// Gets the digital reading for input 2 whose value is either true or false.
        /// The corresponding pin must be <see cref="EnableInputs(int[])">enabled</see> for digital input before values become available.
        /// </summary>
        public DigitalInput IN2 { get; }
        /// <summary>
        /// Gets the digital reading for input 3 whose value is either true or false.
        /// The corresponding pin must be <see cref="EnableInputs(int[])">enabled</see> for digital input before values become available.
        /// </summary>
        public DigitalInput IN3 { get; }
        /// <summary>
        /// Gets the digital reading for input 4 whose value is either true or false.
        /// The corresponding pin must be <see cref="EnableInputs(int[])">enabled</see> for digital input before values become available.
        /// </summary>
        public DigitalInput IN4 { get; }

        /// <summary>
        /// Enables digital inputs and events whose sources are Arduino pins defined in the firmware profile.
        /// A short delay may need to be implemented after a call to this method to allow digital values to start being received.
        /// Enable only the input sources that are needed.
        /// Analog inputs for corresponding Arduino pins are disabled. Input pins are identified by their index position
        /// in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS.
        /// </summary>
        /// <param name="inputPins">The index positions of input pins whose value is between 0 an 4 inclusive.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentException">inputPins not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void EnableInputs(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            _robotIO.CheckSerialState();
            ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
            msg.EnableInputEvents(true, inputPins);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Disables digital inputs and events whose sources are Arduino pins defined in the firmware profile.
        /// Analog inputs for corresponding Arduino pins are disabled. The inverted configuration of any disabled
        /// inputs are reset. Input pins are identified by their index position
        /// in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS.
        /// </summary>
        /// <param name="inputPins">The index positions of input pins whose value is between 0 an 4 inclusive.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentException">inputPins not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void DisableInputs(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            _robotIO.CheckSerialState();
            ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
            msg.EnableInputEvents(false, inputPins);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }


        /// <summary>
        /// Configures Arduino digital inputs to be detected on Low (inverted) signal.
        /// This method should be called before enabling any pins for digital input. Input pins are identified by their index position
        /// in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS.
        /// </summary>
        /// <param name="inputPins">The index positions of input pins whose value is between 0 an 4 inclusive.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentException">inputPins not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void InvertInputs(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            _robotIO.CheckSerialState();
            ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
            msg.InvertDigitalInputs(true, inputPins);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }


        /// <summary>
        /// Resets the state of the specified inputs from true to false.
        /// Input pins are identified by their index position
        /// in the list of Arduino pins in firmware macro DIGITAL_INPUT_PINS.
        /// </summary>
        /// <param name="inputPins">The index positions of input pins whose value is between 0 an 4 inclusive.</param>
        /// <exception cref="ArgumentException">inputPins not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void ResetInputsState(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            for (int i = 0; i < inputPins.Length; i++)
            {
                _inputState[i] = false;
            }
        }

        /// <summary>
        /// If an input event (e.g. button press) has been detected, get its value or return -1 otherwise.
        /// Use class <see cref="RoboTx.Api.Input">Input</see> for symbolic labels for input events.
        /// Input events are only detected for Arduino pins that have been <see cref="EnableInputs(int[])">enabled</see>.
        /// </summary>
        /// <returns>An integer value representing a detected input event. A value of -1 indicates not events detected.</returns>
        public int GetInputEvent()
        {
            lock (_inputEventsReceivedQueue)
            {
                if (_inputEventsReceivedQueue.Count != 0)
                {
                    return _inputEventsReceivedQueue.Dequeue();
                }
            }
            return -1;
        }

        /// <summary>
        /// Clears queued inputs from the internal buffer and resets the current readings to false. Clears for the duration
        /// specified by parameter timeoutMs.
        /// </summary>
        /// <param name="timeoutMs">Time in milliseconds for which the input buffer is cleared.</param>
        public void ClearInputEvents(int timeoutMs = 250)
        {
            DateTime timeStart = DateTime.Now;

            while (DateTime.Now.Subtract(timeStart).TotalMilliseconds < timeoutMs)
            {
                Debug.WriteLine("Getting input event...");
                GetInputEvent();
                Thread.Sleep(10);
            }
            Array.Clear(_inputState);
        }

        /// <summary>
        /// Returns the number of digital inputs that are set to true.
        /// </summary>
        public int InputCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _inputState.Length; i++)
                {
                    if (_inputState[i] == true)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        private volatile int _prevIRCommand = -1;
        private DateTime _lastIRCommandTime = DateTime.MinValue;

        /// <summary>
        /// Gets the value and pressed state of an IR command button if one has been received by pressing a button on an IR remote control.
        /// </summary>
        /// <returns>A tuple consisting of the IR command code and its pressed state. A value of -1 for the IR command
        /// indicates none received. A value of true for the pressed state indicates button in pressed state.
        /// False indicates button released.</returns>
        public IrCommand GetIRCommand()
        {
            lock (_robotIO.IrCommandReceivedQueue)
            {
                if (_robotIO.IrCommandReceivedQueue.Count != 0)
                {
                    (int peekCmd, DateTime cmdTime) = _robotIO.IrCommandReceivedQueue.Peek();

                    if (_prevIRCommand == -1)
                    {
                        _robotIO.IrCommandReceivedQueue.Dequeue();
                        _lastIRCommandTime = cmdTime;
                        _prevIRCommand = peekCmd;
                        return new IrCommand { Code = peekCmd, ButtonPressed = true };
                    }
                    if (peekCmd != _prevIRCommand)
                    {
                        int tmpCmd = _prevIRCommand;
                        _prevIRCommand = -1;
                        return new IrCommand { Code = tmpCmd, ButtonPressed = false, ButtonReleased = true };
                    }
                    if (cmdTime.Subtract(_lastIRCommandTime).TotalSeconds < .15)
                    {
                        _robotIO.IrCommandReceivedQueue.Dequeue();
                        _lastIRCommandTime = cmdTime;
                        return new IrCommand { Code = -1, ButtonPressed = false };
                    }
                    else
                    {
                        _prevIRCommand = -1;
                    }
                }
                else if (_prevIRCommand != -1 && DateTime.Now.Subtract(_lastIRCommandTime).TotalSeconds > .15)
                {
                    int tmpCmd = _prevIRCommand;
                    _prevIRCommand = -1;
                    return new IrCommand { Code = tmpCmd, ButtonPressed = false, ButtonReleased = true };
                }
                return new IrCommand { Code = -1, ButtonPressed = false };
            }
        }


        /// <summary>
        /// Returns the digital inputs as a binary string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string val = string.Empty;

            for (int i = 0; i < _inputState.Length; i++)
            {
                val += _inputState[i] ? "1" : "0";
            }
            return val;
        }

        internal void QueueInputEvent(int inputEvent)
        {
            int idx = Input.EventSourceIndex(inputEvent);

            if (idx >= 0 && idx < _inputState.Length)
            {
                if (Input.Triggered(inputEvent))
                {
                    _inputState[idx] = true;
                }
                else if (Input.Released(inputEvent))
                {
                    _inputState[idx] = false;
                }
            }
            lock (_inputEventsReceivedQueue)
            {
                if (_inputEventsReceivedQueue.Count <= 18)
                {
                    if (Input.Sustained(inputEvent) && _inputEventsReceivedQueue.Contains(inputEvent))
                    {
                        // No need to enqueue sustained events if already queued.
                        return;
                    }
                    Debug.WriteLine("Queuing input event ...");
                    _inputEventsReceivedQueue.Enqueue(inputEvent);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPins"></param>
        /// <exception cref="ArgumentException">inputPins not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        internal void ValidateInputPins(params int[] inputPins)
        {
            if (inputPins == null || inputPins.Length == 0)
            {
                throw new ArgumentException($"Parameter {inputPins} not supplied.");
            }
            for (int i = 0; i < inputPins.Length; i++)
            {
                if (inputPins[i] < 0 || inputPins[i] > 4)
                {
                    throw new ArgumentOutOfRangeException($"Parameter {inputPins} contains out of range value.");
                }
            }
        }
    }
}
