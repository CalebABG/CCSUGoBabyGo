using GoBabyGoV2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GoBabyGoV2.Utilities
{
    public class AccelMonitor
    {
        public static float[] AccelCalibDefault = { -0.86f, 1.17f, -1.03f, 1.01f };

        public static float AccelMinX { get; set; } = -0.86f;
        public static float AccelMaxX { get; set; } = 1.17f;
        public static float AccelMinY { get; set; } = -1.03f;
        public static float AccelMaxY { get; set; } = 1.01f;

        public static void StartAccelMonitor()
        {
            if (Accelerometer.IsMonitoring == false) Accelerometer.Start(CarControlPage.SensorSpeed);
        }

        public static void StopAccelMonitor()
        {
            if (Accelerometer.IsMonitoring) Accelerometer.Stop();
        }

        #region UtilityFunctions
        public static byte[] IntToBytes(int val)
        {
            byte[] b = new byte[4];

            b[0] = (byte)val;
            b[1] = (byte)(((uint)val >> 8) & 0xFF);
            b[2] = (byte)(((uint)val >> 16) & 0xFF);
            b[3] = (byte)(((uint)val >> 24) & 0xFF);

            return b;
        }

        public static double Constrain(double amt, double low, double high)
        {
            return (amt) < (low) ? (low) : (amt) > (high) ? (high) : (amt);
        }

        public static double Map(double value, double min1, double max1, double min2, double max2)
        {
            return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        }

        #endregion
    }
}
