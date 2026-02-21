namespace RoboTx.Api
{
    /// <summary>
    /// Infra red remote sensor command converter. Use for converting command codes to a string value.
    /// </summary>
    /// <param name="irCode">IR remote command code received.</param>
    /// <returns></returns>
    public delegate string IrCommandConverter(int irCode);

    /// <summary>
    /// Represents IR Command received by the IR remote sensor.
    /// </summary>
    public struct IrCommand
    {
        internal Digital Digital {  get; set; }

        /// <summary>
        /// Gets the IR command code.
        /// </summary>
        public int Code { get; internal set; }
        /// <summary>
        /// Gets the pressed state of the command button. True indicates pressed.
        /// </summary>
        public bool ButtonPressed { get; internal set; }
        /// <summary>
        /// Gets the released state of the command button. True indicates released.
        /// </summary>
        public bool ButtonReleased { get; internal set; }

        /// <summary>
        /// Returns true if an IR command has been received.
        /// </summary>
        public bool Received { get => Code >= 0; }

        /// <summary>
        /// Uese the registered <see cref="IrCommandConverter">IrCommandConverter</see> function to convert a received IR command code to a string value.
        /// </summary>
        public string Name { get => Digital?.Convert == null || !Received ? string.Empty : Digital.Convert(Code); }

        /// <summary>
        /// Unpacks the IR Command tuple.
        /// </summary>
        /// <param name="code">IR command code</param>
        /// <param name="name">String representation of command code.</param>
        /// <param name="buttonPressed">Command button pressed state.</param>
        /// <param name="buttonReleased">Command button released state.</param>
        public void Deconstruct(out int code, out string name, out bool buttonPressed, out bool buttonReleased)
        {
            code = Code;
            name = Name;
            buttonPressed = ButtonPressed;
            buttonReleased = ButtonReleased;
        }

        /// <summary>
        /// Unpacks the IR Command tuple.
        /// </summary>
        /// <param name="code">IR command code</param>
        /// <param name="buttonPressed">Command button pressed state.</param>
        /// <param name="buttonReleased">Command button released state.</param>
        public void Deconstruct(out int code, out bool buttonPressed, out bool buttonReleased)
        {
            code = Code;
            buttonPressed = ButtonPressed;
            buttonReleased = ButtonReleased;
        }
    }
}
