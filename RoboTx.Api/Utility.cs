namespace RoboTx.Api
{
    internal class Utility
    {
        public static int Constrain(int x, int a, int b)
        {
            if (x < a) return a;
            if (x > b) return b;
            return x;
        }

        public static float Constrain(float x, float a, float b)
        {
            if (x < a) return a;
            if (x > b) return b;
            return x;
        }
    }
}
