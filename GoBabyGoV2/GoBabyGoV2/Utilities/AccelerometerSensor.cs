using GoBabyGoV2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoBabyGoV2.Utilities
{
    public class AccelerometerSensor : IAccelerometerCalibrationChanged, IDisposable 
    {
        #region StaticProperties

        public static AccelerometerSensor Monitor { get; set; } = new AccelerometerSensor();


        // Set speed delay for monitoring changes. Only set from this class
        public static SensorSpeed SensorSpeed { get; } = SensorSpeed.Game;

        // Default sensor calibration values for X and Y (from OnePlus 6T Min/Max X and Y values)
        public static float[] DefaultCalibration { get; } = { -1f, 1f, -1f, 1f };

        // Axis to freeze when calibrating
        public static CalibrationFreeze CalibrationFreezeAxis { get; set; } = CalibrationFreeze.None;

        public static bool ShouldCalibrate { get; set; }

        #endregion

        #region PublicProperties

        // Set of method references (event handlers) for Accelerometer changes
        public event EventHandler<AccelerometerChangedEventArgs> AccelerometerChangedEvent;

        // Property for Xaml binding to get/set sensor calibration values
        public AccelerometerCalibration Calibration { get; set; } = new AccelerometerCalibration(-1, 1, -1, 1);

        #endregion

        #region AccelMethods

        public void SetAccelerometerChangeEvent(EventHandler<AccelerometerChangedEventArgs> accelerometerChangedEvent = null)
        {
            if (accelerometerChangedEvent == null) return;

            AccelerometerChangedEvent += accelerometerChangedEvent;
            Accelerometer.ReadingChanged += AccelerometerChangedEvent;
        }

        public void StartMonitoring()
        {
            // If already monitoring, return (without check will throw exception because it's not handled in source of Accelerometer)
            if (Accelerometer.IsMonitoring == true) return;

            // The only time the accelerometer shouldn't start is if it's in an iOS emulator
            if (!(DependencyService.Get<IEmulatorDetect>().IsRunningInEmulator() &&
                  Device.RuntimePlatform == Device.iOS))
            {
                Accelerometer.Start(SensorSpeed);
            }
        }

        public void StopMonitoring()
        {
            // If monitoring, stop
            if (Accelerometer.IsMonitoring == true) Accelerometer.Stop();
        }

        #endregion

        #region OnDispose

        public void Dispose()
        {
            if (AccelerometerChangedEvent == null) return;

            // Remove event-handler from Accelerometer.ReadingChanged event
            Accelerometer.ReadingChanged -= AccelerometerChangedEvent;

            // Remove all event-handlers from AccelerometerChangedEvent
            foreach (var _delegate in AccelerometerChangedEvent.GetInvocationList())
            {
                if (_delegate != null && 
                    _delegate is EventHandler<AccelerometerChangedEventArgs> accelEventHandler)
                {
                    AccelerometerChangedEvent -= accelEventHandler;
                }
            }
        }

        #endregion

        #region AccelInterfaceImplementation

        public void UpdateCalibrationAxisX(float val, Update updateFunc)
        {
            updateFunc?.Invoke(val);
        }

        public void UpdateCalibrationAxisY(float val, Update updateFunc)
        {
            updateFunc?.Invoke(val);
        }

        #endregion
    }
}
