using GoBabyGoV2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        #region Methods

        public void SetAccelerometerChangeEvent(EventHandler<AccelerometerChangedEventArgs> accelerometerChangedEvent = null)
        {
            if (accelerometerChangedEvent == null) return;

            AccelerometerChangedEvent += accelerometerChangedEvent;
            Accelerometer.ReadingChanged += AccelerometerChangedEvent;
        }

        /// <summary>
        /// Method to start the Xamarin.Essentials Accelerometer Service, if the service hasn't already been started.
        /// Please make sure that either an existing callback/event handler for the Accelerometer has been added
        /// <see cref="Accelerometer.ReadingChanged"/>
        /// </summary>
        public void StartMonitoring()
        {
            if (Accelerometer.IsMonitoring == false) Accelerometer.Start(SensorSpeed);
        }

        /// <summary>
        /// Method to stop the Xamarin.Essentials Accelerometer Service, if the service is running.
        /// This will stop the service and attempt to unsubscribe any event handlers that are in the <see cref="AccelerometerChangedEvents"/>
        /// set. If successful, it will also clear the set.
        /// </summary>
        public void StopMonitoring()
        {
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

        public void UpdateCalibrationAxisX(float val, Update updateFunc)
        {
            updateFunc?.Invoke(val);
        }

        public void UpdateCalibrationAxisY(float val, Update updateFunc)
        {
            updateFunc?.Invoke(val);
        }
    }
}
