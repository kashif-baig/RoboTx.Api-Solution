namespace RoboTx.Api
{
    /// <summary>
    /// Analog input converter
    /// </summary>
    /// <param name="inputValue">Raw analog input value</param>
    /// <returns>The converted value.</returns>
    public delegate float AnalogConverter(float inputValue);

    /// <summary>
    /// Configures and reports analog inputs via Arduino pins A0 to A7.
    /// </summary>
    public sealed class Analog
    {
        readonly private RobotIO _robotIO;

        internal Analog(RobotIO robotIO)
        {
            _robotIO = robotIO;
            A0 = new AnalogInput();
            A1 = new AnalogInput();
            A2 = new AnalogInput();
            A3 = new AnalogInput();
            A4 = new AnalogInput();
            A5 = new AnalogInput();
            A6 = new AnalogInput();
            A7 = new AnalogInput();
        }


        /// <summary>
        /// Assigns a function to one or more analog inputs to convert the normal range of analog values from 0 to 1023,
        /// to another range of values.
        /// </summary>
        /// <param name="converter">A function that converts the analog values to another range of values.</param>
        /// <param name="input">One or more analog inputs.</param>
        /// <exception cref="ArgumentNullException">converter is null or input is null.</exception>
        public void UseConverter(AnalogConverter converter, params AnalogInput[] input)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == null)
                {
                    throw new ArgumentNullException(nameof(input));
                }
                input[i].Convert = converter;
            }
        }


        /// <summary>
        /// Sets the rate at which the analog pins are sampled.
        /// </summary>
        /// <param name="sampleRateHz">Sample rate in hertz in the range of 1 to 50.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentOutOfRangeException">sampleRateHz has value less than 1 or greater than 50.</exception>
        public void SetSampleRate(int sampleRateHz)
        {
            _robotIO.CheckSerialState();
            AnalogMessage msg = new AnalogMessage(_robotIO.StreamWriter);
            msg.SetSampleRate(sampleRateHz);

            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Enables analog inputs whose sources are Arduino pins A0, A1, A2, A3, A4, A5, A6 and A7.
        /// The default sample rate is 10Hz.
        /// A short delay may need to be implemented after a call to this method to allow analog values to start being received.
        /// Enable only the input sources that are needed.
        /// Digital inputs for the specified Arduino pins are disabled.
        /// </summary>
        /// <param name="inputPins">Any combination of pins from the set of {0,1,2,3,4,5,6,7}.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentException">inputPins is null or not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void EnableInputsA(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            _robotIO.CheckSerialState();
            ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
            msg.EnableAnalogInputs(true, inputPins);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Disables analog inputs whose sources are Arduino pins A0, A1, A2, A3, A4, A5, A6 and A7.
        /// Digital inputs for the specified Arduino pins are also disabled.
        /// </summary>
        /// <param name="inputPins">Any combination of pins from the set of {0,1,2,3,4,5,6,7}.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        /// <exception cref="ArgumentException">inputPins is null or not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        public void DisableInputsA(params int[] inputPins)
        {
            ValidateInputPins(inputPins);

            _robotIO.CheckSerialState();
            ConfigMessage msg = new ConfigMessage(_robotIO.StreamWriter);
            msg.EnableAnalogInputs(false, inputPins);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Gets the analog reading of pin A0 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A0 { get; }
        /// <summary>
        /// Gets the analog reading of pin A1 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A1 { get;  }
        /// <summary>
        /// Gets the analog reading of pin A2 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A2 { get;  }
        /// <summary>
        /// Gets the analog reading of pin A3 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A3 { get;  }
        /// <summary>
        /// Gets the analog reading of pin A4 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A4 { get; }
        /// <summary>
        /// Gets the analog reading of pin A5 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A5 { get;  }
        /// <summary>
        /// Gets the analog reading of pin A6 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A6 { get;  }
        /// <summary>
        /// Gets the analog reading of pin A7 whose value is between 0 and 1023 inclusive.
        /// The pin must be <see cref="EnableInputsA(int[])">enabled</see> for analog input before values become available.
        /// </summary>
        public AnalogInput A7 { get;  }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPins"></param>
        /// <exception cref="ArgumentException">inputPins is null or not supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">inputPins contains out of range value.</exception>
        internal void ValidateInputPins(params int[] inputPins)
        {
            if (inputPins == null || inputPins.Length == 0)
            {
                throw new ArgumentException($"Parameter {inputPins} not supplied.");
            }
            for (int i = 0; i < inputPins.Length; i++)
            {
                if (inputPins[i] < 0 || inputPins[i] > 7)
                {
                    throw new ArgumentOutOfRangeException($"Parameter {inputPins} contains out of range value.");
                }
            }
        }

    }
}
