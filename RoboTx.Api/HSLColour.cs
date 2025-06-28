namespace RoboTx.Api
{
    /// <summary>
    /// Represents colour as Hue, Saturation and Lightness values.
    /// </summary>
    public struct HSLColour
    {
        /// <summary>
        /// Hue component.
        /// </summary>
        public float Hue { get; internal set; }
        /// <summary>
        /// Saturation component.
        /// </summary>
        public float Saturation { get; internal set; }
        /// <summary>
        /// Saturation component.
        /// </summary>
        public float Lightness { get; internal set; }

        /// <summary>
        /// Unpacks HSL Colour tuple.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="lightness"></param>
        public void Deconstruct(out float hue, out float saturation, out float lightness)
        {
            hue = Hue;
            saturation = Saturation;
            lightness = Lightness;
        }
    }
}
