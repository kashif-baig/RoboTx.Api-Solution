namespace RoboTx.Api
{
    /// <summary>
    /// Represents IR Command received by the IR remote sensor.
    /// </summary>
    public struct IrCommand
    {
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
