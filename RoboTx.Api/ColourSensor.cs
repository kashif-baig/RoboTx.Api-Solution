namespace RoboTx.Api
{
    /// <summary>
    /// Reports RGB and HSL values detected using TCS34725 I2C sensor. HSL values may give better results
    /// when detecting colours. For each colour to detect, obtain reference values with the sensor close to the object,
    /// and with the sensor further away. Compare readings against the range of reference values to identify the colour.
    /// </summary>
    public sealed class ColourSensor
    {
        private object _lockObj = new object();
        private readonly RobotIO _robotIO;
        private int _red;
        private int _green;
        private int _blue;
        private int _clear;

        internal ColourSensor(RobotIO robotIO)
        {
            _robotIO = robotIO;
        }

        internal void SetRGBC(int r, int g, int b, int clear)
        {
            lock (_lockObj)
            {
                _red = r;
                _green = g;
                _blue = b;
                _clear = clear;
            }
        }

        /// <summary>
        /// Gets the red, green, blue and clear values reported by the colour sensor once it has been enabled.
        /// How long it takes for colour values to start being reported, and the subsequent reporting interval
        /// will depend in the integration time specified in the <see cref="Enable()">Enable</see> method.
        /// </summary>
        /// <returns>Tuple of (red, green, blue, clear).</returns>
        public RGBColour GetRGBC()
        {
            lock (_lockObj)
            {
                return new RGBColour { Red = _red, Green = _green, Blue = _blue, Clear = _clear };
            }
        }

        /// <summary>
        /// Gets the colour as H, S and L values reported by the colour sensor once it has been enabled.
        /// How long it takes for colour values to start being reported, and the subsequent reporting interval
        /// will depend in the integration time specified in the <see cref="Enable()">Enable</see> method.
        /// </summary>
        /// <returns>Tuple of (H, S, L) values.</returns>
        public HSLColour GetHSL()
        {
            // Code adapted from https://labex.io/tutorials/javascript-rgb-to-hsl-color-conversion-28603

            float r = 0, g = 0, b = 0;

            lock (_lockObj)
            {
                int divisor = _clear;

                if (divisor > 0)
                {
                    r = (float)_red / divisor;
                    g = (float)_green / divisor;
                    b = (float)_blue / divisor;
                }
            }

            // Clamp r,g,b values to 1 if >1.
            r = r > 1 ? 1 : r;
            g = g > 1 ? 1 : g;
            b = b > 1 ? 1 : b;

            float l = Math.Max(r, Math.Max(g, b));
            float s = l - Math.Min(r, Math.Min(g, b));
            float h = s != 0
                ? l == r
                    ? (g - b) / s
                    : l == g
                        ? 2 + (b - r) / s
                        : 4 + (r - g) / s
                : 0;

            return new HSLColour {Hue = 60 * h < 0 ? 60 * h + 360 : 60 * h,
                Saturation = 100 * (s != 0 ? l <= 0.5 ? s / (2 * l - s) : s / (2 - (2 * l - s)) : 0),
                Lightness = 100 * (2 * l - s) / 2
            };
        }

        /// <summary>
        /// Enables the colour sensor to report colour values, using integration time 50ms and gain of x4.
        /// See <see cref="Enable(int, int)">overloaded</see> method for specifying different integration time and gain values.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Enable()
        {
            Enable(TCS34725.INTEGRATION_TIME_50MS, TCS34725.GAIN_4X);
        }

        /// <summary>
        /// Enables the colour sensor to report colour values using specified integration time and gain.
        /// </summary>
        /// <param name="integrationTime">Integration time expressed as a value between 0 and 5 inclusive.
        /// Use static class <see cref="RoboTx.Api.TCS34725">TCS34725</see> that has symbolic labels for integration time values.</param>
        /// <param name="gain">Gain represebted as a value between 0 and 3 inclusive. Use static class <see cref="RoboTx.Api.TCS34725">TCS34725</see> that
        /// has symbolic labels for gain values.</param>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Enable(int integrationTime, int gain)
        {
            _robotIO.CheckSerialState();

            ColourMessage msg = new ColourMessage(_robotIO.StreamWriter);
            msg.Enable(integrationTime, gain);
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

        /// <summary>
        /// Stops the colour sensor reporting colour values.
        /// </summary>
        /// <exception cref="IOException">Serial port is in error state or not open.</exception>
        public void Disable()
        {
            _robotIO.CheckSerialState();

            ColourMessage msg = new ColourMessage(_robotIO.StreamWriter);
            msg.Disable();
            _robotIO.MessageSender.EnQueueMessage(msg);
        }

    }
}