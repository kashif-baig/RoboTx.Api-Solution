namespace RoboTx.Api
{
    /// <summary>
    /// Represents the value of a given digital input.
    /// </summary>
    public class DigitalInput
    {
        private Digital _digital;
        private int _index;

        internal DigitalInput(Digital digital, int index)
        {
            _digital = digital;
            _index = index;
        }

        /// <summary>
        /// Gets the digital input value.
        /// </summary>
        public bool Value { get => _digital.InputState[_index]; }
        /// <summary>
        /// Implicit bool cast operator.
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator bool(DigitalInput d) => d.Value;
        /// <summary>
        /// Returns the string representation of the value.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Value}";
    }
}
