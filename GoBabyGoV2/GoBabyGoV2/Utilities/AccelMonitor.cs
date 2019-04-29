using GoBabyGoV2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoBabyGoV2.Utilities
{
    public class AccelMonitor
    {
        // Set of method references (event handlers) for Accelerometer changes
        public static HashSet<EventHandler<AccelerometerChangedEventArgs>> AccelerometerChangedEvents 
            = new HashSet<EventHandler<AccelerometerChangedEventArgs>>();

        // Set speed delay for monitoring changes. 
        public static SensorSpeed SensorSpeed { get; } = SensorSpeed.Game;

        // Default sensor calibration values for X and Y (from OnePlus 6T Min/Max X and Y values)
        public static readonly float[] AccelCalibDefault = { -0.86f, 1.17f, -1.03f, 1.01f };

        // Static Property for Xaml binding to get/set sensor calibration values
        public static AccelCalibObj AccelCalib = new AccelCalibObj();

        /// <summary>
        /// This method will add the given event handler to the <see cref="AccelerometerChangedEvents"/>
        /// set to keep a reference, if it is not null. Also, if the event isn't null, it will be added to the
        /// <see cref="Accelerometer.ReadingChanged"/>'s event handler; first removing it to handle the possibility of
        /// duplicate subscriptions.
        /// </summary>
        /// <param name="accelEventHandler"></param>
        public static void AddAccelerometerCallback(EventHandler<AccelerometerChangedEventArgs> accelEventHandler)
        {
            if (accelEventHandler == null) return;

            AccelerometerChangedEvents.Add(accelEventHandler);

            try
            {
                Accelerometer.ReadingChanged -= accelEventHandler;
                Accelerometer.ReadingChanged += accelEventHandler;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Method to start the Xamarin.Essentials Accelerometer Service, if the service hasn't already been started.
        /// Please make sure that either an existing callback/event handler for the Accelerometer has been added
        /// <see cref="Accelerometer.ReadingChanged"/>
        /// </summary>
        public static void StartAccelMonitor()
        {
            if (Accelerometer.IsMonitoring == false) Accelerometer.Start(SensorSpeed);
        }

        /// <summary>
        /// Method to stop the Xamarin.Essentials Accelerometer Service, if the service is running.
        /// This will stop the service and attempt to unsubscribe any event handlers that are in the <see cref="AccelerometerChangedEvents"/>
        /// set. If successful, it will also clear the set.
        /// </summary>
        public static void StopAccelMonitor()
        {
            if (!Accelerometer.IsMonitoring) return;

            Accelerometer.Stop();

            try
            {
                if (!AccelerometerChangedEvents.Any()) return;

                foreach (var accelerometerChangedEvent in AccelerometerChangedEvents)
                    Accelerometer.ReadingChanged -= accelerometerChangedEvent;

                AccelerometerChangedEvents.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
