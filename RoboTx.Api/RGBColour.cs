namespace RoboTx.Api
{
    /// <summary>
    /// Represents colour as Red, Green, Blue and Clear values.
    /// </summary>
    public struct RGBColour
    {
        /// <summary>
        /// Red component.
        /// </summary>
        public int Red { get; internal set; }
        /// <summary>
        /// Green component.
        /// </summary>
        public int Green { get; internal set; }
        /// <summary>
        /// Blue component.
        /// </summary>
        public int Blue { get; internal set; }
        /// <summary>
        /// Clear component.
        /// </summary>
        public int Clear { get; internal set; }

        /// <summary>
        /// Unpacks RGBC tuple.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="clear"></param>
        public void Deconstruct(out int red, out int green, out int blue, out int clear)
        {
            red = Red;
            green = Green;
            blue = Blue;
            clear = Clear;
        }
    }
}
