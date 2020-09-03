namespace GoBabyGoV2.Utilities
{
    public class Utilities
    {
        #region UtilityFunctions

        public static byte[] IntToBytes(int val)
        {
            var b = new byte[4];

            b[0] = (byte) val;
            b[1] = (byte) (((uint) val >> 8) & 0xFF);
            b[2] = (byte) (((uint) val >> 16) & 0xFF);
            b[3] = (byte) (((uint) val >> 24) & 0xFF);

            return b;
        }

        public static double Constrain(double amt, double low, double high)
        {
            return amt < low ? low : amt > high ? high : amt;
        }

        public static double Map(double value, double min1, double max1, double min2, double max2)
        {
            return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        }

        #endregion
    }
}