namespace RoboTx.Api
{
    /// <summary>
    /// Represents the value of a given analog input.
    /// </summary>
    public class AnalogInput
    {
        private volatile float _value = 0;

        internal AnalogInput()
        {
            Convert = null;
        }

        /// <summary>
        /// Gets the analog inpput value.
        /// </summary>
        public float Value { get => Convert==null ? _value : Convert(_value); internal set => _value = value; }
        /// <summary>
        /// Implicit float cast operator.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator float(AnalogInput a) => a.Value;

        internal Func<float, float> Convertx { get; set; }

        internal AnalogConverter Convert { get; set; }

        /// <summary>
        /// Returns the string representation of the value.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Value}";
    }
}
