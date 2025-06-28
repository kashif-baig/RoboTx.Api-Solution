namespace RoboTx.Api
{
    /// <summary>
    /// Input constants representing the pressing, holding and releasing of buttons, or triggering
    /// of contacts on Arduino pins defined in the firmware.
    /// </summary>
    public static class Input
    {
        private const int BUTTON_PRESSED_IND = 0 << 6;
        private const int BUTTON_SHORT_RELEASE_IND = 1 << 6;
        private const int BUTTON_SUSTAINED_IND = 2 << 6;
        private const int BUTTON_SUSTAIN_RELEASE_IND = 3 << 6;

        internal const int SOURCE_IN0 = 1;

        /// <summary>
        /// Button 1 was pressed.
        /// </summary>
        public const int BUTTON_1_PRESSED = SOURCE_IN0 | BUTTON_PRESSED_IND;
        /// <summary>
        /// Button 1 was released after a short press. 
        /// </summary>
        public const int BUTTON_1_RELEASED = SOURCE_IN0 | BUTTON_SHORT_RELEASE_IND;
        /// <summary>
        /// Button 1 is held down.
        /// </summary>
        public const int BUTTON_1_SUSTAINED = SOURCE_IN0 | BUTTON_SUSTAINED_IND;
        /// <summary>
        /// Button 1 was released after a long press.
        /// </summary>
        public const int BUTTON_1_SUSTAIN_RELEASED = SOURCE_IN0 | BUTTON_SUSTAIN_RELEASE_IND;

        internal const int SOURCE_IN1 = 2;
        /// <summary>
        /// Button 2 was pressed.
        /// </summary>
        public const int BUTTON_2_PRESSED = SOURCE_IN1 | BUTTON_PRESSED_IND;
        /// <summary>
        /// Button 2 was released after a short press. 
        /// </summary>
        public const int BUTTON_2_RELEASED = SOURCE_IN1 | BUTTON_SHORT_RELEASE_IND;
        /// <summary>
        /// Button 2 is held down.
        /// </summary>
        public const int BUTTON_2_SUSTAINED = SOURCE_IN1 | BUTTON_SUSTAINED_IND;
        /// <summary>
        /// Button 2 was released after a long press.
        /// </summary>
        public const int BUTTON_2_SUSTAIN_RELEASED = SOURCE_IN1 | BUTTON_SUSTAIN_RELEASE_IND;

        internal const int SOURCE_IN2 = 3;
        /// <summary>
        /// Button 3 was pressed.
        /// </summary>
        public const int BUTTON_3_PRESSED = SOURCE_IN2 | BUTTON_PRESSED_IND;
        /// <summary>
        /// Button 3 was released after a short press. 
        /// </summary>
        public const int BUTTON_3_RELEASED = SOURCE_IN2 | BUTTON_SHORT_RELEASE_IND;
        /// <summary>
        /// Button 3 is held down.
        /// </summary>
        public const int BUTTON_3_SUSTAINED = SOURCE_IN2 | BUTTON_SUSTAINED_IND;
        /// <summary>
        /// Button 3 was released after a long press.
        /// </summary>
        public const int BUTTON_3_SUSTAIN_RELEASED = SOURCE_IN2 | BUTTON_SUSTAIN_RELEASE_IND;

        internal const int SOURCE_IN3 = 4;
        /// <summary>
        /// Button 4 was pressed.
        /// </summary>
        private const int BUTTON_4_PRESSED = SOURCE_IN3 | BUTTON_PRESSED_IND;
        ///// <summary>
        ///// Button 4 was released after a short press. 
        ///// </summary>
        //private const int BUTTON_4_RELEASED = SOURCE_IN3 | BUTTON_SHORT_RELEASE_IND;
        ///// <summary>
        ///// Button 4 is held down.
        ///// </summary>
        //private const int BUTTON_4_SUSTAINED = SOURCE_IN3 | BUTTON_SUSTAINED_IND;
        ///// <summary>
        ///// Button 4 was released after a long press.
        ///// </summary>
        //private const int BUTTON_4_SUSTAIN_RELEASED = SOURCE_IN3 | BUTTON_SUSTAIN_RELEASE_IND;

        internal const int SOURCE_IN4 = 5;
        private const int BUTTON_5_PRESSED = SOURCE_IN4 | BUTTON_PRESSED_IND;
        //private const int BUTTON_5_RELEASED = SOURCE_IN4 | BUTTON_SHORT_RELEASE_IND;
        //private const int BUTTON_5_SUSTAINED = SOURCE_IN4 | BUTTON_SUSTAINED_IND;
        //private const int BUTTON_5_SUSTAIN_RELEASED = SOURCE_IN4 | BUTTON_SUSTAIN_RELEASE_IND;

        /// <summary>
        /// No digital input signal was detected.
        /// </summary>
        public const int None = -1;

        /// <summary>
        /// Digital signal was detected on IN0.
        /// </summary>
        public const int IN0_TRIGGERED = BUTTON_1_PRESSED;
        ///// <summary>
        ///// Digital signal was released on IN0.
        ///// </summary>
        //public const int IN0_RELEASED = BUTTON_1_RELEASED;
        ///// <summary>
        ///// Sustained digital signal was detected on IN0.
        ///// </summary>
        //public const int IN0_SUSTAINED = BUTTON_1_SUSTAINED;
        ///// <summary>
        ///// Digital signal was released on IN0 after sustained signal.
        ///// </summary>
        //public const int IN0_SUSTAIN_RELEASED = BUTTON_1_SUSTAIN_RELEASED;

        /// <summary>
        /// Digital signal was detected on IN1.
        /// </summary>
        public const int IN1_TRIGGERED = BUTTON_2_PRESSED;
        ///// <summary>
        ///// Digital signal was released on IN1.
        ///// </summary>
        //public const int IN1_RELEASED = BUTTON_2_RELEASED;
        ///// <summary>
        ///// Sustained digital signal was detected on IN1.
        ///// </summary>
        //public const int IN1_SUSTAINED = BUTTON_2_SUSTAINED;
        ///// <summary>
        ///// Digital signal was released on IN1 after sustained signal.
        ///// </summary>
        //public const int IN1_SUSTAIN_RELEASED = BUTTON_2_SUSTAIN_RELEASED;

        /// <summary>
        /// Digital signal was detected on IN2.
        /// </summary>
        public const int IN2_TRIGGERED = BUTTON_3_PRESSED;
        ///// <summary>
        ///// Digital signal was released on IN2.
        ///// </summary>
        //public const int IN2_RELEASED = BUTTON_3_RELEASED;
        ///// <summary>
        ///// Sustained digital signal was detected on IN2.
        ///// </summary>
        //public const int IN2_SUSTAINED = BUTTON_3_SUSTAINED;
        ///// <summary>
        ///// Digital signal was released on IN2 after sustained signal.
        ///// </summary>
        //public const int IN2_SUSTAIN_RELEASED = BUTTON_3_SUSTAIN_RELEASED;

        /// <summary>
        /// Digital signal was detected on IN3.
        /// </summary>
        public const int IN3_TRIGGERED = BUTTON_4_PRESSED;
        ///// <summary>
        ///// Digital signal was released on IN3.
        ///// </summary>
        //public const int IN3_RELEASED = BUTTON_4_RELEASED;
        ///// <summary>
        ///// Sustained digital signal was detected on IN3.
        ///// </summary>
        //public const int IN3_SUSTAINED = BUTTON_4_SUSTAINED;
        ///// <summary>
        ///// Digital signal was released on IN3 after sustained signal.
        ///// </summary>
        //public const int IN3_SUSTAIN_RELEASED = BUTTON_4_SUSTAIN_RELEASED;

        /// <summary>
        /// Digital signal was detected on IN4.
        /// </summary>
        public const int IN4_TRIGGERED = BUTTON_5_PRESSED;
        ///// <summary>
        ///// Digital signal was released on IN4.
        ///// </summary>
        //public const int IN4_RELEASED = BUTTON_5_RELEASED;
        ///// <summary>
        ///// Sustained digital signal was detected on IN4.
        ///// </summary>
        //public const int IN4_SUSTAINED = BUTTON_5_SUSTAINED;
        ///// <summary>
        ///// Digital signal was released on IN4 after sustained signal.
        ///// </summary>
        //public const int IN4_SUSTAIN_RELEASED = BUTTON_5_SUSTAIN_RELEASED;

        internal static int EventSource(int inputEvent)
        {
            return inputEvent & 0b00111111;
        }

        internal static int EventSourceIndex(int inputEvent)
        {
            int eventSource = EventSource(inputEvent);
            int index = -1;

            switch (eventSource)
            {
                case SOURCE_IN0:
                    index = 0;
                    break;
                case SOURCE_IN1:
                    index = 1;
                    break;
                case SOURCE_IN2:
                    index = 2;
                    break;
                case SOURCE_IN3:
                    index = 3;
                    break;
                case SOURCE_IN4:
                    index = 4;
                    break;
            }
            return index;
        }

        internal static bool Triggered(int inputEvent)
        {
            return inputEvent > 0 && ((inputEvent & 0b11000000) == BUTTON_PRESSED_IND || (inputEvent & 0b11000000) == BUTTON_SUSTAINED_IND);
        }

        internal static bool Sustained(int inputEvent)
        {
            return inputEvent > 0 && (inputEvent & 0b11000000) == BUTTON_SUSTAINED_IND;
        }

        internal static bool Released(int inputEvent)
        {
            return (inputEvent & 0b11000000) == BUTTON_SHORT_RELEASE_IND || (inputEvent & 0b11000000) == BUTTON_SUSTAIN_RELEASE_IND;
        }
    }
}
